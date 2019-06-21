using System;

namespace Mummybot.Attributes
{

    public class UsageAttribute : Attribute
    {
        public string Example { get; }

        public UsageAttribute(string example)
        {
            Example = example;
        }

    }
}
