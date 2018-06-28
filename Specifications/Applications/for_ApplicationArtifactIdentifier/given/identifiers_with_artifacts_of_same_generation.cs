using Dolittle.Artifacts;
using Machine.Specifications;
using Moq;

namespace Dolittle.Applications.Specs.for_ApplicationArtifactIdentifier.given
{
    public class identifiers_with_artifacts_of_same_generation
    {
        protected static ApplicationArtifactIdentifier identifier_a;
        protected static ApplicationArtifactIdentifier identifier_b;

        Establish context = () =>
        {
            var application = new Mock<IApplication>();
            application.SetupGet(a => a.Name).Returns("SomeApplication");
            var area = (ApplicationArea)"Some Area";
            var location = Mock.Of<IApplicationLocation>(_ => _.Equals(
                Moq.It.IsAny<IApplicationLocation>()) == true
                );

            var artifactType = new Mock<IArtifactType>();
            artifactType.SetupGet(_ => _.Identifier).Returns("Command");
            
            var artifactA = new Artifact("Artifact", artifactType.Object, 1);
            var artifactB = new Artifact("Artifact", artifactType.Object, 1);

            identifier_a = new ApplicationArtifactIdentifier(application.Object, area, location, artifactA);
            identifier_b = new ApplicationArtifactIdentifier(application.Object, area, location, artifactB);
        };
    }
}