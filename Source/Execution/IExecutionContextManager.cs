// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Runtime.CompilerServices;
using Dolittle.Applications;
using Dolittle.Security;
using Dolittle.Tenancy;

namespace Dolittle.Execution
{
    /// <summary>
    /// Defines the manager for <see cref="ExecutionContext"/>.
    /// </summary>
    public interface IExecutionContextManager
    {
        /// <summary>
        /// Gets the current <see cref="ExecutionContext"/>.
        /// </summary>
        ExecutionContext Current { get; }

        /// <summary>
        /// Set constants that are used typically within a running process.
        /// </summary>
        /// <param name="application">Which <see cref="Application"/> that is running.</param>
        /// <param name="boundedContext">Which <see cref="BoundedContext"/> that is running.</param>
        /// <param name="environment">Which <see cref="Environment"/> the system is running in.</param>
        void SetConstants(Application application, BoundedContext boundedContext, Environment environment);

        /// <summary>
        /// Sets the current execution context to be for the system.
        /// </summary>
        /// <param name="filePath">FilePath of the caller.</param>
        /// <param name="lineNumber">Linenumber in the file of the caller.</param>
        /// <param name="member">Membername of the caller.</param>
        /// <returns>Current <see cref="ExecutionContext"/>.</returns>
        ExecutionContext System([CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string member = "");

        /// <summary>
        /// Sets the current execution context to be for the system.
        /// </summary>
        /// <param name="correlationId"><see cref="CorrelationId"/> to associate.</param>
        /// <param name="filePath">FilePath of the caller.</param>
        /// <param name="lineNumber">Linenumber in the file of the caller.</param>
        /// <param name="member">Membername of the caller.</param>
        /// <returns>Current <see cref="ExecutionContext"/>.</returns>
        ExecutionContext System(CorrelationId correlationId, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string member = "");

        /// <summary>
        /// Set current execution context for a <see cref="TenantId"/>.
        /// </summary>
        /// <param name="context"><see cref="ExecutionContext"/> to set for.</param>
        /// <param name="filePath">FilePath of the caller.</param>
        /// <param name="lineNumber">Linenumber in the file of the caller.</param>
        /// <param name="member">Membername of the caller.</param>
        /// <returns>Current <see cref="ExecutionContext"/>.</returns>
        ExecutionContext CurrentFor(ExecutionContext context, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string member = "");

        /// <summary>
        /// Set current execution context for a <see cref="TenantId"/>.
        /// </summary>
        /// <param name="tenant"><see cref="TenantId"/> to set for.</param>
        /// <param name="filePath">FilePath of the caller.</param>
        /// <param name="lineNumber">Linenumber in the file of the caller.</param>
        /// <param name="member">Membername of the caller.</param>
        /// <returns>Current <see cref="ExecutionContext"/>.</returns>
        ExecutionContext CurrentFor(TenantId tenant, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string member = "");

        /// <summary>
        /// Set current execution context for a <see cref="TenantId"/>.
        /// </summary>
        /// <param name="tenant"><see cref="TenantId"/> to set for.</param>
        /// <param name="correlationId"><see cref="CorrelationId"/> to associate.</param>
        /// <param name="filePath">FilePath of the caller.</param>
        /// <param name="lineNumber">Linenumber in the file of the caller.</param>
        /// <param name="member">Membername of the caller.</param>
        /// <returns>Current <see cref="ExecutionContext"/>.</returns>
        ExecutionContext CurrentFor(TenantId tenant, CorrelationId correlationId, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string member = "");

        /// <summary>
        /// Set current execution context for a <see cref="TenantId"/> with <see cref="CorrelationId"/> and <see cref="Claims"/>.
        /// </summary>
        /// <param name="tenant"><see cref="TenantId"/> to set for.</param>
        /// <param name="correlationId"><see cref="CorrelationId"/> to associate.</param>
        /// <param name="claims"><see cref="Claims"/> to assocatie.</param>
        /// <param name="filePath">FilePath of the caller.</param>
        /// <param name="lineNumber">Linenumber in the file of the caller.</param>
        /// <param name="member">Membername of the caller.</param>
        /// <returns>Current <see cref="ExecutionContext"/>.</returns>
        ExecutionContext CurrentFor(TenantId tenant, CorrelationId correlationId, Claims claims, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string member = "");
    }
}