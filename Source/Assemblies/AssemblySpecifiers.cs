﻿/*---------------------------------------------------------------------------------------------
 *  Copyright (c) 2008-2017 doLittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Linq;
using System.Reflection;
using doLittle.Assemblies.Configuration;
using doLittle.Collections;
using doLittle.Types.Utils;

namespace doLittle.Assemblies
{
    /// <summary>
    /// Represents an implementation of <see cref="IAssemblySpecifiers"/>
    /// </summary>
    public class AssemblySpecifiers : IAssemblySpecifiers
    {
        readonly IAssemblyRuleBuilder _assemblyRuleBuilder;

        /// <summary>
        /// Initializes a new instance of <see cref="AssemblySpecifiers"/>
        /// </summary>
        /// <param name="assemblyRuleBuilder"><see cref="IAssemblyRuleBuilder"/> used for building the rules for assemblies</param>
        public AssemblySpecifiers(IAssemblyRuleBuilder assemblyRuleBuilder)
        {
            _assemblyRuleBuilder = assemblyRuleBuilder;
        }

        /// <inheritdoc/>
        public void SpecifyUsingSpecifiersFrom(Assembly assembly)
        {
            assembly
                .GetTypes()
                .Where(t => t.Implements(typeof(ICanSpecifyAssemblies)))
                .Where(t => t.GetTypeInfo().Assembly.FullName == assembly.FullName)
                .Where(type => type.HasDefaultConstructor())
                .ForEach(type =>
                {
                    var specifier = Activator.CreateInstance(type) as ICanSpecifyAssemblies;
                    specifier.Specify(_assemblyRuleBuilder);
                });
        }
    }
}
