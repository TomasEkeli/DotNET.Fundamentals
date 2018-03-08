﻿using Dolittle.Mapping;
using Dolittle.Types;
using Machine.Specifications;
using Moq;

namespace Dolittle.Mapping.Specs.for_MappingTargets.given
{
    public class all_dependencies
    {
        protected static Mock<IInstancesOf<IMappingTarget>> mapping_targets_mock;

        Establish context = () => mapping_targets_mock = new Mock<IInstancesOf<IMappingTarget>>();
    }
}
