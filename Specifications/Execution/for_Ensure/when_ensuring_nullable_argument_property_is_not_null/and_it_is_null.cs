namespace Dolittle.Execution.for_Ensure.when_ensuring_nullable_argument_property_is_not_null
{
    using System;
    using Machine.Specifications;

    [Subject(typeof(Ensure))]
    public class and_it_is_null
    {
        const string path = "my_object";
        const string property_name = "test";
        static Nullable<int> property;
        static Exception exception;

        Establish context = () => 
        {
            property = null;
        };
        Because of = () => exception = Catch.Exception(() => Ensure.NullableArgumentPropertyIsNotNull(property_name,path,property));

        It should_throw_an_exception = () => exception.ShouldNotBeNull();
        It should_be_an_argument_null_exception = () => exception.ShouldBeOfExactType<ArgumentNullException>();
        It should_include_the_property_name_in_the_exception = () => ((ArgumentNullException)exception)?.ParamName.ShouldEqual($"{path}.{property_name}");
    }
}