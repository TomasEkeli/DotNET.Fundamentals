﻿/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
namespace Dolittle.Rules
{
    /// <summary>
    /// Represents an implementation of <see cref="IRuleContexts"/>
    /// </summary>
    public class RuleContexts : IRuleContexts
    {
#pragma warning disable 1591 // Xml Comments
        public IRuleContext GetFor(object instance)
        {
            var ruleContext = new RuleContext();
            return ruleContext;
        }
#pragma warning restore 1591 // Xml Comments
    }
}
