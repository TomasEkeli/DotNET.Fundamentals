﻿using System;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace Dolittle.Applications.Specs.for_BoundedContext
{
    public class when_adding_existing_module
    {
        static BoundedContext bounded_context;
        static Mock<IModule> module;
        static Exception exception;

        Establish context = () =>
        {
            bounded_context = new BoundedContext("Some bounded context");
            module = new Mock<IModule>();
            bounded_context.AddModule(module.Object);
        };

        Because of = () => exception = Catch.Exception(() => bounded_context.AddModule(module.Object));

        It should_throw_module_already_added_to_bounded_context = () => exception.ShouldBeOfExactType<ModuleAlreadyAddedToBoundedContext>();
    }
}
