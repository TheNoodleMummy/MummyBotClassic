using System;
using System.Collections.Generic;
using System.Text;

namespace Mummybot.Exceptions
{
    public class QuahuLiedException : Exception
    {
        public QuahuLiedException(string message) : base($"QUAHU CANNOT BE TRUSTED: {message}")
        {
        }
    }
}
