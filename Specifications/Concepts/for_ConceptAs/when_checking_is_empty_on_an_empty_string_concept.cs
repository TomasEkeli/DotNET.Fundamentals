﻿using Dolittle.Concepts;
using Machine.Specifications;

namespace Dolittle.Specs.Concepts.for_ConceptAs
{
    [Subject(typeof(ConceptAs<>))]
    public class when_checking_is_empty_on_an_empty_string_concept : Dolittle.Specs.Concepts.given.concepts
    {
        static bool is_empty;

        Establish context = () => is_empty = string_is_empty.IsEmpty();

        It should_be_empty = () => is_empty.ShouldBeTrue();
    }
}