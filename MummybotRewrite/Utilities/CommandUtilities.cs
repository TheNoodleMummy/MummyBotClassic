using Discord;
using Discord.WebSocket;
using Humanizer;
using Mummybot.Commands;
using Mummybot.Exceptions;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mummybot
{
    public static partial class Utilities
    {
       
        public static Embed BuildErrorEmbed(FailedResult result, MummyContext context)
        {
            var builder = new EmbedBuilder
            {
                Author = new EmbedAuthorBuilder
                {
                    IconUrl = context.User.GetAvatarOrDefaultUrl(),
                    Name = context.User.GetDisplayName()
                },
                Color = new Color(0xff6868)
            };

            string message;

            switch (result)
            {
                //case ArgumentParseFailedResult argumentParseFailedResult:
                    //switch (argumentParseFailedResult.)
                    //{
                    //    case ArgumentParserFailure.UnclosedQuote:
                    //    case ArgumentParserFailure.UnexpectedQuote:
                    //    case ArgumentParserFailure.NoWhitespaceBetweenArguments:
                    //    case ArgumentParserFailure.TooManyArguments:

                    //        var position = argumentParseFailedResult.Position ??
                    //                       throw new QuahuLiedException("Result.Position");

                    //        var padding = position + context.PrefixUsed.Length 
                    //                               + string.Join(' ', context.Path).Length + 2;

                    //        var leftPad = "^".PadLeft(padding, ' ');
                    //        var rightPad = leftPad.PadRight(context.Message.Content.Length, '^');

                    //        message = string.Concat(
                    //            result.Reason,
                    //            "\n```",
                    //            $"\n{context.Message.Content}\n",
                    //            rightPad,
                    //            "\n```");

                    //        builder.WithDescription(message);
                    //        break;

                    //    case ArgumentParserFailure.TooFewArguments:

                    //        var cmd = argumentParseFailedResult.Command;
                    //        var parameters = cmd.Parameters;

                    //        var response = string.Concat(
                    //            result.Reason,
                    //            "\n",
                    //            cmd.FullAliases.First(),
                    //            " ",
                    //            string.Join(' ', parameters.Select(x => x.Name)));

                    //        builder.WithDescription(response);
                    //        break;
                    //}
                    //break;

                case ChecksFailedResult checksFailedResult:
                    message = string.Concat(
                        result.FailureReason,
                        '\n',
                        string.Join('\n', checksFailedResult.FailedChecks.Select(x => x.Result.FailureReason)));

                    builder.WithDescription(message);
                    break;

                case CommandOnCooldownResult commandOnCooldownResult:
                    message = string.Concat(
                        "You are currently on cooldown for this command",
                        '\n',
                        "Retry in: ",
                        commandOnCooldownResult.Cooldowns[0].RetryAfter.Humanize(1)
                        );

                    builder.WithDescription(message);
                    break;
                                    
                case CommandExecutionFailedResult _:
                    builder.WithDescription("Something went horribly wrong... " +
                                            "The problem has been forwarded to the appropiate authorities");
                    break;

                case OverloadsFailedResult overloadsFailedResult:
                    message = overloadsFailedResult.FailedOverloads.OrderBy(x => x.Key.Priority).Last().Value.FailureReason;

                    builder.WithDescription(message);
                    break;

                case ParameterChecksFailedResult parameterChecksFailedResult:
                    message = string.Concat(
                        result.FailureReason,
                        "\n",
                        string.Join('\n', parameterChecksFailedResult.FailedChecks.Select(x => x.Result.FailureReason)));

                    builder.WithDescription(message);
                    break;

                case TypeParseFailedResult typeParseFailedResult:
                    builder.WithDescription(typeParseFailedResult.FailureReason);
                    break;
            }

            return builder.Build();
        }

        public static IReadOnlyDictionary<Type, string> ExampleUsage = new Dictionary<Type, string>
        {
            [typeof(IGuildUser)] = "@user",
            [typeof(TimeSpan)] = "1day3hrs14mins30s",
            [typeof(SocketRole)] = "@role",
            [typeof(SocketTextChannel)] = "#channel",
        };
    }
}
