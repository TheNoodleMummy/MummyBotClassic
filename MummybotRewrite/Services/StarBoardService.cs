using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using System.Linq;
using System;

using Mummybot.Database.Models;
using Mummybot.Attributes;

namespace Mummybot.Services
{
    [Service("Starboard Service",typeof(StarBoardService))]
    public class StarBoardService
    {
        [Inject]
        private DiscordSocketClient _client;
        [Inject]
        private readonly DBService _dBService;
        [Inject]
        private readonly GuildService _guildService;
        [Inject]
        private readonly LogService Log;
        [Inject]
        private readonly IServiceProvider _provider;

        public const string one = "⭐";
        public const string three = "🌟";
        public const string five = "✨";
        public const string seven = "🎇";
        

        internal  async Task Client_ReactionRemoved(Cacheable<IUserMessage, ulong> messageParam, ISocketMessageChannel ichannel, SocketReaction reaction)
        {
            var guild = _dBService.GetGuildUncached((ichannel as SocketGuildChannel).Guild);
            if (!guild.UsesStarboard)
                return;
            SocketTextChannel starchannel;

            if (reaction.UserId == _client.CurrentUser.Id)
                return;
            if (guild.StarBoardChannelID == 0)
            {
                Log.LogWarning($"dropped star in {guild.GuildID}/{(ichannel as SocketGuildChannel).Guild.Name} => no starboard confirgured", Enums.LogSource.StarBoard);
                return;
            }
            else
            {
                starchannel = _client.GetChannel(guild.StarBoardChannelID) as SocketTextChannel;
            }

            try
            {
                var message = await messageParam.GetOrDownloadAsync();
                if (message == null)
                {
                     Log.LogWarning($"Dumped message (not in cache) with id {reaction.MessageId}",Enums.LogSource.StarBoard);
                    return;
                }
                if (!reaction.User.IsSpecified)
                {
                     Log.LogWarning($"Dumped message (invalid user) with id {message.Author.Id}", Enums.LogSource.StarBoard);
                    return;
                }

                


                if (reaction.Emote.Name == "⭐")
                {
                    if (message.Channel.Id == guild.StarBoardChannelID)
                        return;


                    EmbedBuilder emb = new EmbedBuilder();
                    emb.WithAuthor(x =>
                    {
                        x.IconUrl = message.Author.GetAvatarUrl();
                        x.Name = message.Author.Username;
                    });
                    emb.WithTitle($"[{message.GetJumpUrl()}](Jump)");

                    if (message.Content != "" || message.Content != null)
                        emb.WithDescription(message.Content);
                    if (message.Attachments.FirstOrDefault() != null)
                        emb.WithImageUrl(message.Attachments.FirstOrDefault().Url);

                    emb.WithTimestamp(message.CreatedAt);

                    Star fromlist = guild.Stars.FirstOrDefault(x => x.StaredMessageId == message.Id);

                    if (fromlist != null)
                        fromlist.Stars--;

                    if (fromlist != null)
                    {
                        Star sm = new Star()
                        {
                            StaredMessageId = reaction.MessageId,
                            Stars = fromlist.Stars,
                            StaredMessageChannelID = message.Channel.Id,
                            StartboardMessageId = fromlist.StartboardMessageId

                        };
                        string emote = one; ;

                        switch (sm.Stars)
                        {
                            case 0:
                            case 1:
                            case 2:
                                emote = one;
                                break;
                            case 3:
                            case 4:
                                emote = three;
                                break;
                            case 5:
                            case 6:
                                emote = five;
                                break;
                            default:
                                emote = seven;
                                break;
                        }
                        var channel = (message.Channel as ISocketMessageChannel);
                        var starboardmessage = await starchannel.GetMessageAsync(fromlist.StartboardMessageId);
                        await (starboardmessage as IUserMessage).ModifyAsync(x =>
                        {
                            x.Content = $"{emote}{sm.Stars} {(reaction.Channel as SocketTextChannel).Mention} ID:{sm.StaredMessageId}";
                            x.Embed = emb.Build();
                        });
                        if (sm.Stars == 0)
                        {
                            await (await (_client.GetChannel(guild.StarBoardChannelID) as SocketTextChannel).GetMessageAsync(sm.StartboardMessageId)).DeleteAsync();
                            var messagefromlist = guild.Stars.FirstOrDefault(x => x.StartboardMessageId == sm.StartboardMessageId);
                            guild.Stars.Remove(messagefromlist);
                        
                        }


                    }
                }
            }
            catch (Exception ex)
            {

                Log.LogError("starboard done a goof", Enums.LogSource.StarBoard, ex);
            }
            finally
            {
                await _guildService.UpdateGuildAsync(guild);
            }


        }

        internal async Task OnReactionAddedAsync(Cacheable<IUserMessage, ulong> messageParam, ISocketMessageChannel ichannel, SocketReaction reaction)
        {

            var guild = await _guildService.GetGuildAsync((ichannel as SocketGuildChannel).Guild);
            if (!guild.UsesStarboard)
                return;
            SocketTextChannel starchannel;

            if (reaction.UserId == _client.CurrentUser.Id)
                return;
            if (guild.StarBoardChannelID == 0)
                return;
            else
            {
                starchannel = _client.GetChannel(guild.StarBoardChannelID) as SocketTextChannel;
            }

            try
            {
                var message = await messageParam.GetOrDownloadAsync();
                if (message == null)
                {
                     Log.LogInformation($"Dumped message (not in cache) with id {reaction.MessageId}", Enums.LogSource.StarBoard);
                    return;
                }

                if (reaction.Emote.Name == "⭐")
                {
                    if (reaction.UserId == _client.CurrentUser.Id) return;

                    if (starchannel == null)
                    {
                         Log.LogInformation($"Dumped Reaction (NO starboard channel configured) in guild {(ichannel as SocketGuildChannel)?.Guild.Name}", Enums.LogSource.StarBoard);
                        return;
                    }

                    if (message.Channel.Id == starchannel.Id) return;

                    EmbedBuilder emb = new EmbedBuilder();
                    emb.WithAuthor(x =>
                    {
                        x.IconUrl = message.Author.GetAvatarUrl();
                        x.Name = message.Author.Username;
                    });
                    emb.WithTimestamp(message.CreatedAt);


                    if (message.Content != "" || message.Content != null)
                        emb.WithDescription(message.Content);
                    if (message.Attachments?.FirstOrDefault() != null)
                        emb.WithImageUrl(message.Attachments.FirstOrDefault().Url);
                    if (message.Embeds.FirstOrDefault()?.Url != null)
                        emb.WithImageUrl(message.Embeds.FirstOrDefault().Url);
                    //ZWS in first value dont remove
                    emb.AddField("​", $"[Message]({message.GetJumpUrl()})");


                    Star fromlist = guild.Stars.FirstOrDefault(x => x.StaredMessageId == message.Id);
                    string emote = one; ;
                    if (fromlist != null)
                        fromlist.Stars++;

                    if (fromlist != null)
                    {
                        Star sm = new Star()
                        {
                            StaredMessageId = reaction.MessageId,
                            Stars = fromlist.Stars,
                            StaredMessageChannelID = message.Channel.Id
                        };
                        switch (sm.Stars)
                        {
                            case 0:
                            case 1:
                            case 2:
                                emote = one;
                                break;
                            case 3:
                            case 4:
                                emote = three;
                                break;
                            case 5:
                            case 6:
                                emote = five;
                                break;
                            default:
                                emote = seven;
                                break;
                        }
                        var channel = (message.Channel as ISocketMessageChannel);
                        var starboardmessage = await starchannel.GetMessageAsync(fromlist.StartboardMessageId);
                        await (starboardmessage as IUserMessage).ModifyAsync(x =>
                        {
                            x.Content = $"{emote}{sm.Stars} {(reaction.Channel as SocketTextChannel).Mention} ID:{sm.StaredMessageId}";
                            x.Embed = emb.Build();
                        });


                    }
                    else if (fromlist == null)
                    {
                        Star sm = new Star()
                        {
                            StaredMessageId = reaction.MessageId,
                            Stars = 1,
                            StaredMessageChannelID = message.Channel.Id

                        };
                        switch (sm.Stars)
                        {
                            case 0:
                            case 1:
                            case 2:
                                emote = one;
                                break;
                            case 3:
                            case 4:
                                emote = three;
                                break;
                            case 5:
                            case 6:
                                emote = five;
                                break;
                            default:
                                emote = seven;
                                break;
                        }
                        var embed = await starchannel.SendMessageAsync($"{emote}{sm.Stars} {(reaction.Channel as SocketTextChannel).Mention} ID:{sm.StaredMessageId}", false, emb.Build());
                        sm.StartboardMessageId = embed.Id;
                        guild.Stars.Add(sm);
                    }
                }
               
            }
            catch (Exception ex)
            {

                 Log.LogInformation("oh boi starboard broke",Enums.LogSource.StarBoard,ex);
            }
            finally
            {
                await _guildService.UpdateGuildAsync(guild);
            }


        }
    }
}

////public class StaredMessage
////{
////    public ulong StaredMessageChannelID { get; set; }
////    public ulong StaredMessageId { get; set; }
////    public int Stars { get; set; }
////    public ulong StartboardMessageId { get; set; }
////}


////public static class StaredMessagesExtend
////{
////    public static List<StaredMessage> LoadStaredMessages()
////    {
////        List<StaredMessage> dict = new List<StaredMessage>();
////        dict = JsonConvert.DeserializeObject<List<StaredMessage>>(File.ReadAllText(Directory.GetCurrentDirectory() + @"/configs/Stars.json"));
////        if (dict == null)
////            dict = new List<StaredMessage>();
////        return dict;
////    }

////    public static void SaveStaredMessages(this List<StaredMessage> dict)
////    {

////        string tobesaved = JsonConvert.SerializeObject(dict, Formatting.Indented);
////        File.WriteAllText(Directory.GetCurrentDirectory() + @"/configs/Stars.json", tobesaved, Encoding.ASCII);
////    }
////}
