using System;
using System.Collections.Generic;
using System.Text;

namespace Mummybot.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class DontAutoAddAttribute : Attribute
    {

    }
}
