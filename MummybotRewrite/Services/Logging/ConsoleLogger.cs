using Discord;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mummybot.Services.Logging
{
    class ConsoleLogger(string name) : ILogger
    {
        public static SemaphoreSlim ss = new SemaphoreSlim(1, 1);

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => default!;


        public bool IsEnabled(LogLevel logLevel)
        => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            ss.Wait();
            var _source = name;
            var _message = formatter(state,exception);
            var _exception = exception;
            var _severity = logLevel;

            var time = DateTime.Now;

            Console.Write($"{(time.Day < 10 ? "0" : "")}{time.Day}-{(time.Month < 10 ? "0" : "")}{time.Month}-{time.Year} {(time.Hour < 10 ? "0" : "")}{time.Hour}:{(time.Minute < 10 ? "0" : "")}{time.Minute}:{(time.Second < 10 ? "0" : "")}{time.Second}");

            Console.Write("[");
            Console.ForegroundColor = _severity switch
            {
                LogLevel.Critical => ConsoleColor.DarkRed,
                LogLevel.Error => ConsoleColor.Red,
                LogLevel.Warning => ConsoleColor.DarkYellow,
                LogLevel.Information => ConsoleColor.DarkGreen,
                LogLevel.Debug => ConsoleColor.Magenta,
                LogLevel.Trace => ConsoleColor.Yellow,
                LogLevel.None => ConsoleColor.White,
                _ => throw new ArgumentOutOfRangeException(),
            };
            const int sevLength = 8;
            if (_severity.ToString().Length < sevLength)
            {
                var builder = new StringBuilder(sevLength);
                builder.Append(_severity.ToString());
                builder.Append(' ', sevLength - _severity.ToString().Length);
                Console.Write($"{builder}");
            }
            else if (_severity.ToString().Length > sevLength)
            {
                Console.Write($"{_severity.ToString().Substring(0, sevLength)}");
            }
            else
            {
                Console.Write(_severity.ToString());
            }
            Console.ResetColor();
            Console.Write("]");

            Console.Write("[");
            Console.ForegroundColor = ConsoleColor.Cyan;
            if (_source.Length < 11)
            {
                var builder = new StringBuilder(11);
                builder.Append(_source);
                builder.Append(' ', 11 - _source.Length);
                Console.Write($"{builder}");
            }
            else if (_source.Length > 11)
            {
                Console.Write($"{_source.Substring(0, 11)}");
            }
            else
            {
                Console.Write(_source);
            }
            Console.ResetColor();
            Console.Write("] ");

            //if (log.Guild != null)
            //{
            //    Console.Write("[");
            //    Console.ForegroundColor = ConsoleColor.White;

            //    if (log.Guild.Id.ToString().Length > 20)
            //    {
            //        Console.Write($"Id to long");
            //    }
            //    else
            //    {
            //        Console.Write(log.Guild.Id.ToString());
            //    }
            //    Console.Write("/");
            //    if (log.Guild.Name.Length < 15)
            //    {
            //        var builder = new StringBuilder(15);
            //        builder.Append(log.Guild.Name);
            //        builder.Append(' ', 15 - log.Guild.Name.Length);
            //        Console.Write($"{builder}");
            //    }
            //    else if (log.Guild.Name.Length > 15)
            //    {
            //        Console.Write($"{log.Guild.Name.Substring(0, 15)}");
            //    }
            //    else
            //    {
            //        Console.Write(log.Guild.Name);
            //    }
            //    Console.ResetColor();
            //    Console.Write("] ");
            //}
            //else
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


            if (!string.IsNullOrEmpty(_message))
                Console.Write(string.Join("", _message.Where(x => !char.IsControl(x))));

            if (!string.IsNullOrEmpty(exception?.ToString()))
            {
                if (!string.IsNullOrEmpty(exception?.ToString()))
                {
                    Console.WriteLine(" " + exception.Message);
                    Console.WriteLine(exception.StackTrace);
                }
            }

            Console.WriteLine();


            ss.Release(1);
            return;
        }
    }
}
