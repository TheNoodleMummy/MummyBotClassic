using System.Threading.Tasks;

namespace Mummybot.interfaces
{
    public interface IRemoveableService
    {
        Task RemoveAsync(IRemoveable obj);
    }
}
