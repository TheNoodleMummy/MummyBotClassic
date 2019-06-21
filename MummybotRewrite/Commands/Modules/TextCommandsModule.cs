
using Discord;
using Discord.WebSocket;
using Mummybot.Attributes;
using Mummybot.Enums;
//using Humanizer;
using Mummybot.overrrides_extentions;
using Qmmands;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{
    [Name("Text Chat  Commands"), Description("Commands for text based channels")]
    public class TextCommands : MummyBase
    {
        public Process _thisbot { get; set; }
        //public EmojifyerService _emojiService { get; set; }
        //public SendedMessagesService _MessagesService { get; set; }
        public Random Random { get; set; }

        [Command("bs", "bullshit")]
        public Task Bullshit() => ReplyAsync($"i call {Random.Next(100)}% bullshit");

        //[Command("ohsnap")]
        //[Alias("osnap", "snap", "osnap")]
        //[Description("ohsnap.gif")]
        //public async Task OhSnap()
        //    => await Context.Channel.SendFileAsync(@"gifs/ohsnap.gif");


        //[Command("triggered")]
        //[Description("triggered.gif")]
        //public async Task Triggered()
        //    => await Context.Channel.SendFileAsync(@"gifs/TRIGGERD.gif");

        //[Command("imdone")]
        //[Description("imdone.gif")]
        //public async Task Imdone()
        //    => await Context.Channel.SendFileAsync(@"gifs/imdone.gif");

        //[Command("yolo")]
        //[Description("yolo.png")]
        //public async Task Yolo()
        //    => await Context.Channel.SendFileAsync(@"pics\causeyolo.png");


        //[Command("deeznuts")]
        //[Description("dezznuts.png")]
        //public async Task Deeznuts()
        //    => await Context.Channel.SendFileAsync(@"pics\deeznuts.jpg");

        //[Command("boom")]
        //[Description("boom.gif")]
        //[Alias("kaboom")]
        //public async Task Boom()
        //        => await Context.Channel.SendFileAsync(@"gifs/boom.gif");

        //[Command("fuckyou")]
        //[Description("fuckyou.gif")]
        //[Alias("fu")]
        //public async Task Fuckyou()
        //    => await Context.Channel.SendFileAsync(@"gifs/fuckyou.gif");

        //[Command("run")]
        //[Description("run cats are taking over the world")]
        //public async Task Cats()
        //{
        //    string path = @"pics\runfromcat";
        //    path += Random.Next(1, 4);
        //    path += ".png";
        //    await Context.Channel.SendFileAsync(path);
        //}

        //[Command("haters")]
        //[Description("hate me already?")]
        //public async Task Haters()
        //    => await Context.Channel.SendFileAsync(@"pics\hatersgonnehate.jpg");

        //[Command("lag")]
        //[Description("lag whats that?")]
        //public async Task Lag()
        //    => await Context.Channel.SendFileAsync(@"gifs\lag.gif");


        //[Command("toggles")]
        //[Description("list's all toggle fucntions and there value's")]
        //public async Task Toggles()
        //{
        //    PropertyInfo[] toggles = _toggle.GetType().GetProperties();
        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendLine("```");
        //    foreach (PropertyDescriptor context in TypeDescriptor.GetProperties(_toggle))
        //    {
        //        sb.AppendLine($"{context.Name.ToString()}: {context.GetValue(_toggle)}");
        //    }
        //    sb.AppendLine("```");

        //    await ReplyAsync(sb.ToString());

        //}

        //[Command("emojify")]
        //[Description("Gives the given string parameter back in emoji")]
        //public async Task Emojify([Remainder]string param)
        //{
        //    await Context.Message.DeleteAsync();
        //    string parm = param;
        //    var messages = _emojiService.GetEmojiString(parm);

        //    foreach (string message in messages)
        //    {
        //        await ReplyAsync(message);
        //        await Task.Delay(1000);
        //    }
        //}





        [Command("quote")]
        [Description("quote a message by ID")]
        public async Task Quote(ulong id)
        {
            try
            {
                IMessage message = await Context.Channel.GetMessageAsync(id);
                if (message == null)
                {
                    await ReplyAsync("Could not find this message in this channel");
                }
                else
                {
                    if (message.Content == "")
                    {
                        EmbedBuilder emb = new EmbedBuilder();
                        EmbedAuthorBuilder embauth = new EmbedAuthorBuilder()
                        {
                            Name = message.Author.Username,
                            IconUrl = message.Author.GetAvatarUrl()
                        };
                        ;
                        emb.WithAuthor(embauth);
                        emb.WithImageUrl(message.Attachments.First().ToString());
                        await ReplyAsync("", embed: emb.Build());
                    }
                    else
                    {
                        EmbedBuilder emb = new EmbedBuilder();
                        EmbedAuthorBuilder embauth = new EmbedAuthorBuilder()
                        {
                            Name = message.Author.Username,
                            IconUrl = message.Author.GetAvatarUrl()
                        };
                        ;
                        emb.WithAuthor(embauth);
                        emb.AddField(x =>
                        {
                            x.IsInline = false;
                            x.Name = message.Author.GetName();
                            x.Value = message.Content;
                        });
                        emb.WithTimestamp(message.CreatedAt);
                        await ReplyAsync("", embed: emb.Build());
                    }



                }
            }
            catch (Exception ex)
            {
                await ReplyAsync($"im an idiot and failed to quote that.(Message ID {id}");
                Logs.LogError("I failed to quote a message with a exception", LogSource.Commands, ex);
            }
        }


        [Command("color", "colour")]
        [Description("sets the color of the mentioned role")]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task ColorRGB([Description("Red")]int r, [Description("Green")]int g, [Description("Blue")]int b, [Description("the role")]IRole Role = null)
        {
            if (Role == null)
            {
                Role = (Context.User as SocketGuildUser).Roles.FirstOrDefault(x => x.IsHoisted);
            }
            else if (!(Context.User as SocketGuildUser).Roles.Any(x => x.Permissions.ManageRoles))
            {
                Role = (Context.User as SocketGuildUser).Roles.FirstOrDefault(x => x.IsHoisted);
            }

            if (Role == null)
            {
                await ReplyAsync("No Role was found to edit a color of :(");
                return;
            }
            else
            {
                if (r > 256)
                {
                    await Messages.SendMessageAsync(Context, $"The value of R ({r}) was invalid");
                    return;
                }
                if (g > 256)
                {
                    await Messages.SendMessageAsync(Context, $"The value of G ({g}) was invalid");
                    return;
                }
                if (b > 256)
                {
                    await Messages.SendMessageAsync(Context, $"The value of B ({b}) was invalid");
                    return;
                }

                var color = new Color(r, g, b);

                await Role.ModifyAsync(R => R.Color = color);
                var emb = new EmbedBuilder();
                emb.WithColor(color);
                emb.WithDescription($"Changed color of {Role.Mention}");
                await Messages.SendMessageAsync(Context, embed: emb.Build());
            }

        }
        [Command("color", "colour")]
        [Description("sets the color of the mentioned role")]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        public async Task ColorHEX([Description("Hexadecimal Value")]string Hex, [Description("the role")]IRole Role = null)
        {
            if (Role == null)
            {
                Role = (Context.User as SocketGuildUser).Roles.FirstOrDefault(x => x.IsHoisted);
            }
            else if (!(Context.User as SocketGuildUser).Roles.Any(x => x.Permissions.ManageRoles))
            {
                Role = (Context.User as SocketGuildUser).Roles.FirstOrDefault(x => x.IsHoisted);
            }

            if (Role == null)
            {
                await ReplyAsync("No Role was found to edit a color of :(");
                return;
            }
            else
            {


                if (Hex.StartsWith("#"))
                    Hex = Hex.Substring(1);

                if (Hex.Length != 6) throw new Exception("Color not valid");

                var color = new Color(
                    int.Parse(Hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
                    int.Parse(Hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
                    int.Parse(Hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber));



                await Role.ModifyAsync(R => R.Color = color);
                var emb = new EmbedBuilder();
                emb.WithColor(color);
                emb.WithDescription($"Changed color of {Role.Mention}");
                await Messages.SendMessageAsync(Context, embed: emb.Build());
            }
        }



        [Command("botinfo")]
        [Description("displays various info about mummy bot")]
        public async Task Botstats()
        {
            _thisbot.Refresh();

            var uptime = DateTime.Now - _thisbot.StartTime;
            var hour = uptime.Hours;
            var min = uptime.Minutes;
            var day = uptime.Days;

            EmbedBuilder emb = new EmbedBuilder();
            emb.AddField(x =>
            {
                x.Name = "BotName";
                x.Value = Context.Client.CurrentUser.Username;
            });
            emb.AddField(x =>
            {
                x.Name = "Memory:";
                x.Value = GC.GetTotalMemory(false);
            });
            emb.AddField(x =>
            {
                x.Name = "Uptime:";
                x.Value = $"{day} Days, {hour} Hours, {min} Mins";
            });
            emb.AddField(x =>
            {
                x.Name = "Opperating system";
                x.Value = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            });
            emb.AddField(x =>
            {
                x.Name = $"Build with ";
                x.Value = $"Discord.net {DiscordConfig.Version}";

            });
            await ReplyAsync(embed: emb.Build());
        }

        [Command("unix")]
        [Description("displays the current time in unix format")]
        public async Task Unix()
        {
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            await ReplyAsync("UNIX: " + unixTimestamp.ToString("##,##,##,##,##"));//+"\n"+"ticks: "+ticksTimestamp
        }


        [Command("info")]
        [Description("gets the info of a mention user or yourself")]
        public async Task Info(IUser user = null)
        {
            try
            {
                var userInfo = user ?? Context.User;
                var guilduser = userInfo as SocketGuildUser;
                var role = guilduser.Roles.FirstOrDefault(x => x.IsHoisted);
                var emb = new EmbedBuilder();

                EmbedAuthorBuilder embauth = new EmbedAuthorBuilder()
                {
                    Name = userInfo.Username,
                    IconUrl = userInfo.GetAvatarUrl()
                };

                emb.Color = role?.Color ?? Color.Blue;
                emb.WithAuthor(embauth);
                emb.AddField(x =>
                {
                    x.Name = "Name";
                    x.Value = $"{userInfo.Username}#{userInfo.Discriminator}";
                    x.IsInline = true;
                });

                if (!string.IsNullOrWhiteSpace(guilduser.Nickname))
                {
                    emb.AddField(x =>
                    {
                        x.Name = "Nickname";
                        x.Value = $"{guilduser.Nickname}";
                        x.IsInline = true;
                    });
                }

                emb.AddField(x =>
                {
                    x.Name = "ID";
                    x.Value = $"{userInfo.Id}";
                    x.IsInline = true;
                });

                if ((user.Activity != null))
                {
                    emb.AddField(x =>
                    {
                        x.Name = "Game";
                        x.Value = $"{userInfo.Activity.Name}";
                        x.IsInline = true;
                    });
                }

                if (guilduser.JoinedAt != null)
                {
                    emb.AddField(x =>
                    {
                        x.Name = "Jonied at";
                        x.Value = $"{guilduser.JoinedAt.Value.DateTime.ToUniversalTime()}UTC";
                        x.IsInline = true;
                    });
                }

                emb.AddField(x =>
                {
                    x.Name = "Roles";
                    x.IsInline = true;
                    x.Value = $"{guilduser.Roles.Count() - 1} - {string.Join(", ", guilduser.Roles.Where(r => r.Id != Context.Guild.EveryoneRole.Id))}";
                });

                emb.AddField(x =>
                {
                    x.Name = "Avatar url:";
                    x.Value = userInfo.GetAvatarUrl();
                });
                emb.WithImageUrl(userInfo.GetAvatarUrl());
                await ReplyAsync("", embed: emb.Build());
            }
            catch (Exception ex)
            {

                Logs.LogError("Failed to get the info of a user", LogSource.Commands, ex);
            }
        }


        [Command("checkid")]
        public async Task CheckIDAsync(ulong id = 0)
        {
            var datetime = DateTimeOffset.FromUnixTimeMilliseconds((long)(id >> 22) + 1420070400000);
            var workerid = (id & 0x3E0000) >> 17;
            var processid = (id & 0x1F000) >> 12;
            var increment = id & 0xFFF;
            var emb = new EmbedBuilder()
                .AddField("created", datetime, true)
                .AddField("Worker ID", workerid, true)
                .AddField("process ID", processid, true)
                .AddField("increment", increment, true);

            await ReplyAsync(embed: emb.Build());
        }
    }
}
