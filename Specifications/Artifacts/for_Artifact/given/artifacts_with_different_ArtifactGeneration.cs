/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Machine.Specifications;
using Moq;

namespace Dolittle.Artifacts.Specs.for_Artifact.given
{
    public class artifacts_with_different_ArtifactGeneration : two_artifacts
    {
        Establish context = () =>
        {
            var artifactName = "Artifact";
            var type = new Mock<IArtifactType>();
            type.SetupGet(_ => _.Identifier).Returns("ArtifactType");

            var generationA = 1;
            var generationB = 2;

            artifactA = new Artifact(artifactName, type.Object, generationA);
            artifactB = new Artifact(artifactName, type.Object, generationB);

        };
    }
}