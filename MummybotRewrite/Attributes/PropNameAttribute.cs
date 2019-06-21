using System;

namespace Mummybot.Attributes
{
    public class PropNameAttribute : Attribute
    {
        public string Name;

        public PropNameAttribute(string name)
        {
            Name = name;
        }
    }
}
