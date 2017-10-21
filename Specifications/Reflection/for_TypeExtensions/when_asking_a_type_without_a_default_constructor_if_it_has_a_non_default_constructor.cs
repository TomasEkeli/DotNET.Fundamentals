﻿using Machine.Specifications;

namespace doLittle.Reflection.Specs.for_TypeExtensions
{
    public class when_asking_a_type_without_a_default_constructor_if_it_has_a_non_default_constructor
    {
        static bool result;

        Because of = () => result = typeof(TypeWithoutDefaultConstructor).HasNonDefaultConstructor();

        It should_return_true = () => result.ShouldBeTrue();
    }
}
