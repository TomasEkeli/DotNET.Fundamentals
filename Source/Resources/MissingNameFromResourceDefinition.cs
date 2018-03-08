/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;

namespace Dolittle.Resources
{
    /// <summary>
    /// Exception that gets thrown when a <see cref="IResourceDefinition"/> is missing its name
    /// </summary>
    public class MissingNameFromResourceDefinition : ArgumentException
    {
    }
}