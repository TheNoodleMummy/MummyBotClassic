using Mummybot.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mummybot.Exceptions
{
    public class InvalidServiceException : Exception
    {
            public InvalidServiceException(string type) : base($"{type} does not inherit {nameof(BaseService)}")
            {
            }
        
    }
}
