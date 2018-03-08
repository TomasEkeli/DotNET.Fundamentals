﻿/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;

namespace Dolittle.Types
{
    /// <summary>
    /// Exception that is thrown when a type is not possible to be resolved by its name
    /// </summary>
    public class UnableToResolveTypeByName : ArgumentException
    {
        /// <summary>
        /// Initializes an instance of <see cref="UnableToResolveTypeByName"/>
        /// </summary>
        /// <param name="typeName"></param>
        public UnableToResolveTypeByName(string typeName) : base($"Unable to resolve '{typeName}'.")
        {

        }
    }
}
