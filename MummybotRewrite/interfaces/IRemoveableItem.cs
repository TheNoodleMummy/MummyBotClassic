using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.interfaces
{
    public interface IRemoveable
    {
         int Identifier { get; }

        DateTime When { get; }

        Task RemoveAsync();

    }
}
