using Mummybot.Exceptions;
using Mummybot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Extentions
{
    public partial class Extentions
    {
        public static async Task RunInitialisersAsync(this IServiceProvider services, IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                var service = services.GetService(type);

                if (!(service is BaseService validService))
                    throw new InvalidServiceException(nameof(type));

                await validService.InitialiseAsync(services);
            }
        }
    }
}
