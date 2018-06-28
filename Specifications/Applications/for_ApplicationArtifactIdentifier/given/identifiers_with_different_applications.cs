﻿/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Dolittle.Artifacts;
using Machine.Specifications;
using Moq;

namespace Dolittle.Applications.Specs.for_ApplicationArtifactIdentifier.given
{
    public class identifiers_with_different_applications
    {
        protected static ApplicationArtifactIdentifier identifier_a;
        protected static ApplicationArtifactIdentifier identifier_b;

        Establish context = () =>
        {
            var application_a = new Mock<IApplication>();
            application_a.SetupGet(a => a.Name).Returns("ApplicationA");

            var application_b = new Mock<IApplication>();
            application_b.SetupGet(a => a.Name).Returns("ApplicationB");

            var area = (ApplicationArea)"Some Area";
            var location = Mock.Of<IApplicationLocation>(_ => _.Equals(
                Moq.It.IsAny<IApplicationLocation>()) == true
                );
            
            var artifactType = new Mock<IArtifactType>();
            artifactType.SetupGet(_ => _.Identifier).Returns("Command");

            var artifact = new Artifact("Artifact", artifactType.Object, 1);

            identifier_a = new ApplicationArtifactIdentifier(application_a.Object, area, location, artifact);
            identifier_b = new ApplicationArtifactIdentifier(application_b.Object, area, location, artifact);
        };
       
    }
}
