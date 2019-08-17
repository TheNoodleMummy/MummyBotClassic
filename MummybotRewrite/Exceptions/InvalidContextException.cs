using Mummybot.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mummybot.Exceptions
{
    class InvalidContextException : Exception
    {
        public InvalidContextException(Type type) : base($"Expected {typeof(MummyContext)}, got: {type}")
        {
            
        }
    }
}
