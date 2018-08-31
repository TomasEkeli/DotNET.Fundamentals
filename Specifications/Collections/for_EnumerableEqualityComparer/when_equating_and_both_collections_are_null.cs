namespace Dolittle.Collections.for_EnumerableEqualityComparer
{
    using Machine.Specifications;
    using Dolittle.Collections;
    using System.Collections.Generic;

    [Subject(typeof(EnumerableEqualityComparer<>))]
    public class when_equating_and_both_collections_are_null
    {
        static IEqualityComparer<IEnumerable<int>> comparer;
        static bool is_equal;

        Establish context = () => 
        {
            comparer = new EnumerableEqualityComparer<int>();
        };

        Because of = () => is_equal = comparer.Equals(null, null);

        It should_be_equal = () => is_equal.ShouldBeTrue();
    }  
}