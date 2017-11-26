using System;
using doLittle.Concepts;

namespace doLittle.Applications.for_ApplicationStructureFragment
{
    public class SomeName : ConceptAs<string>, IApplicationLocationSegmentName
    {
        public string AsString()
        {
            return Value;
        }
    }

    public class SegmentWithWrongTypeForNameInConstructor : IApplicationLocationSegment<SomeName>
    {
        public SegmentWithWrongTypeForNameInConstructor(string name)
        {

        }
        
        IApplicationLocationSegmentName IApplicationLocationSegment.Name => Name;

        public SomeName Name => throw new NotImplementedException();
    }
}