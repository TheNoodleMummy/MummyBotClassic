using System;

namespace Mummybot.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceAttribute : Attribute
    {
        public string ServiceName { get; }
        public Type Target { get; internal set; }

        public ServiceAttribute(string serviceName,Type target)
        {
            Target = target;
            ServiceName = serviceName;
        }
    }
}
