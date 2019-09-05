/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using Dolittle.Grpc;
using Grpc.Core;

namespace Dolittle.DependencyInversion.Grpc
{
    /// <summary>
    /// Represents an implementation of <see cref="ICanBindManagementServices"/> for exposing
    /// management service implementations for DependencyInversion
    /// </summary>
    public class ManagementServices : ICanBindManagementServices
    {
        readonly ContainerService _containerService;

        /// <summary>
        /// Initializes a new instance of <see cref="ManagementServices"/>
        /// </summary>
        /// <param name="containerService">The <see cref="ContainerService"/></param>
        public ManagementServices(ContainerService containerService)
        {
            _containerService = containerService;
        }

        /// <inheritdoc/>
        public IEnumerable<ServerServiceDefinition> BindServices()
        {
            return new ServerServiceDefinition[] {
                Container.BindService(_containerService)
            };
        }       
    }
}