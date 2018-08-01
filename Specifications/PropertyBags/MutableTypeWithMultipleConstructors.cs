namespace Dolittle.PropertyBags.Specs
{
    using System;
    using Dolittle.Concepts;

    public class MutableTypeWithMultipleConstructors : Value<MutableTypeWithNoDefaultConstructor>
    {
        public MutableTypeWithMultipleConstructors(int intProperty)
        {
            IntProperty = intProperty;
        }

        public MutableTypeWithMultipleConstructors(string stringProperty)
        {
            StringProperty = stringProperty;
        }

        public int IntProperty { get; set; }
        public string StringProperty { get; set; }
        public DateTime DateTimeProperty { get; set; }
    }
}