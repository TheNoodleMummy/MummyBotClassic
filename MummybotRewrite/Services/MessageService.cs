using Casino.Common;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Mummybot.Commands;
using Mummybot.Database;
using Mummybot.Database.Entities;
using Mummybot.Enums;
using Mummybot.Exceptions;
using Mummybot.Extentions;
using Qmmands;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Mummybot.Services
{
    public class MessageService : BaseService
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        private readonly LogService _logger;
        private readonly TaskQueue _scheduler;
        private readonly HttpClient _http;
        private readonly IServiceProvider _services;

        private readonly ConcurrentDictionary<ulong, ConcurrentDictionary<ulong, ConcurrentDictionary<Guid, ScheduledTask<(Guid, CachedMessage)>>>> _messageCache;

        private readonly ConcurrentDictionary<ulong, ulong> _lastJumpUrlQuotes;

        private readonly ConcurrentDictionary<ulong, byte> _quoteReactions;

        private const string Regex = @"(?:https://(?:canary.)?discord.com/channels/[\d]+/[\d]+/[\d]+)";

        private static readonly Emoji QuoteEmote = new Emoji("🗨");

        private static TimeSpan MessageLifeTime => TimeSpan.FromMinutes(10);

        public MessageService(IServiceProvider services, LogService logs)
        {
            _commands = services.GetRequiredService<CommandService>();
            _client = services.GetRequiredService<DiscordSocketClient>();
            _scheduler = services.GetRequiredService<TaskQueue>();
            _http = services.GetRequiredService<HttpClient>();
            _services = services;
            _logger = logs;
            _messageCache =
                new ConcurrentDictionary<ulong, ConcurrentDictionary<ulong,
                    ConcurrentDictionary<Guid, ScheduledTask<(Guid, CachedMessage)>>>>();

            _lastJumpUrlQuotes = new ConcurrentDictionary<ulong, ulong>();

            _quoteReactions = new ConcurrentDictionary<ulong, byte>();
        }

        public override Task InitialiseAsync(IServiceProvider services)
        {
            _commands.CommandExecutionFailed += CommandErroredAsync; ; //fires for parralel command that had a exception
            _commands.CommandExecuted += CommandExecutedAsync; ; ;//fires for parralel command that went ok
            var jumpUrlRegex = new Regex(Regex, RegexOptions.Compiled);

            _client.MessageReceived += msg =>
                msg is SocketUserMessage message
                    ? HandleReceivedMessageAsync(message, false)
                    : Task.CompletedTask;

            _client.MessageReceived += msg =>
            {
                if (msg.Channel is IDMChannel)
                    return Task.CompletedTask;

                var match = jumpUrlRegex.Match(msg.Content);

                if (match.Success)
                {
                    _lastJumpUrlQuotes[msg.Channel.Id] = msg.Id;

                    _scheduler.ScheduleTask(MessageLifeTime, () =>
                    {
                        _lastJumpUrlQuotes.TryRemove(msg.Channel.Id, out _);

                        return Task.CompletedTask;
                    });
                }
                return Task.CompletedTask;
            };

            _client.ReactionAdded += async (cache, channel, reaction) =>
            {
                if (!reaction.Emote.Equals(QuoteEmote) || _quoteReactions.ContainsKey(cache.Id) ||
                    channel is IDMChannel)
                    return;

                var message = await cache.GetOrDownloadAsync();

                if (message is null)
                    return;

                var embed = await Utilities.QuoteFromString(_client, message.Content);

                if (embed is null)
                    return;

                await message.Channel.SendMessageAsync(string.Empty, embed: embed);

                _quoteReactions.TryAdd(cache.Id, 0);

                _scheduler.ScheduleTask(MessageLifeTime, () =>
                {
                    _quoteReactions.TryRemove(cache.Id, out _);

                    return Task.CompletedTask;
                });
            };

            _client.ReactionAdded += async (cache, channel, reaction) =>
            {
                if (!reaction.Emote.Equals(QuoteEmote) || _quoteReactions.ContainsKey(cache.Id) ||
                   channel is IDMChannel)
                    return;

                var message = await cache.GetOrDownloadAsync();

                if (message is null)
                    return;

                var embed = await Utilities.QuoteFromMessage(message);

                if (embed is null)
                    return;

                await message.Channel.SendMessageAsync(string.Empty, embed: embed);

                _quoteReactions.TryAdd(cache.Id, 0);

                _scheduler.ScheduleTask(MessageLifeTime, () =>
                {
                    _quoteReactions.TryRemove(cache.Id, out _);

                    return Task.CompletedTask;
                });
            };

            _client.MessageUpdated += (before, msg, __) =>
                before.HasValue && msg is SocketUserMessage after && after.Content != before.Value.Content
                    ? HandleReceivedMessageAsync(after, true)
                    : Task.CompletedTask;
            return Task.CompletedTask;
        }

       

        private async Task HandleReceivedMessageAsync(SocketUserMessage message, bool isEdit)
        {
            if (message.Author.IsBot)
                return;
          
            if (message.Channel is IDMChannel channel)
            {
                var c = _client.GetUser(122520984413667331);
                if (message.Attachments.Count != 0)
                {
<<<<<<< HEAD
                    using var stream = await _http.GetStreamAsync(message.Attachments.First().Url);
                    await c.SendFileAsync(stream, message.Attachments.First().Filename, $"```i recieved a dm from {message.Author} message passed:\n{message.Content}```");
=======
                    foreach (var attachment in message.Attachments)
                    {
                        using var stream = await _http.GetStreamAsync(message.Attachments.First().Url);
                        await c.SendFileAsync(stream, message.Attachments.First().Filename, $"```i recieved a dm from {message.Author} message passed:\n{message.Content}```");
                    }
                    
>>>>>>> made it so attachments in dms also get forwarded + akk gateway intents cause fuck them + updated stuff
                }
                else
                    await c.SendMessageAsync($"```i recieved a dm from {message.Author} message passed:\n{message.Content}```");
            }

            if (!(message.Channel is SocketTextChannel textChannel) || !textChannel.Guild.CurrentUser.GetPermissions(textChannel).Has(ChannelPermission.SendMessages))
                return;


            IEnumerable<string> prefixes;

            Guild guild;
            using (var guildStore = _services.GetService<GuildStore>())
            {

                guild = await guildStore.GetOrCreateGuildAsync(textChannel.Guild);
                prefixes = guild.Prefixes.Select(x => x.Prefix);
                guildStore.SaveChanges();
            }
            if (guild.AutoQuotes && !_quoteReactions.ContainsKey(message.Id))
                _ = Task.Run(async () =>
                {
                    var embed = await Utilities.QuoteFromString(_client, message.Content);

                    if (embed is null)
                        return;

                    await message.Channel.SendMessageAsync(string.Empty, embed: embed);

                    _quoteReactions.TryAdd(message.Id, 0);

                    _scheduler.ScheduleTask(message.Id, MessageLifeTime, mId =>
                    {
                        _quoteReactions.TryRemove(mId, out _);
                        return Task.CompletedTask;
                    });
                });


            if (CommandUtilities.HasAnyPrefix(message.Content, prefixes, StringComparison.CurrentCulture, out var prefix, out var output) ||
                CommandUtilities.HasPrefix(message.Content, $"<@{_client.CurrentUser.Id}>", StringComparison.Ordinal, out output) ||
                    CommandUtilities.HasPrefix(message.Content, $"<@!{_client.CurrentUser.Id}>", StringComparison.Ordinal, out output))
            {
                if (string.IsNullOrWhiteSpace(output))
                    return;

                try
                {
                    prefix ??= _client.CurrentUser.Mention;

                    var commandContext = MummyContext.Create(_client, message, _services.GetRequiredService<HttpClient>(), _services, prefix, isEdit);

                    var result = await _commands.ExecuteAsync(output, commandContext);



                    if (result is CommandNotFoundResult notfoundresult)
                    {
                        var emb = new EmbedBuilder();
                        emb.WithAuthor(commandContext.User.GetDisplayName(), commandContext.User.GetAvatarOrDefaultUrl());
                        emb.WithDescription("Could not find any command with that name");
                        await SendMessageAsync(commandContext, new MessageProperties() { Embed = emb.Build() });
                        _logger.LogInformation(notfoundresult.ToString(), LogSource.Commands, commandContext.GuildId);
                    }
                    else if (result is OverloadsFailedResult overloadsFailedResult)
                    {

                        Console.Write(string.Join(Environment.NewLine, overloadsFailedResult.FailedOverloads.Select(x => $"{x.Key}: {x.Value}")));
                    }
                    else if (result is ChecksFailedResult checks)
                    {
                        var emb = new EmbedBuilder();
                        emb.WithAuthor(commandContext.User.GetDisplayName(), commandContext.User.GetAvatarOrDefaultUrl());
                        emb.WithTitle($"{checks.FailedChecks.Count} checks failed");
                        foreach (var (Check, Result) in checks.FailedChecks.Take(10))
                        {
                            var attri = Check.GetType().GetCustomAttributes(typeof(Attributes.NameAttribute), false).FirstOrDefault();
                            var nameattri = (attri as Attributes.NameAttribute).Name;
                            emb.AddField(nameattri, Result.FailureReason, true);
                        }
                        await SendMessageAsync(commandContext, new MessageProperties() { Embed = emb.Build() });
                    }
                    else if (result is TypeParseFailedResult parser)
                    {
                        var emb = new EmbedBuilder();
                        emb.WithAuthor(commandContext.User.GetDisplayName(), commandContext.User.GetAvatarOrDefaultUrl());
                        emb.AddField(parser.Parameter.Name, parser.Value);
                        await SendMessageAsync(commandContext, new MessageProperties() { Embed = emb.Build() });
                    }
                    //else if (result is ArgumentParseFailedResult argumentParse)
                    //{
                    //    argumentParse.
                    //    var emb = new EmbedBuilder();
                    //    var postion = (argumentParse. .ParserResult.Arguments.Keys.First(). ?? 1;
                    //    emb.AddField(argumentParse.Reason, $"{argumentParse.RawArguments}\n{string.Concat(Enumerable.Repeat(" ", postion - 1))}^");
                    //    SendMessageAsync(commandContext, new MessageProperties() { Embed = emb.Build() });
                    //}
                    else if (result is CommandOnCooldownResult ccr)
                    {
                        var emb = new EmbedBuilder();
                        emb.WithAuthor(commandContext.User.GetDisplayName(), commandContext.User.GetAvatarOrDefaultUrl());
                        emb.WithTitle(ccr.Command.Name + " is on cooldown");
                        foreach (var item in ccr.Cooldowns.Take(2))
                        {
                            if (Enum.TryParse<CooldownBucketType>(item.Cooldown.BucketType.ToString(), true, out var enumeration))
                                switch (enumeration)
                                {
                                    case CooldownBucketType.Guilds:
                                        emb.WithAuthor(commandContext.Guild.Name, commandContext.Guild.IconUrl);
                                        emb.AddField("Guild on Cooldown", $"retry in {item.RetryAfter}");
                                        break;
                                    case CooldownBucketType.Channels:
                                        emb.WithAuthor(commandContext.Channel.Name);
                                        emb.AddField("channel on Cooldown", $"retry in {item.RetryAfter}");
                                        break;
                                    case CooldownBucketType.User:
                                        emb.WithAuthor(commandContext.User.GetDisplayName(), commandContext.User.GetAvatarOrDefaultUrl());
                                        emb.AddField("user on Cooldown", $"retry in {item.RetryAfter}");
                                        break;
                                    case CooldownBucketType.Global:
                                        emb.WithAuthor(commandContext.Guild.CurrentUser.GetDisplayName(), commandContext.Guild.CurrentUser.GetAvatarOrDefaultUrl());
                                        emb.AddField("Global on Cooldown", $"retry in {item.RetryAfter}");
                                        break;
                                    default:
                                        throw new InvalidOperationException($"got unhandled bucketType");
                                }
                            else
                                throw new InvalidOperationException($"got invalid CooldownBucketType expected: {typeof(CooldownBucketType)} got: {item.Cooldown.BucketType}");
                        }
                        await SendAsync(commandContext, msg => msg.Embed = emb.Build());
                    }


//                    else if (result is ExecutionFailedResult failed)
//                    {
//                        _logger.LogError(failed.ToString(), LogSource.Commands, commandContext.GuildId, failed.Exception);

//#if !DEBUG
//                        var expetionlenght = (failed.Exception?.ToString().Length <= 1000 ? failed.Exception?.ToString().Length : 1000);
//                        var c = _client.GetChannel(484898662355566593) as SocketTextChannel;
//                        await c.SendMessageAsync($"```{commandContext.Command} failed for {commandContext.User.GetDisplayName()}" +
//                        $"message passed {commandContext.Message.Content}```{Format.Sanitize(failed.Exception?.ToString().Substring(0,expetionlenght))}");
//#endif
//                        await SendAsync(commandContext, x => x.Embed = Utilities.BuildErrorEmbed(failed, commandContext));

//                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Issue with message service", LogSource.Commands,exception: ex);
                }
            }
        }

        private async ValueTask CommandErroredAsync(object sender, CommandExecutionFailedEventArgs args)
        {
            if ((args.Context is MummyContext context))
            {
                if (args.Result is CommandExecutionFailedResult failed)
                {
                    _logger.LogError(failed.ToString(), LogSource.Commands,context.GuildId, failed.Exception);

#if !DEBUG
                    var expetionlenght = (failed.Exception.ToString().Length <= 1000 ? failed.Exception.ToString().Length : 1000);
                    var c = _client.GetChannel(484898662355566593) as SocketTextChannel;
                    await c.SendMessageAsync($"```{context.Command} failed for {context.User.GetDisplayName()}" +
                    $"message passed {context.Message.Content}```{Format.Sanitize(failed.Exception?.ToString().Substring(0,expetionlenght))}");
#endif
                }

                await SendAsync(context, x => x.Embed = Utilities.BuildErrorEmbed(args.Result, context));
            }
            else
                throw new InvalidContextException(args.Context.GetType());
        }

        private async ValueTask CommandExecutedAsync(object sender, CommandExecutedEventArgs args)
        {
            if ((args.Context is MummyContext context))
            {
                _logger.LogVerbose($"Successfully executed {{{context.Command.Name}}} for {{{context.User.GetDisplayName()}}}", LogSource.Commands,context.GuildId);

                await Task.Delay(1);
            }
            else
                throw new InvalidContextException(args.Context.GetType());
        }

        public async Task<IUserMessage> SendAsync(MummyContext context, Action<MessageProperties> properties)
        {
            if (!_messageCache.TryGetValue(context.Channel.Id, out var foundChannel))
                foundChannel = _messageCache[context.Channel.Id] =
                    new ConcurrentDictionary<ulong, ConcurrentDictionary<Guid, ScheduledTask<(Guid, CachedMessage)>>>();

            if (!foundChannel.TryGetValue(context.User.Id, out var foundCache))
                foundCache = foundChannel[context.User.Id] =
                    new ConcurrentDictionary<Guid, ScheduledTask<(Guid, CachedMessage)>>();

            var messageProperties = properties.Invoke();

            var (guid, value) = foundCache.FirstOrDefault(x => x.Value.State.Item2.ExecutingId == context.Message.Id);

            IUserMessage sentMessage;

            if (value is null)
            {
                sentMessage = await SendMessageAsync(context, messageProperties);

                var message = new CachedMessage(context, sentMessage);

                var key = Guid.NewGuid();

                var task = _scheduler.ScheduleTask((key, message), MessageLifeTime, RemoveAsync);

                _messageCache[context.Channel.Id][context.User.Id][key] = task;

                return sentMessage;
            }

            if (context.IsEdit)
            {
                context.IsEdit = false;

                var perms = context.Guild.CurrentUser.GetPermissions(context.Channel).ManageMessages;
                await DeleteMessagesAsync(context, perms, new[] { (guid, value) });

                sentMessage = await SendMessageAsync(context, messageProperties);

                var message = new CachedMessage(context, sentMessage);

                var key = Guid.NewGuid();

                var task = _scheduler.ScheduleTask((key, message), MessageLifeTime, RemoveAsync);

                _messageCache[context.Channel.Id][context.User.Id][key] = task;

                return sentMessage;
            }

            sentMessage = await SendMessageAsync(context, messageProperties);
            value.State.Item2.ResponseIds.Add(sentMessage.Id);

            return sentMessage;
        }

        //async needed for the cast//what cast tho?
        private static async Task<IUserMessage> SendMessageAsync(MummyContext context, MessageProperties properties)
        {
            if (properties.Stream is null)
            {
                return await context.Channel.SendMessageAsync(properties.Content, embed: properties.Embed);
            }

            return await context.Channel.SendFileAsync(
                stream: properties.Stream,
                filename: properties.FileName,
                text: properties.Content,
                embed: properties.Embed);
        }

        private Task<IMessage> GetOrDownloadMessageAsync(ulong channelId, ulong messageId)
        {
            if (!(_client.GetChannel(channelId) is SocketTextChannel channel))
                return null;

            return !(channel.GetCachedMessage(messageId) is IMessage message)
                ? channel.GetMessageAsync(messageId)
                : Task.FromResult(message);
        }

        private Task RemoveAsync((Guid, CachedMessage) obj)
        {
            var (key, message) = obj;

            _messageCache[message.ChannelId][message.UserId].TryRemove(key, out _);

            if (_messageCache[message.ChannelId][message.UserId].IsEmpty)
                _messageCache.Remove(message.UserId, out _);

            if (_messageCache[message.ChannelId].IsEmpty)
                _messageCache.Remove(message.ChannelId, out _);

            return Task.CompletedTask;
        }

        public async Task DeleteMessagesAsync(MummyContext context, int amount)
        {
            var perms = context.Guild.CurrentUser.GetPermissions(context.Channel);
            var manageMessages = perms.ManageMessages;

            var deleted = 0;

            do
            {
                if (!_messageCache.TryGetValue(context.Channel.Id, out var foundCache))
                    return;

                if (!foundCache.TryGetValue(context.User.Id, out var found))
                    return;

                if (found is null)
                    return;

                if (found.IsEmpty)
                {
                    _messageCache[context.Channel.Id].Remove(context.User.Id, out _);

                    if (_messageCache[context.Channel.Id].IsEmpty)
                        _messageCache.Remove(context.Channel.Id, out _);

                    return;
                }

                var ordered = found.OrderByDescending(x => x.Value.State.Item2.CreatedAt).ToArray();

                amount = amount > ordered.Length ? ordered.Length : amount;

                var toDelete = new List<(Guid, ScheduledTask<(Guid, CachedMessage)>)>();

                for (var i = 0; i < amount; i++)
                    toDelete.Add((ordered[i].Key, ordered[i].Value));

                var res = await DeleteMessagesAsync(context, manageMessages, toDelete);
                deleted += res;

            } while (deleted < amount);
        }

        private async Task<int> DeleteMessagesAsync(MummyContext context, bool manageMessages,
            IEnumerable<(Guid Key, ScheduledTask<(Guid, CachedMessage)> Task)> messages)
        {
            var fetchedMessages = new List<IMessage>();

            foreach (var (_, task) in messages)
            {
                var item = task.State;

                await RemoveAsync(item);
                task.Cancel();

                foreach (var id in item.Item2.ResponseIds)
                    fetchedMessages.Add(await GetOrDownloadMessageAsync(item.Item2.ChannelId, id));
            }

            if (manageMessages)
            {
                await context.Channel.DeleteMessagesAsync(fetchedMessages);
            }
            else
            {
                foreach (var message in fetchedMessages)
                    await context.Channel.DeleteMessageAsync(message);
            }

            return fetchedMessages.Count;
        }

        public bool TryGetLastJumpMessage(ulong channelId, out ulong messageId)
            => _lastJumpUrlQuotes.TryGetValue(channelId, out messageId);

        private class CachedMessage
        {
            public ulong ChannelId { get; }
            public IList<ulong> ResponseIds { get; }
            public ulong ExecutingId { get; }
            public ulong UserId { get; }
            public long CreatedAt { get; }

            public CachedMessage(MummyContext context, IMessage message)
            {
                ChannelId = context.Channel.Id;
                UserId = context.User.Id;
                ExecutingId = context.Message.Id;
                ResponseIds = new List<ulong>
                {
                    message.Id
                };
                CreatedAt = message.CreatedAt.ToUnixTimeMilliseconds();
            }
        }

        public class MessageProperties
        {
            public string Content { get; set; }
            public Embed Embed { get; set; }
            public Stream Stream { get; set; }
            public string FileName { get; set; }
        }
    }
}
