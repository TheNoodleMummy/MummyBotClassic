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

namespace Mummybot.Services
{
    [Service("Log Service", typeof(LogService))]
    public class LogService


    {

        public string _logDirectory => Path.Combine(DateTime.Now.ToString("MMM"));
        public string _logfile => Path.Combine(_logDirectory, $"{DateTime.UtcNow.ToString("yyyy-MM-dd")}.txt");


        SemaphoreSlim ss = new SemaphoreSlim(1, 1);

        public Task LogEventAsync(LogMessage log)
        {

            ss.Wait();
            var source = log.Source;
            var message = log.Message;
            var exception = log.Exception?.InnerException ?? log.Exception;
            var severity = log.Severity;

            var time = DateTime.Now;

            Console.Write($"{(time.Day < 10 ? "0" : "")}{time.Day}-{(time.Month < 10 ? "0" : "")}{time.Month}-{time.Year} {(time.Hour < 10 ? "0" : "")}{time.Hour}:{(time.Minute < 10 ? "0" : "")}{time.Minute}:{(time.Second < 10 ? "0" : "")}{time.Second}");

            Console.Write("[");
            switch (severity)
            {
                case LogSeverity.Critical:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                case LogSeverity.Verbose:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

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

#if DEBUG
#else

            if (!Directory.Exists(_logDirectory))
            {
                Directory.CreateDirectory(_logDirectory);
            }
            if (!File.Exists(_logfile))
            {
                File.Create(_logfile).Dispose();

            }

            string logText = $"{DateTime.UtcNow.ToString("hh:mm:ss")} [{severity}] {source}: {message} => {exception}";
            File.AppendAllText(_logfile, logText + Environment.NewLine);  
#endif




            ss.Release(1);
            return Task.CompletedTask;
        }       

        internal Task LogLavalink(LogMessage arg1)
        => LogEventAsync(arg1);

        internal void LogDebug(string Message, LogSource source = LogSource.Unkown, Exception exception = null)
        => LogEventAsync(new LogMessage(LogSeverity.Debug, source.ToString(), Message, exception));

        internal void LogWarning(string Message, LogSource source = LogSource.Unkown, Exception exception = null)
        => LogEventAsync(new LogMessage(LogSeverity.Warning, source.ToString(), Message, exception));

        internal void LogVerbose(string Message, LogSource source = LogSource.Unkown, Exception exception = null)
        => LogEventAsync(new LogMessage(LogSeverity.Verbose, source.ToString(), Message, exception));

        internal void LogCritical(string Message, LogSource source = LogSource.Unkown, Exception exception = null)
        => LogEventAsync(new LogMessage(LogSeverity.Critical, source.ToString(), Message, exception));

        internal void LogError(string Message, LogSource source = LogSource.Unkown, Exception exception = null)
        => LogEventAsync(new LogMessage(LogSeverity.Error, source.ToString(), Message, exception));

        internal void LogInformation(string Message, LogSource source = LogSource.Unkown, Exception exception = null)
        => LogEventAsync(new LogMessage(LogSeverity.Info, source.ToString(), Message, exception));       
    }
}