using Discord;
using Discord.WebSocket;
using Mummybot.Attributes;
using Mummybot.Commands;
using Mummybot.Entities;
using Qmmands;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;


namespace Mummybot.Services
{
    [Service("Message Service",typeof(MessagesService))]
    public class MessagesService
    {
        [Inject]
        DBService _database;
        [Inject]
        GuildService _guildService;
        [Inject]
        DiscordSocketClient _client;
        [Inject]
        private CommandService _commandService;
        [Inject]
        private IServiceProvider _services;
        [Inject]
        private LogService _logs;
        [Inject]
        HttpClient _httpClient;



        private readonly ConcurrentDictionary<ulong, ConcurrentQueue<Message>> _messageCache = new ConcurrentDictionary<ulong, ConcurrentQueue<Message>>();
        private readonly int CacheSize = 10;

        public async Task HandleMessageAsync(SocketMessage msg)
        {
            try
            {


                //if (msg is null)
                //    return;

                if (msg.Author.IsBot ||
                    string.IsNullOrEmpty(msg.Content) ||
                    !(msg.Channel is SocketGuildChannel channel) ||
                    !(msg is SocketUserMessage message))
                    return;

                var guildconfig = _database.GetGuildUncached(channel.Guild);

                if (CommandUtilities.HasPrefix(message.Content, $"<@{_client.CurrentUser.Id}>", StringComparison.Ordinal, out var leftover) ||
                    CommandUtilities.HasPrefix(message.Content, $"<@!{_client.CurrentUser.Id}>", StringComparison.Ordinal, out leftover) ||
                    guildconfig.Prefixes.Any(x => CommandUtilities.HasPrefix(message.Content, x, StringComparison.OrdinalIgnoreCase, out leftover)))
                {

                    if (guildconfig.BlackList.Any(x => x.UserID == message.Author.Id) && !guildconfig.IsBlackListed)
                    {
                        _logs.LogInformation($"Dumped Message from Blacklisted user/guild {message.Author}/{message.Author.Id} in {channel.Guild}/{channel.Guild.Id}", Enums.LogSource.MessageService);

                        return;
                    }
                    var context = new MummyContext(_client, message, _httpClient);
                    await HandleCommandAsync(context, leftover);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
        }

        public async Task HandleCommandAsync(MummyContext context, string command)
        {
            if (!context.Guild.CurrentUser.GetPermissions(context.Channel).SendMessages)
            {
                _logs.LogError($"Missing Send Message permissions {context.Channel}/{context.Guild}", Enums.LogSource.MessageService);
                return;
            }

            var result = await _commandService.ExecuteAsync(command, context, _services);

            if (result is FailedResult failedResult)
            {
                var guild = await _guildService.GetGuildAsync(context.Guild);
                switch (result)
                {

                    case CommandNotFoundResult search:
                        _logs.LogError($"{search} in {context.Channel}/{context.Guild}", Enums.LogSource.Commands);

                        if (guild.UnknownCommands)
                        {
                            var prefixes = guild.Prefixes;
                            if (CommandUtilities.HasPrefix(context.Message.Content, _client.CurrentUser.Mention, StringComparison.OrdinalIgnoreCase, out string leftover) ||
                                guild.Prefixes.Any(x => CommandUtilities.HasPrefix(context.Message.Content, x, StringComparison.OrdinalIgnoreCase, out leftover))) return;

                            var i = 1;
                            var commands = _commandService.FindCommands(leftover)?.Select(x => $"{i++}{x.Command.Aliases.First()}")?.ToList();
                            if (commands != null && commands.Count != 0)
                            {
                                await SendMessageAsync(context, "Command not found, did you mean:\n" +
                                                            $"{string.Join("\n", commands, 0, 3)}");
                            }
                            else
                            {
                                await SendMessageAsync(context, "No command found, Also no similiar commands found");
                            }
                        }
                        break;


                    case TypeParseFailedResult reader://only for actual failed parsing
                        _logs.LogError($"the parameter {reader.Parameter} failed to parse {reader.Value} because: {reader.Reason}", Enums.LogSource.Commands);
                        if (guild.AdvancedCommandErrors)
                            await SendMessageAsync(context, $"the parameter {reader.Parameter} failed to parse {reader.Value} because: {reader.Reason}");
                        break;

                    case ArgumentParseFailedResult parseFailed://only for things like missing quotes and shit not for actual failed parsing
                        switch (parseFailed.ArgumentParserFailure)
                        {
                            case ArgumentParserFailure.UnclosedQuote:
                                _logs.LogError($"Missing a Quote on {parseFailed.Parameter.Name} from {parseFailed.Command.Name} in {context.Guild.Name}/{context.Guild.Id}", Enums.LogSource.Commands);
                                if (guild.AdvancedCommandErrors)
                                    await SendMessageAsync(context, $"im sorry, but you failed to quote {parseFailed.Parameter.Name} from {parseFailed.Command.Name}");

                                break;

                            case ArgumentParserFailure.UnexpectedQuote:
                                _logs.LogError($"Found a lost quote at {parseFailed.Position} for {parseFailed.Command.Name}");
                                if (guild.AdvancedCommandErrors)
                                    await SendMessageAsync(context, $"oh, look a lost quote sign (@{parseFailed.Position})");
                                break;
                            case ArgumentParserFailure.NoWhitespaceBetweenArguments:
                                _logs.LogError($"Found a lost quote at {parseFailed.Position} for {parseFailed.Command.Name}");
                                if (guild.AdvancedCommandErrors)
                                    await SendMessageAsync(context, $"oh,your missing a space (@{parseFailed.Position})");
                                break;
                            case ArgumentParserFailure.TooFewArguments:
                                _logs.LogError($"to few arguments to run {parseFailed.Command} required {parseFailed.Command.Parameters.Count} got {parseFailed.Arguments.Count}");
                                if (guild.AdvancedCommandErrors)
                                    await SendMessageAsync(context, $"oh, looks like we are missing some parameters required: {parseFailed.Command.Parameters.Count} got: {parseFailed.Arguments.Count}");
                                break;
                            case ArgumentParserFailure.TooManyArguments:
                                _logs.LogError($"to many arguments to run {parseFailed.Command} required {parseFailed.Command.Parameters.Count} got {parseFailed.Arguments.Count}");
                                if (guild.AdvancedCommandErrors)
                                    await SendMessageAsync(context, $"oh, looks like we have to many parameters, required: {parseFailed.Command.Parameters.Count} got: {parseFailed.Arguments.Count}");
                                break;
                        }
                        break;
                    case ChecksFailedResult precondition:
                        _logs.LogError($"command: {precondition.Command.FullAliases.FirstOrDefault()}, failed because failed check('s) ({string.Join(", ", precondition.FailedChecks)}) {context.Channel}/{context.Guild}", Enums.LogSource.Commands);
                        if (guild.AdvancedCommandErrors)
                            await SendMessageAsync(context, $"command: {precondition.Command.Aliases.FirstOrDefault()}, failed because failed check('s){string.Join('\n', precondition.FailedChecks)}");
                        break;

                    case ExecutionFailedResult execute:
                        _logs.LogCritical($"boop not so good", Enums.LogSource.Commands, execute.Exception.InnerException ?? execute.Exception);
                        if (guild.AdvancedCommandErrors)
                        {
                            await SendMessageAsync(context, "There was an error,This error has been reported. We will try to get a fix out as soon a possible");

                        }
                        await NewMessageAsync(0, 0, 484898662355566593, $"a exception happend for **{execute.Command}** in **{context.Channel}/{context.Guild}**: \n{execute.Exception}");
                        break;

                    default:
                        if (guild.AdvancedCommandErrors)
                        {
                            await SendMessageAsync(context, $"something went wrong. the issue is been reported and We will try to get a fix out as soon as possible");

                        }
                        await NewMessageAsync(0, 0, 484898662355566593, result.ToString());
                        break;
                }
            }

        }

        public async Task<IUserMessage> SendMessageAsync(ICommandContext ctx, string content = null, bool isTTS = false, Embed embed = null)
        {
            var context = ctx as MummyContext;
            var message = await GetExistingMessageAsync(context);
            if (message is null)
            {
                return await NewMessageAsync(context, content, isTTS, embed);
            }

            var currentUser = context.Guild.CurrentUser;
            var perms = currentUser.GetPermissions(context.Channel as IGuildChannel);

            if (perms.ManageMessages)
                await message.RemoveAllReactionsAsync();

            await message.ModifyAsync(x =>
            {
                x.Content = content;
                x.Embed = embed;
            });

            return message;
        }

        public Task<IUserMessage> NewMessageAsync(ICommandContext ctx, string content, bool isTTS = false, Embed embed = null)
        {
            var context = ctx as MummyContext;
            return NewMessageAsync(context.User.Id, context.Message.Id, context.Channel.Id, content, isTTS, embed);
        }
        public async Task<IUserMessage> NewMessageAsync(ulong userId, ulong executingId, ulong channelId, string content,
            bool isTTS = false, Embed embed = null)
        {
            if (!(_client.GetChannel(channelId) is SocketTextChannel channel)) return null;
            var response = await channel.SendMessageAsync(content, isTTS, embed);
            await NewItem(userId, channelId, response.CreatedAt, executingId, response.Id);

            return response;
        }

        //public async Task<IUserMessage> SendPaginatedMessageAsync(ICommandContext context, paginator)
        //{
        //    var message = await GetExistingMessageAsync(context);

        //    if (!(message is null))
        //    {
        //        await message.DeleteAsync();
        //    }

        //    TODO: add paginator

        //    await NewItem(context.User.Id, context.Channel.Id, callback.Message.CreatedAt, context.Message.Id,
        //        callback.Message.Id);

        //    return callback.Message;

        //}

        public async Task<int> ClearMessages(ICommandContext ctx, int amount)
        {
            var context = ctx as MummyContext;

            if (!_messageCache.TryGetValue(context.User.Id, out var found)) return 0;
            amount = amount > found.Count ? found.Count + 1 : amount;
            var matching = found.Where(x => x.ChannelId == context.Channel.Id).TakeWhile(item => amount-- != 0);
            var retrieved = new List<IMessage>();
            foreach (var item in matching)
            {
                var msg = await GetOrDownloadMessageAsync(context, item.ResponseId);
                if (msg is null) continue;
                retrieved.Add(msg);
            }

            var currentUser = context.Guild.CurrentUser;
            var perms = currentUser.GetPermissions(context.Channel as IGuildChannel);

            if (perms.ManageMessages)
            {
                if (context.Channel is ITextChannel channel)
                {
                    await channel.DeleteMessagesAsync(retrieved);
                }
            }
            else
            {
                foreach (var message in retrieved)
                    await context.Channel.DeleteMessageAsync(message);
            }

            var newQueue = new ConcurrentQueue<Message>();
            var delIds = retrieved.Select(x => x.Id);
            foreach (var item in found)
            {
                if (delIds.Contains(item.ResponseId)) continue;
                newQueue.Enqueue(item);
            }


            if (newQueue.IsEmpty)
                _messageCache.TryRemove(context.User.Id, out _);
            else
                _messageCache[context.User.Id] = newQueue;

            return retrieved.Count(x => !(x is null));
        }

        public async Task DeleteMessageAsync(ICommandContext ctx, IMessage message)
        {
            var context = ctx as MummyContext;

            if (_messageCache.TryGetValue(context.User.Id, out var found))
            {
                if (found.Any(x => x.ResponseId == message.Id))
                {
                    await message.DeleteAsync();
                    _messageCache[context.User.Id] =
                        new ConcurrentQueue<Message>(found.Where(x => x.ResponseId != message.Id));
                    if (_messageCache.TryGetValue(context.User.Id, out var newFound))
                    {
                        if (newFound.IsEmpty)
                            _messageCache.TryRemove(context.User.Id, out _);
                    }
                }
            }
        }
        public async Task DeleteMessagesAsync(ICommandContext ctx, IEnumerable<IMessage> messages)
        {
            var context = ctx as MummyContext;

            foreach (var message in messages)
            {
                if (_messageCache.TryGetValue(context.User.Id, out var found))
                {
                    if (found.Any(x => x.ResponseId == message.Id))
                    {
                        _messageCache[context.User.Id] =
                            new ConcurrentQueue<Message>(found.Where(x => x.ResponseId != message.Id));
                        if (_messageCache.TryGetValue(context.User.Id, out var newFound))
                        {
                            if (newFound.IsEmpty)
                                _messageCache.TryRemove(context.User.Id, out _);
                        }
                    }
                }
            }
            await context.Channel.DeleteMessagesAsync(messages);
        }

        private Task NewItem(ulong userId, ulong channelId, DateTimeOffset createdAt, ulong triggerid, ulong responseId)
        {
            _messageCache.TryAdd(userId, new ConcurrentQueue<Message>());
            if (!_messageCache.TryGetValue(userId, out var found)) return null;
            if (found.Count >= CacheSize)
                found.TryDequeue(out _);

            var newMessage = new Message
            {
                UserId = userId,
                ChannelId = channelId,
                CreatedAt = createdAt,
                TriggerID = triggerid,
                ResponseId = responseId,


            };

            found.Enqueue(newMessage);

            _messageCache[userId] = found;
            return Task.CompletedTask;
        }

        private async Task<IUserMessage> GetExistingMessageAsync(ICommandContext ctx)
        {
            var context = ctx as MummyContext;

            if (!_messageCache.TryGetValue(context.User.Id, out var queue)) return null;
            var found = queue.FirstOrDefault(x => x.TriggerID == context.Message.Id);
            if (found is null) return null;
            var retrievedMessage = await GetOrDownloadMessageAsync(context, found.ResponseId);
            return retrievedMessage as IUserMessage;
        }

        private static Task<IMessage> GetOrDownloadMessageAsync(ICommandContext ctx, ulong messageId)

        {
            var context = ctx as MummyContext;

            return context.Channel.GetMessageAsync(messageId);
        }
    }
}
