/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Reflection;
using Dolittle.Assemblies.Configuration;
using Dolittle.Assemblies.Rules;
using Dolittle.Logging;

namespace Dolittle.Assemblies.Bootstrap
{
    /// <summary>
    /// Represents the entrypoint for initializing assemblies
    /// </summary>
    public class Boot
    {
        /// <summary>
        /// Initialize assemblies setup
        /// </summary>
        /// <param name="logger"><see cref="ILogger"/> to use for logging</param>
        /// <param name="entryAssembly"><see cref="Assembly"/> to use as entry assembly - null indicates it will get it from the <see cref="Assembly.GetEntryAssembly()"/> method</param>
        /// <param name="defaultAssemblyProvider">The default <see cref="ICanProvideAssemblies"/> - null inidicates it will use the default implementation</param>
        /// <returns><see cref="IAssemblies"/></returns>
        public static IAssemblies Start(ILogger logger, Assembly entryAssembly = null, ICanProvideAssemblies defaultAssemblyProvider = null)
        {
            var assembliesConfigurationBuilder = new AssembliesConfigurationBuilder();
            assembliesConfigurationBuilder
                .ExcludeAll()
                .ExceptProjectLibraries()
                .ExceptDolittleLibraries();

            if( entryAssembly == null ) entryAssembly = Assembly.GetEntryAssembly();

            var assembliesConfiguration = new AssembliesConfiguration(assembliesConfigurationBuilder.RuleBuilder);
            var assemblyFilters = new AssemblyFilters(assembliesConfiguration);

            var assemblyProvider = new AssemblyProvider(
                new ICanProvideAssemblies[] { defaultAssemblyProvider ?? new DefaultAssemblyProvider(logger, entryAssembly) },
                assemblyFilters,
                new AssemblyUtility());

            var assemblies = new Assemblies(entryAssembly, assemblyProvider);
            return assemblies;
        }
    }

}