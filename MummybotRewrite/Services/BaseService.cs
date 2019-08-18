﻿using Mummybot.Extentions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Services
{
    public class BaseService
    {
        /// <summary>
        /// gets run when RunInitializers() is called on IServiceProvider
        /// </summary>
        /// <param name="services">the current IServiceProvider</param>
        public virtual Task InitialiseAsync(IServiceProvider services)
            => Task.CompletedTask;
    }
}
