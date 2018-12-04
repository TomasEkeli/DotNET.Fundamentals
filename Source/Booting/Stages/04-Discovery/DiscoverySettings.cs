/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Reflection;
using Dolittle.Assemblies;

namespace Dolittle.Booting.Stages
{
    /// <summary>
    /// Represents the settings for <see cref="BootStage.InitialSystem"/> stage
    /// </summary>
    public class DiscoverySettings : IRepresentSettingsForBootStage
    {
        /// <summary>
        /// Gets the <see cref="AssemblyName">assemblies</see> to use
        /// </summary>
        public WellKnownAssembliesAssemblyProvider AssemblyProvider { get; internal set; }
    }
}