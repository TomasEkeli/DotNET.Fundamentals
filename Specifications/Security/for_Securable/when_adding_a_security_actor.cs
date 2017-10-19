﻿using System.Collections.Generic;
using doLittle.Security;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;

namespace doLittle.Security.Specs.for_Securable
{
    [Subject(typeof(SecurityActor))]
    public class when_adding_a_security_actor
    {
        static Securable security_target;
        static Mock<ISecurityActor> security_actor_mock;

        Establish context = () => 
        {
            security_target = new NamespaceSecurable("doLittle.Security");
            security_actor_mock = new Mock<ISecurityActor>();
        };

        Because of = () => security_target.AddActor(security_actor_mock.Object);

        It should_have_it_available_in_the_collection = () => security_target.Actors.ShouldContain(security_actor_mock.Object);
    }
}
