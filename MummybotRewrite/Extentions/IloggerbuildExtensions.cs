using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Mummybot;
using Mummybot.Exceptions;
using Mummybot.Extentions;
using Mummybot.Services.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Extentions
{
    public static class IloggerbuildExtensions
    {
        public static ILoggingBuilder AddMyConsole(
        this ILoggingBuilder builder)
        {
            

            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Singleton<ILoggerProvider, ConsoleLoggerProvider>());

            

            return builder;
        }

        public static ILoggingBuilder AddMyConsole(
            this ILoggingBuilder builder,
            Action<LogService> configure)
        {
            builder.AddMyConsole();
            builder.Services.Configure(configure);

            return builder;
        }
    }
}
