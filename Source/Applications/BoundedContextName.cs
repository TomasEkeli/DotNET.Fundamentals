﻿/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Dolittle.Concepts;

namespace Dolittle.Applications
{
    /// <summary>
    /// Represents the name of a <see cref="BoundedContext"/>
    /// </summary>
    public class BoundedContextName : ConceptAs<string>, IApplicationLocationSegmentName
    {
        /// <summary>
        /// Gets the <see cref="BoundedContextName"/> representing an undefined name
        /// </summary>
        public static readonly BoundedContextName NotSet = "[Not Set]";


        /// <inheritdoc/>
        public string AsString()
        {
            return this;
        }

        /// <summary>
        /// Implicitly converts from a <see cref="string"/> to a <see cref="BoundedContextName"/>
        /// </summary>
        /// <param name="boundedContextName">Name of the <see cref="BoundedContext"/></param>
        public static implicit operator BoundedContextName(string boundedContextName)
        {
            return new BoundedContextName { Value = boundedContextName };
        }
    }
}
