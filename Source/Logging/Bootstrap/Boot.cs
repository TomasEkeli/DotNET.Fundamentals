/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dolittle.Collections;
using Dolittle.Reflection;
using Microsoft.Extensions.Logging;

namespace Dolittle.Logging.Bootstrap
{
    /// <summary>
    /// Represents the entrypoint for initializating Logging
    /// </summary>
    public class Boot
    {
        /// <summary>
        /// Discover any <see cref="ICanConfigureLogAppenders"/> from the entry assembly and setup 
        /// <see cref="ILogAppenders"/>
        /// </summary>
        /// <returns>An instance of <see cref="ILogAppenders"/> that can be used</returns>
        public static ILogAppenders Start(ILoggerFactory loggerFactory, ILogAppender defaultLogAppender = null, Assembly assembly = null)
        {
            if( assembly == null ) assembly = Assembly.GetEntryAssembly();
            var types = assembly.GetTypes();

            var configuratorTypes = types.Where(t => t.HasInterface<ICanConfigureLogAppenders>());

            var configurators = new List<ICanConfigureLogAppenders>();
            configuratorTypes.ForEach(c =>
            {
                ThrowIfLogAppenderConfiguratorIsMissingDefaultConstructor(c);
                var configurator = Activator.CreateInstance(c) as ICanConfigureLogAppenders;
                configurators.Add(configurator);
            });

            var logAppenders = new LogAppenders(configurators, defaultLogAppender);
            return logAppenders;
        }

        static void ThrowIfLogAppenderConfiguratorIsMissingDefaultConstructor(Type c)
        {
            if (!c.HasDefaultConstructor()) throw new LogAppenderConfiguratorMissingDefaultConstructor(c);
        }
    }
}