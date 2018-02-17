using Machine.Specifications;

namespace doLittle.DependencyInversion.for_BindingCollection
{
    public class when_checking_if_has_binding_and_it_has_not
    {
        static BindingCollection collection;
        static bool result;

        Establish context = ()=> collection = new BindingCollection();

        Because of = () => result = collection.HasBindingFor<string>();

        It should_not_have_it = () => result.ShouldBeFalse();
    }
}