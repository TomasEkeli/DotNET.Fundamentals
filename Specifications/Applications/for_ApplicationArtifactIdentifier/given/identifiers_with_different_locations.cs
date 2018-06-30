﻿/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Dolittle.Artifacts;
using Machine.Specifications;
using Moq;

namespace Dolittle.Applications.Specs.for_ApplicationArtifactIdentifier.given
{
    public class identifiers_with_different_locations
    {
        protected static ApplicationArtifactIdentifier identifier_a;
        protected static ApplicationArtifactIdentifier identifier_b;

        Establish context = () =>
        {
            var application = new Mock<IApplication>();
            application.SetupGet(a => a.Name).Returns("SomeApplication");
            var area = (ApplicationArea)"Some Area";
            var location_a = Mock.Of<IApplicationLocation>();
            var location_b = Mock.Of<IApplicationLocation>();

            var artifact = Mock.Of<IArtifact>();

            identifier_a = new ApplicationArtifactIdentifier(application.Object, area, location_a, artifact);
            identifier_b = new ApplicationArtifactIdentifier(application.Object, area, location_b, artifact);
        };
    }
}
