using System;
using System.Collections.Generic;
using System.Text;

namespace Mummybot.Interfaces
{
    public interface IMummyResult
    {
        bool IsSuccess { get; set; }
        string ErrorReason { get; set; }
        Exception Exception { get; set; } 
    }
}
