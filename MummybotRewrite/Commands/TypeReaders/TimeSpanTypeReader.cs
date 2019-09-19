using Qmmands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Commands.TypeReaders
{
    public class TimeSpanTypeparser : MummyTypeParser<TimeSpan>
    {
        private static readonly string[] Formats = {
            "%d'd'%h'h'%m'm'%s's'", //4d3h2m1s
            "%d'd'%h'h'%m'm'",      //4d3h2m
            "%d'd'%h'h'%s's'",      //4d3h  1s
            "%d'd'%h'h'",           //4d3h
            "%d'd'%m'm'%s's'",      //4d  2m1s
            "%d'd'%m'm'",           //4d  2m
            "%d'd'%s's'",           //4d    1s
            "%d'd'",                //4d
            "%h'h'%m'm'%s's'",      //  3h2m1s
            "%h'h'%m'm'",           //  3h2m
            "%h'h'%s's'",           //  3h  1s
            "%h'h'",                //  3h
            "%m'm'%s's'",           //    2m1s
            "%m'm'",                //    2m
            "%s's'",                //      1s
        };

        public override ValueTask<TypeParserResult<TimeSpan>> ParseAsync(Parameter parameter, string value, MummyContext context)

        {
            return (TimeSpan.TryParseExact(value.ToLowerInvariant(), Formats, CultureInfo.InvariantCulture, out TimeSpan timeSpan))
                ? new TypeParserResult<TimeSpan>(timeSpan)
                : new TypeParserResult<TimeSpan>($"could not parse \"{value}\" as a timespan.");

        }
    }
}
