// Copyright (c) Dolittle. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Machine.Specifications;

namespace Dolittle.DependencyInversion.for_BindingBuilder
{
    public class when_binding_to_type : given.a_null_binding
    {
        static Binding result;

        Because of = () =>
        {
            builder.To(typeof(string));
            result = builder.Build();
        };

        It should_have_a_type_strategy = () => result.Strategy.ShouldBeOfExactType<Strategies.Type>();
        It should_hold_the_type_in_the_strategy = () => ((Strategies.Type)result.Strategy).Target.ShouldEqual(typeof(string));
        It should_have_transient_scope = () => result.Scope.ShouldBeAssignableTo<Scopes.Transient>();
    }
}