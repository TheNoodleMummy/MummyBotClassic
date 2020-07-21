using Discord;
using Qmmands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mummybot.Attributes;
using Mummybot.Enums;
using System.Threading;
using System.IO;
using Microsoft.Extensions.DependencyInjection;

namespace Mummybot.Services
{
    [InitilizerPriority(1)]
    public class LogService : BaseService
    {
        public string LogDirectory => Path.Combine(DateTime.Now.Year.ToString(), DateTime.Now.ToString("MMM"));
        public string Logfile => Path.Combine(LogDirectory, $"{DateTime.UtcNow.ToString("yyyy-MM-dd")}.txt");

        public override Task InitialiseAsync(IServiceProvider services)
        {
            DiscordSocketClient = services.GetRequiredService<DiscordSocketClient>();
            return Task.CompletedTask;
        }
        public static SemaphoreSlim ss = new SemaphoreSlim(1, 1);

        public DiscordSocketClient DiscordSocketClient { get; set; }

        public Task LogEventAsync(LogMessage log) => LogEventCustomAsync(new Structs.LogMessage(log.Severity, log.Source, log.Message, log.Exception));
        public Task LogEventCustomAsync(Structs.LogMessage log)
        {
            ss.Wait();
            var source = log.Source;
            var message = log.Message;
            var exception = log.Exception?.InnerException ?? log.Exception;
            var severity = log.Severity;

            var time = DateTime.Now;

            Console.Write($"{(time.Day < 10 ? "0" : "")}{time.Day}-{(time.Month < 10 ? "0" : "")}{time.Month}-{time.Year} {(time.Hour < 10 ? "0" : "")}{time.Hour}:{(time.Minute < 10 ? "0" : "")}{time.Minute}:{(time.Second < 10 ? "0" : "")}{time.Second}");

            Console.Write("[");
            Console.ForegroundColor = severity switch
            {
                LogSeverity.Critical => ConsoleColor.DarkRed,
                LogSeverity.Error => ConsoleColor.Red,
                LogSeverity.Warning => ConsoleColor.DarkYellow,
                LogSeverity.Info => ConsoleColor.DarkGreen,
                LogSeverity.Verbose => ConsoleColor.Green,
                LogSeverity.Debug => ConsoleColor.Magenta,
                _ => throw new ArgumentOutOfRangeException(),
            };
            const int sevLength = 8;
            if (severity.ToString().Length < sevLength)
            {
                var builder = new StringBuilder(sevLength);
                builder.Append(severity.ToString());
                builder.Append(' ', sevLength - severity.ToString().Length);
                Console.Write($"{builder}");
            }
            else if (severity.ToString().Length > sevLength)
            {
                Console.Write($"{severity.ToString().Substring(0, sevLength)}");
            }
            else
            {
                Console.Write(severity.ToString());
            }
            Console.ResetColor();
            Console.Write("]");

            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Cyan;
            if (source.Length < 11)
            {
                var builder = new StringBuilder(11);
                builder.Append(source);
                builder.Append(' ', 11 - source.Length);
                Console.Write($"{builder}");
            }
            else if (source.Length > 11)
            {
                Console.Write($"{source.Substring(0, 11)}");
            }
            else
            {
                Console.Write(source);
            }
            Console.ResetColor();
            Console.Write("] ");

            if (log.Guild != null)
            {
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.White;

                if (log.Guild.Id.ToString().Length > 20)
                {
                    Console.Write($"Id to long");
                }
                else
                {
                    Console.Write(log.Guild.Id.ToString());
                }
                Console.Write("/");
                if (log.Guild.Name.Length < 15)
                {
                    var builder = new StringBuilder(15);
                    builder.Append(log.Guild.Name);
                    builder.Append(' ', 15 - log.Guild.Name.Length);
                    Console.Write($"{builder}");
                }
                else if (log.Guild.Name.Length > 15)
                {
                    Console.Write($"{log.Guild.Name.Substring(0, 15)}");
                }
                else
                {
                    Console.Write(log.Guild.Name);
                }
                Console.ResetColor();
                Console.Write("] ");
            }
            else
            {
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.White;

                var builder = new StringBuilder(40);
                builder.Append("No Guild Specified");
                builder.Append(' ', 34 - "No Guild Specified".Length);
                Console.Write($"{builder}");
            
                Console.ResetColor();
                Console.Write("] ");
            }
            

            if (!string.IsNullOrEmpty(message))
                Console.Write(string.Join("", message.Where(x => !char.IsControl(x))));

            if (!string.IsNullOrEmpty(exception?.ToString()))
            {
                if (!string.IsNullOrEmpty(exception?.ToString()))
                {
                    Console.WriteLine(" " + exception.Message);
                    Console.WriteLine(exception.StackTrace);
                }
            }

            Console.WriteLine();

#if !DEBUG
            if (!Directory.Exists(LogDirectory))
            {
                Directory.CreateDirectory(LogDirectory);
            }
            if (!File.Exists(Logfile))
            {
                File.Create(Logfile).Dispose();

            }

            string logText = $"{DateTime.UtcNow.ToString("hh:mm:ss")} [{severity}] {source}: {message} => {exception}";
            File.AppendAllText(Logfile, logText + Environment.NewLine);  
#endif
            ss.Release(1);
            return Task.CompletedTask;
        }

        internal Task LogLavalink(LogMessage arg1)
        => LogEventAsync(arg1);      

        internal void LogDebug(string Message, LogSource source = LogSource.Unkown, ulong Guildid = 0, Exception exception = null)
        {
            var Guild = DiscordSocketClient?.GetGuild(Guildid);
            LogEventCustomAsync(new Structs.LogMessage(LogSeverity.Debug, source.ToString(), Message, exception, Guild));
        }

        internal void LogWarning(string Message, LogSource source = LogSource.Unkown, ulong Guildid = 0, Exception exception = null)
        {
            var Guild = DiscordSocketClient?.GetGuild(Guildid);
            LogEventCustomAsync(new Structs.LogMessage(LogSeverity.Warning, source.ToString(), Message, exception, Guild));
        }

        internal void LogVerbose(string Message, LogSource source = LogSource.Unkown, ulong Guildid = 0, Exception exception = null)
        {
            var Guild = DiscordSocketClient?.GetGuild(Guildid);
            LogEventCustomAsync(new Structs.LogMessage(LogSeverity.Verbose, source.ToString(), Message, exception, Guild));
        }

        internal void LogCritical(string Message, LogSource source = LogSource.Unkown, ulong Guildid = 0, Exception exception = null)
        {
            var Guild = DiscordSocketClient?.GetGuild(Guildid);
            LogEventCustomAsync(new Structs.LogMessage(LogSeverity.Critical, source.ToString(), Message, exception, Guild));
        }

        internal void LogError(string Message, LogSource source = LogSource.Unkown, ulong Guildid = 0, Exception exception = null)
        {
            var Guild = DiscordSocketClient?.GetGuild(Guildid);
            LogEventCustomAsync(new Structs.LogMessage(LogSeverity.Error, source.ToString(), Message, exception, Guild));
        }

        internal void LogInformation(string Message, LogSource source = LogSource.Unkown, ulong Guildid = 0, Exception exception = null)
        {
            var Guild = DiscordSocketClient?.GetGuild(Guildid);
            LogEventCustomAsync(new Structs.LogMessage(LogSeverity.Info, source.ToString(), Message, exception, Guild));
        }
    }
}