/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;

namespace Dolittle.Configuration
{
    /// <summary>
    /// Defines a system for parsing configuration files into <see cref="IConfigurationObject"/> instances
    /// </summary>
    public interface ICanParseConfigurationFile
    {
        /// <summary>
        /// Method that gets called to check if it can be parsed
        /// </summary>
        /// <param name="type"><see cref="Type"/> of <see cref="IConfigurationObject"/></param>
        /// <param name="filename">The filename of the configuration</param>
        /// <param name="content">The actual content of the file</param>
        /// <returns>True if it can parse, false if not</returns>
        bool CanParse(Type type, string filename, string content);

        /// <summary>
        /// Parse specific content coming from a file into a specified <see cref="IConfigurationObject"/> type
        /// </summary>
        /// <param name="type"><see cref="Type"/> of <see cref="IConfigurationObject"/></param>
        /// <param name="filename">The filename of the configuration</param>
        /// <param name="content">The actual content of the file</param>
        /// <returns>An instance of the <see cref="IConfigurationObject"/></returns>
        object Parse(Type type, string filename, string content);
    }
}