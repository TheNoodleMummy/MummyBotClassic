﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Mummybot.Exceptions
{
    public class InvalidPriorityException : Exception
    {
        public InvalidPriorityException(int index,Type service) : base($"the priorirty of {index} on {service.FullName} is already taken")
        {

        }
        
    }
}
