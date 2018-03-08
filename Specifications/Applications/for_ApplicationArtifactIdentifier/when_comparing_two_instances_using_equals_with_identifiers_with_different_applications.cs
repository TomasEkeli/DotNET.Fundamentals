﻿/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Machine.Specifications;
using It = Machine.Specifications.It;

namespace Dolittle.Applications.Specs.for_ApplicationArtifactIdentifier
{
    public class when_comparing_two_instances_using_equals_with_identifiers_with_different_applications : given.identifiers_with_different_applications
    {
        static bool result;

        Because of = () => result = identifier_a.Equals(identifier_b);

        It should_not_be_considered_the_same = () => result.ShouldBeFalse();
    }
}
