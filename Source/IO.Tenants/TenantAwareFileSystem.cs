/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Dolittle.Execution;

namespace Dolittle.IO.Tenants
{  
    /// <summary>
    /// Represents an implementation of <see cref="ITenantAwareFileSystem"/>
    /// </summary>
    /// <remarks>
    /// https://en.wikipedia.org/wiki/Directory_traversal_attack
    /// </remarks>
    public class TenantAwareFileSystem : ITenantAwareFileSystem
    {
        static Regex _windowsPathRooted = new Regex("^[a-z]*:");

        readonly IExecutionContextManager _executionContextManager;
        readonly IFileSystem _fileSystem;
        private readonly TenantAwareFileSystemConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of <see cref="TenantAwareFileSystem"/>
        /// </summary>
        /// <param name="executionContextManager"><see cref="IExecutionContextManager"/> to use for determining the current tenant</param>
        /// <param name="configuration"><see cref="TenantAwareFileSystemConfiguration">Configuration</see></param>
        /// <param name="fileSystem">Underlying <see cref="IFileSystem"/></param>
        public TenantAwareFileSystem(
            IExecutionContextManager executionContextManager,
            TenantAwareFileSystemConfiguration configuration,
            IFileSystem fileSystem)
        {
            _executionContextManager = executionContextManager;
            _fileSystem = fileSystem;
            _configuration = configuration;
        }

        /// <inheritdoc/>
        public IEnumerable<string> GetDirectoriesIn(string relativePath)
        {
            var absolutePath = MapPath(relativePath);
            ThrowIfAccessingOutsideTenantSandbox(relativePath, absolutePath);
            var directories = _fileSystem.GetDirectoriesIn(absolutePath);
            var tenantBasePath = GetTenantBasePath();
            var relativeDirectories = directories.Select(_ => {
                var relativeDir = _.Substring(tenantBasePath.Length);
                if( relativeDir.StartsWith("/") || relativeDir.StartsWith("\\"))
                    relativeDir = relativeDir.Substring(1);

                return relativeDir;
            }).ToArray();
            return relativeDirectories;
        }

        /// <inheritdoc/>
        public bool DirectoryExists(string relativePath)
        {
            var absolutePath = MapPath(relativePath);
            ThrowIfAccessingOutsideTenantSandbox(relativePath, absolutePath);
            return _fileSystem.DirectoryExists(absolutePath);
        }

        /// <inheritdoc/>
        public bool Exists(string relativePath)
        {
            var absolutePath = MapPath(relativePath);
            ThrowIfAccessingOutsideTenantSandbox(relativePath, absolutePath);
            return _fileSystem.Exists(absolutePath);
        }

        /// <inheritdoc/>
        public string ReadAllText(string relativePath)
        {
            var absolutePath = MapPath(relativePath);
            ThrowIfAccessingOutsideTenantSandbox(relativePath, absolutePath);           
            return _fileSystem.ReadAllText(absolutePath);
        }


        /// <inheritdoc/>
        public void WriteAllText(string relativePath, string content)
        {
            var absolutePath = MapPath(relativePath);
            ThrowIfAccessingOutsideTenantSandbox(relativePath, absolutePath);
            _fileSystem.WriteAllText(absolutePath, content);
        }

        string GetTenantBasePath()
        {
            return Path.Combine(
                _configuration.RootPath,
                _executionContextManager.Current.Tenant.ToString());

        }

        string MapPath(string relativePath)
        {
            return Path.Combine(GetTenantBasePath(), relativePath);
        }

        void ThrowIfAccessingOutsideTenantSandbox(string relativePath, string absolutePath)
        {
            absolutePath = absolutePath.Replace('\\','/');
            var tenantPath = GetTenantBasePath().Replace('\\','/');
            var fullAbsolutePath = Path.GetFullPath(absolutePath);
            var fullBasePath = Path.GetFullPath(tenantPath);
            
            // Note: Path.IsPathRooted would've been perfect for this. But it is platform specific at runtime and 
            // don't feel comfortable having specs that are platform specific for this - so hand-rolling the support because of that
            var outside = 
                relativePath.StartsWith("..") ||
                relativePath.StartsWith("\\") ||
                relativePath.StartsWith("/") ||
                _windowsPathRooted.IsMatch(relativePath) ||
                !fullAbsolutePath.StartsWith(fullBasePath);                

            if( outside ) throw new AccessOutsideSandboxDenied(relativePath);
        }
    }
}