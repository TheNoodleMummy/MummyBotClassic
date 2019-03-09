using System;

namespace Mummybot.Attributes
{
    public class PropNameAttribute : Attribute
    {
        public string name;

        public PropNameAttribute(string Name)
        {
            name = Name;
        }
    }
}
