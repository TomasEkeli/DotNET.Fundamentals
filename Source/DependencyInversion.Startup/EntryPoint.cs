/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using doLittle.DependencyInversion.Conventions;
using doLittle.Reflection;
using doLittle.Types;

namespace doLittle.DependencyInversion.Startup
{
    /// <summary>
    /// The entrypoint for DependencyInversion
    /// </summary>
    public class EntryPoint
    {
        /// <summary>
        /// Initialize the entire DependencyInversion pipeline
        /// </summary>
        /// <param name="typeFinder"><see cref="ITypeFinder"/> for doing discovery</param>
        /// <returns>Configured <see cref="IContainer"/></returns>
        public static IContainer Initialize(ITypeFinder typeFinder)
        {
            var bindingConventionManager = new BindingConventionManager(typeFinder);
            var bindingsFromConventions = bindingConventionManager.DiscoverAndSetupBindings();
            var bindingsFromProviders = DiscoverBindingProvidersAndGetBindings(typeFinder);
            var bindingCollection = new BindingCollection(bindingsFromConventions, bindingsFromProviders);
            var container = DiscoverAndConfigureContainer(typeFinder, bindingCollection);
            return container;
        }

        static IBindingCollection DiscoverBindingProvidersAndGetBindings(ITypeFinder typeFinder)
        {
            var bindingProviders = typeFinder.FindMultiple<ICanProvideBindings>();
            var bindingCollections = new ConcurrentBag<IBindingCollection>();

            Parallel.ForEach(bindingProviders, bindingProviderType =>
            {
                ThrowIfBindingProviderIsMissingDefaultConstructor(bindingProviderType);
                var bindingProvider = Activator.CreateInstance(bindingProviderType) as ICanProvideBindings;
                var bindingProviderBuilder = new BindingProviderBuilder();
                bindingProvider.Provide(bindingProviderBuilder);
                bindingCollections.Add(bindingProviderBuilder.Build());
            });

            var bindingCollection = new BindingCollection(bindingCollections.ToArray());
            return bindingCollection;
        }

        static IContainer DiscoverAndConfigureContainer(ITypeFinder typeFinder, IBindingCollection bindingCollection)
        {
            var containerProviderType = typeFinder.FindSingle<ICanProvideContainer>();
            ThrowIfMissingContainerProvider(containerProviderType);
            var containerProvider = Activator.CreateInstance(containerProviderType) as ICanProvideContainer;

            var container = containerProvider.Provide(bindingCollection);
            return container;
        }

        static void ThrowIfBindingProviderIsMissingDefaultConstructor(Type bindingProvider)
        {
            if (!bindingProvider.HasDefaultConstructor()) throw new BindingProviderMustHaveADefaultConstructor(bindingProvider);
        }

        static void ThrowIfMissingContainerProvider(Type containerProvider)
        {
            if( containerProvider == null ) throw new MissingContainerProvider();

        }
    }
}