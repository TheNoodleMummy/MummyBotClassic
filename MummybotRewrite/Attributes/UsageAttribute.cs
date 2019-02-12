using System;
using System.Collections.Generic;
using System.Text;

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
