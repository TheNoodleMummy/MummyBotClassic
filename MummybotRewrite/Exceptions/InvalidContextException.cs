using System;
using System.Collections.Generic;
using System.Text;

namespace Mummybot.Exceptions
{
    class InvalidContextException : Exception
    {
        public InvalidContextException(Type type) : base($"Expected MummyContext, got: {type}")
        {
            
        }
    }
}
