using System;
using System.Collections.Generic;
using System.Text;

namespace Mummybot.Attributes
{
    public class PropDescriptionAttribute : Attribute
    {
        public string Description;

        public PropDescriptionAttribute(string Name)
        {
            Description = Name;
        }
    }
}
