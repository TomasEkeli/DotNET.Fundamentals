/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using Machine.Specifications;
using Moq;
using It = Machine.Specifications.It;
using Dolittle.Types;

namespace Dolittle.Build.for_PostBuildTaskPerformers
{
    public class when_performing_with_two_performers_in_the_system
    {
        static PostBuildTaskPerformers  performers;

        static Mock<ICanPerformPostBuildTasks>  first_performer;
        static Mock<ICanPerformPostBuildTasks>  second_performer;

        Establish context = () => 
        {

            first_performer = new Mock<ICanPerformPostBuildTasks>();
            second_performer = new Mock<ICanPerformPostBuildTasks>();
            var actualInstances = new List<ICanPerformPostBuildTasks> {
                first_performer.Object,
                second_performer.Object
            };
            var instances = new Mock<IInstancesOf<ICanPerformPostBuildTasks>>();
            instances.Setup(_ => _.GetEnumerator()).Returns(actualInstances.GetEnumerator());
            performers = new PostBuildTaskPerformers(instances.Object, Mock.Of<IBuildMessages>());
        };

        Because of = () => performers.Perform();

        It should_call_perform_on_first_performer = () => first_performer.Verify(_ => _.Perform(), Moq.Times.Once());
        It should_call_perform_on_second_performer = () => second_performer.Verify(_ => _.Perform(), Moq.Times.Once());
    }
}