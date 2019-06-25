using System;
using System.Collections.Generic;
using System.Text;

namespace Mummybot.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class InjectAttribute : Attribute
    {
    }
}
