﻿using Machine.Specifications;

namespace Dolittle.Reflection.Specs.for_TypeExtensions
{
    public class when_asking_if_non_nullable_type_is_nullable
    {
        static bool result;

        Because of = () => result = typeof(int).IsNullable();

        It should_return_false = () => result.ShouldBeFalse();
    }
}
