namespace Execution.for_Ensure.when_ensuring_nullable_property_is_not_null
{
    using System;
    using Dolittle.Execution;
    using Machine.Specifications;

    [Subject(typeof(Ensure))]
    public class and_it_is_null
    {
        const string property_name = "test";
        static Exception exception;
        Because of = () => exception = Catch.Exception(() => Ensure.IsNotNull<string>(property_name,null));

        It should_throw_an_exception = () => exception.ShouldNotBeNull();
        It should_be_an_argument_null_exception = () => exception.ShouldBeOfExactType<ArgumentNullException>();
        It should_include_the_property_name_in_the_exception = () => ((ArgumentNullException)exception)?.ParamName.ShouldEqual(property_name);
    }
}