/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;

namespace Dolittle.Resources
{
    /// <summary>
    /// Defines a resource type and its services
    /// </summary>
    public interface IAmAResourceType
    {
        /// <summary>
        /// Gets the name of the resource type. for example "readModels" or "eventStore"
        /// </summary>
        ResourceType Name { get; }
        /// <summary>
        /// Gets the services 
        /// </summary>
        IEnumerable<Type> Services { get; }
    }
}