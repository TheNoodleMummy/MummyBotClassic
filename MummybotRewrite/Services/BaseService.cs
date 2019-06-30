using Mummybot.Extentions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Services
{
    public class BaseService
    {
        /// <summary>
        /// A Log service for logging, idk /shrug
        /// </summary>
        public LogService LogService { get; set; }
        
        
        /// <summary>
        /// gets run when RunInitializers() is called on IServiceProvider
        /// </summary>
        /// <param name="services">the current IServiceProvider</param>
        public virtual Task InitialiseAsync(IServiceProvider services)
            => Task.CompletedTask;
    }
}
