using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.interfaces
{
    public interface IRemoveableService
    {
        Task RemoveAsync(IRemoveable obj);
    }
}
