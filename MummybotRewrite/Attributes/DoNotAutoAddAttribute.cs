using System;
using System.Collections.Generic;
using System.Text;

namespace Mummybot.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class DoNotAutoAddAttribute : Attribute
    {
    }
}
