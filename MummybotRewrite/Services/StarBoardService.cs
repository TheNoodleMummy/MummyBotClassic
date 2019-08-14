using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Mummybot.Database;
using Mummybot.Database.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mummybot.Services
{
    public class StarBoardService : BaseService
    {
        private readonly DiscordSocketClient _client;
        private readonly SnowFlakeGeneratorService _snowFlakeGenerator;

        public StarBoardService(DiscordSocketClient client,SnowFlakeGeneratorService snowFlakeGenerator,LogService logService)
        {
            _client = client;
            _snowFlakeGenerator = snowFlakeGenerator;
            LogService = logService;
        }

        

        public override Task InitialiseAsync(IServiceProvider services)
        {
            _client.ReactionAdded += async (chachemessage, channel, reaction) =>
            {
                if (channel is SocketTextChannel Textchannel)
                {
                    using var guildstore = services.GetService<GuildStore>();
                    var guild = await guildstore.GetOrCreateGuildAsync(Textchannel.Guild, g => g.Stars);
                    if (guild.UsesStarBoard && guild.StarboardEmote == reaction.Emote.Name)
                    {
                        var msg = await chachemessage.GetOrDownloadAsync();
                        var star = guild.Stars.Find(x => x.MessageId == chachemessage.Id);
                        if (star is null)
                        {
                            star = new Star
                            {
                                Id = _snowFlakeGenerator.NextLong(),
                                MessageId = chachemessage.Id,
                                Stars = 1
                            };

                            var emb = new EmbedBuilder();
                            var starboard = _client.GetGuild(guild.GuildID).GetTextChannel(guild.StarboardChannelId);
                            emb.WithAuthor(msg.Author);
                            emb.AddField("\u200b", msg.Content);

                            emb.AddField("\u200b", $"in {Textchannel.Guild.Name} / <#{msg.Channel.Id}> /" +
                                    $"{Format.Url($"{msg.Id}", msg.GetJumpUrl())}");

                            var starboardmsg = await starboard.SendMessageAsync($"{star.Stars} {guild.StarboardEmote}; in {(msg.Channel as ITextChannel).Mention};", embed: emb.Build());

                            star.StarboardMessageId = starboardmsg.Id;
                            guild.Stars.Add(star);
                            await guildstore.SaveChangesAsync();
                        }
                        else
                        {
                            star.Stars++;
                            var emb = new EmbedBuilder();
                            var starboard = _client.GetGuild(guild.GuildID).GetTextChannel(guild.StarboardChannelId);
                            emb.WithAuthor(msg.Author);
                            emb.AddField("\u200b", msg.Content);

                            emb.AddField("\u200b", $"in {Textchannel.Guild.Name} / <#{msg.Channel.Id}> /" +
                                    $"{Format.Url($"Message", msg.GetJumpUrl())}");
                            var staboardmsg = (await Textchannel.Guild.GetTextChannel(guild.StarboardChannelId).GetMessageAsync(star.StarboardMessageId) as IUserMessage);

                            await staboardmsg.ModifyAsync(x => { x.Content = $"{star.Stars} {guild.StarboardEmote}; in {(msg.Channel as ITextChannel).Mention};"; x.Embed = emb.Build(); });
                            guildstore.Update(guild);
                            await guildstore.SaveChangesAsync();
                        }
                    }
                }
            };
            _client.ReactionRemoved += async (chachemessage, channel, reaction) =>
            {
                if (channel is SocketTextChannel Textchannel)
                {
                    using var guildstore = services.GetService<GuildStore>();
                    var guild = await guildstore.GetOrCreateGuildAsync(Textchannel.Guild, g => g.Stars);
                    if (guild.UsesStarBoard && guild.StarboardEmote == reaction.Emote.Name)
                    {
                        var msg = await chachemessage.GetOrDownloadAsync();
                        var star = guild.Stars.Find(x => x.MessageId == chachemessage.Id);
                        if (star is null)
                            return;
                        else
                        {
                            star.Stars--;
                            var starboard = _client.GetGuild(guild.GuildID).GetTextChannel(guild.StarboardChannelId);
                            if (star.Stars == 0)
                            {
                                await starboard.DeleteMessageAsync(star.StarboardMessageId);
                                guild.Stars.Remove(star);
                                guildstore.Update(guild);
                                await guildstore.SaveChangesAsync();
                                return;
                            }
                            var emb = new EmbedBuilder();

                            emb.WithAuthor(msg.Author);
                            emb.AddField("\u200b", msg.Content);

                            emb.AddField("\u200b", $"in {Textchannel.Guild.Name} / <#{msg.Channel.Id}> /" +
                                    $"{Format.Url("Message", msg.GetJumpUrl())}");
                            var staboardmsg = (await Textchannel.Guild.GetTextChannel(guild.StarboardChannelId).GetMessageAsync(star.StarboardMessageId) as IUserMessage);

                            await staboardmsg.ModifyAsync(x => { x.Content = $"{star.Stars} {guild.StarboardEmote}; in {(msg.Channel as ITextChannel).Mention};"; x.Embed = emb.Build(); });
                            guildstore.Update(guild);
                            await guildstore.SaveChangesAsync();
                        }
                    }
                }
            };
            return Task.CompletedTask;
        }
    }
};