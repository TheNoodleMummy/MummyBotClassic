using System;
using System.Collections.Generic;
using System.Text;

namespace Mummybot.Exceptions
{
    public class QuahuRenamedException : Exception
    {
        public QuahuRenamedException(string message) : base(message) { }
    }
}
