using System;
using System.Collections.Generic;
using System.Text;

namespace Mummybot.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ServiceAttribute : Attribute
    {
        public Type Target { get; }
        public string Name { get; }
        public bool AutoAdd { get; }

        public ServiceAttribute(string name, Type target, bool autoadd = true)
        {
            Name = name;
            Target = target;
            AutoAdd = autoadd;
        }
    }
}
