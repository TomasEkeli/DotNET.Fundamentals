﻿/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Dolittle.Specifications;
using Microsoft.Extensions.DependencyModel;

namespace Dolittle.Assemblies.Rules
{
    /// <summary>
    /// Represents the <see cref="IAssemblyRuleBuilder">builder</see> for building the <see cref="IncludeAllRule"/> and
    /// possible exceptions
    /// </summary>
    public class ExcludeAll : IAssemblyRuleBuilder
    {
        /// <summary>
        /// Initializes an instance of <see cref="IncludeAll"/>
        /// </summary>
        public ExcludeAll()
        {
            Specification = new ExcludeAllRule();
        }

        /// <summary>
        /// Gets the <see cref="IncludeAllRule"/>
        /// </summary>
        public Specification<Library> Specification { get; set; }
    }
}
