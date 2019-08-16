/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using Dolittle.Concepts;
using Polly;

namespace Dolittle.Resilience
{
    /// <summary>
    /// Represents a system that is capable of defining resilience 
    /// </summary>
    public interface ICanDefinePoliciesFor<T>
    {
        /// <summary>
        /// Define the policy for the given type
        /// </summary>
        Policy Define();
    }
}