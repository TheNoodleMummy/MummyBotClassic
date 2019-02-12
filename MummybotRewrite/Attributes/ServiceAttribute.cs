using Mummybot.Exceptions;
using System;

namespace Mummybot.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceAttribute : Attribute
    {
        public Type Target { get; }

        public ServiceAttribute(Type target)
        {
            if(!target.IsInterface)
                //throw new NotInterfaceException($"{target} must be an interface");

            Target = target;
        }
    }
}
