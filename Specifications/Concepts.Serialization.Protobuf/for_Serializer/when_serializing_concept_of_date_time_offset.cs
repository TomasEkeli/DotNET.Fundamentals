using System;
using Dolittle.Concepts;
using Machine.Specifications;
using Dolittle.Serialization.Protobuf;

namespace Dolittle.Concepts.Serialization.Protobuf.for_Serializer
{
    public class when_serializing_concept_of_date_time_offset : given.a_serializer
    {
        
        class type_for_serialization
        {
            public ConceptAs<DateTimeOffset> concept {  get; set; }
        }

        static type_for_serialization original;
        static type_for_serialization deserialized;

        Establish context = () =>
        {
            original = new type_for_serialization {concept = new ConceptAs<DateTimeOffset> { Value = DateTimeOffset.UtcNow }};
            message_descriptions.Setup(_ => _.GetFor<type_for_serialization>()).Returns(MessageDescription.DefaultFor<type_for_serialization>());
        };

        Because of = () =>
        {
            var bytes = serializer.ToProtobuf(original);
            deserialized = serializer.FromProtobuf<type_for_serialization>(bytes);
        };

        It should_hold_the_correct_value = () => deserialized.concept.ShouldEqual(original.concept);
    }
}