using System;
using System.Collections.Generic;
using System.Text;

namespace Mummybot.Exceptions
{
    public class InvalidPriorityException : Exception
    {
        public InvalidPriorityException(int index) : base($"the priorirty of {index} is already taken")
        {

        }
        
    }
}
