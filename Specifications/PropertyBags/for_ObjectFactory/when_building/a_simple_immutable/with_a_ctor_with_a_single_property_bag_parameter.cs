﻿namespace Dolittle.PropertyBags.Specs.for_ObjectFactory.when_building.a_simple_immutable
{
    using Machine.Specifications;
    using Dolittle.PropertyBags;
    using Dolittle.PropertyBags.Specs;
    using System;

    [Subject(typeof(ObjectFactory), "Build")]
    public class with_a_ctor_with_a_single_property_bag_parameter : given.an_object_factory
    {
        static IObjectFactory factory;
        static ImmutableWithPropertyBagConstructor immutable_type;
        static PropertyBag source;
        static object result;
        Establish context = () => 
        {
            factory = instance;
            var propertyBag = new { IntProperty = 67, StringProperty = "Lisbon", DateTimeProperty = new DateTime(1967,5,25) }.ToPropertyBag();
            immutable_type = new ImmutableWithPropertyBagConstructor(propertyBag);
            source = immutable_type.ToPropertyBag();
        };

        Because of = () => result = factory.Build(typeof(ImmutableWithPropertyBagConstructor), source);

        It should_build_an_instance_of_the_type = () => result.ShouldBeOfExactType<ImmutableWithPropertyBagConstructor>();
        It should_have_the_same_properties_as_the_source = () => result.ShouldEqual(immutable_type);
    }
}