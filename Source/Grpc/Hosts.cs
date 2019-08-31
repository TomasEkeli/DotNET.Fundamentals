/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using Dolittle.Collections;
using Dolittle.DependencyInversion;
using Dolittle.Lifecycle;
using Dolittle.Logging;
using Dolittle.Types;

namespace Dolittle.Grpc
{
    /// <summary>
    /// Represents an implementation of <see cref="IHosts"/>
    /// </summary>
    [Singleton]
    public class Hosts : IHosts
    {
        class HostInfo
        {
            public string Identifier;
            public HostConfiguration Configuration;
        }

        readonly IDictionary<Type, HostInfo> _bindersWithConfigAccessor = new Dictionary<Type, HostInfo>();

        readonly IList<IHost> _hosts = new List<IHost>();
        readonly ITypeFinder _typeFinder;
        readonly IContainer _container;
        readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of <see cref="Hosts"/>
        /// </summary>
        /// <param name="hostTypes">Instances of <see cref="IRepresentHostType"/></param>
        /// <param name="typeFinder"><see cref="ITypeFinder"/> for finding services to host</param>
        /// <param name="container"><see cref="IContainer"/> for working with instances of host binders</param>
        /// <param name="logger"><see cref="ILogger"/> for logging</param>
        public Hosts(
            IInstancesOf<IRepresentHostType> hostTypes,
            ITypeFinder typeFinder,
            IContainer container,
            ILogger logger)
        {
            hostTypes.ForEach(_ => 
                _bindersWithConfigAccessor[_.BindingInterface] = 
                    new HostInfo { Identifier = _.Identifier, Configuration = _.Configuration }
                );

            _typeFinder = typeFinder;
            _container = container;
            _logger = logger;
        }

        /// <summary>
        /// Destructs the <see cref="Hosts"/> instance
        /// </summary>
        ~Hosts()
        {
            Dispose();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _hosts.ForEach(_ => _.Dispose());
        }

        /// <inheritdoc/>
        public void Start()
        {
            _logger.Information("Starting all hosts");

            foreach ((var type, var hostInfo) in _bindersWithConfigAccessor)
            {
                if (hostInfo.Configuration.Enabled)
                {
                    _logger.Information($"Preparing host for {hostInfo.Identifier}");

                    var binders = _typeFinder.FindMultiple(type);
                    binders.ForEach(_ =>
                    {
                        var binder = _container.Get(_) as ICanBindServices;
                        _logger.Information($"Bind services from {_.AssemblyQualifiedName}");
                        var services = binder.BindServices();
                        _logger.Information($"  {services.Count()} will be added");
                        var host = _container.Get<IHost>();
                        host.Start(hostInfo.Identifier, hostInfo.Configuration, services);
                        _hosts.Add(host);
                    });
                }
                else
                {
                    _logger.Information($"{hostInfo.Identifier} host is disabled");
                }
            }
        }
    }
}