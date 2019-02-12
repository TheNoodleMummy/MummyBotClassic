
using Discord;
using Discord.WebSocket;
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
        public async Task Bullshit()
        {

            var seed = (int)(Context.Channel.Id - Context.Message.Id);


            await ReplyAsync($"i call {Random.Next(100)}% bullshit");
        }


        [Command("pin")]
        public async Task Pin()
        {
            var pins = await Context.Channel.GetPinnedMessagesAsync();
            var reply = pins.First();
            if (reply.Content == "" && reply.Attachments.Count() > 0)
            {
                await ReplyAsync(reply.Attachments.First().Url);
            }
            else
            {
                await ReplyAsync(reply.Content);
            }
        }

        //[Command("ohsnap")]
        //[Alias("osnap", "snap", "osnap")]
        //[Description("ohsnap.gif")]
        //public async Task OhSnap()
        //    => await Context.Channel.SendFileAsync(@"gifs/ohsnap.gif");


        //[Command("imdone")]
        //[Description("imdone.gif")]
        //public async Task Imdone()
        //    => await Context.Channel.SendFileAsync(@"gifs/imdone.gif");


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

        //[Command("mention", RunMode = RunMode.Async)]
        //[Description("mentions the mentioned user a set amount of times, you can give it a message to")]
        //public async Task Mention(IGuildUser user, int amount, [Remainder]string message = null)
        //{
        //    if (amount > 20)
        //    {
        //        await ReplyAsync("yo dont you think that's a bit much?");
        //        return;
        //    }
        //    for (int i = 0; i < amount; i++)
        //    {
        //        _MessagesService.AddTomentionlist(await ReplyAsync($"{user.Mention} {message}"));
        //        await Task.Delay(1000);

        //    }
        //    _ = Task.Delay(10000).ContinueWith(async x => await _MessagesService.DeleteMentions());
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
                            IconUrl = (message.Author as IGuildUser).GetAvatarUrl()
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
                            IconUrl = (message.Author as IGuildUser).GetAvatarUrl()
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
                //Log(LogSeverity.Critical, LogSource.TextCommands, ex.Message, true, ex);
            }
        }

        //[Command("triggered")]
        //[Description("triggered.gif")]
        //public async Task Triggered()
        //    => await Context.Channel.SendFileAsync(@"gifs/TRIGGERD.gif");


        [Command("wave")]
        [Description("makes me wave to everyone")]
        public async Task Wave()
        {

            int wait = 1050;
            var msg = await ReplyAsync("``` ¯\\_(ツ)_\\¯ ```");
            for (int i = 0; i < 3; i++)
            {

                await Task.Delay(wait);
                await msg.ModifyAsync(x => { x.Content = "``` ¯|_(ツ)_|¯ ```"; });
                await Task.Delay(wait);
                await msg.ModifyAsync(x => { x.Content = "``` ¯/_(ツ)_ /¯ ```"; });
                await Task.Delay(wait);
                await msg.ModifyAsync(x => { x.Content = "``` ¯|_(ツ)_|¯ ```"; });
                await Task.Delay(wait);
                await msg.ModifyAsync(x => { x.Content = "``` ¯\\_(ツ)_\\¯ ```"; });
            }

        }

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

        [Command("color", "colour")]
        [Description("sets the color of the mentioned role")]
        public async Task ColorRGB([Description("Red")]int r, [Description("Green")]int g, [Description("Blue")]int b, [Description("the role")]IRole Role = null)
        {
            if (Role == null)
            {
                Role = (Context.User as SocketGuildUser).Roles.FirstOrDefault(x => x.IsHoisted);
            }
            else if (!(Context.User as SocketGuildUser).Roles.Any(x => x.Permissions.Administrator))
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
        public async Task ColorHEX([Description("Hexadecimal Value")]string Hex, [Description("the role")]IRole Role = null)
        {
            if (Role == null)
            {
                Role = (Context.User as SocketGuildUser).Roles.FirstOrDefault(x => x.IsHoisted);
            }
            else if (!(Context.User as SocketGuildUser).Roles.Any(x => x.Permissions.Administrator))
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

        //[Command("yolo")]
        //[Description("yolo.png")]
        //public async Task Yolo()
        //    => await Context.Channel.SendFileAsync(@"pics\causeyolo.png");


        //[Command("deeznuts")]
        //[Description("dezznuts.png")]
        //public async Task Deeznuts()
        //    => await Context.Channel.SendFileAsync(@"pics\deeznuts.jpg");


        //[Command("run")]
        //[Description("run cats are taking over the world")]
        //public async Task Cats()
        //{
        //    string path = @"pics\runfromcat";
        //    path += Random.Next(1, 4);
        //    path += ".png";
        //    await Context.Channel.SendFileAsync(path);
        //}

        //[Command("botinfo")]
        //[Description("displays various info about mummy bot")]
        //public async Task Botstats()
        //{


        //    string subKey = @"SOFTWARE\Wow6432Node\Microsoft\Windows NT\CurrentVersion";
        //    RegistryKey key = Registry.LocalMachine;
        //    RegistryKey skey = key.OpenSubKey(subKey);

        //    var uptime = DateTime.UtcNow.ToLocalTime() - _thisbot.StartTime.ToLocalTime();
        //    var hour = uptime.Hours;
        //    var min = uptime.Minutes;
        //    var day = uptime.Days;

        //    EmbedBuilder emb = new EmbedBuilder();
        //    emb.AddField(x =>
        //    {
        //        x.Name = "BotName";
        //        x.Value = Context.Client.CurrentUser.Username;
        //    });
        //    emb.AddField(x =>
        //    {
        //        _thisbot.Refresh();
        //        x.Name = "Memory:";
        //        x.Value = _thisbot.WorkingSet64.Bytes().ToString("#");
        //    });
        //    emb.AddField(x =>
        //    {
        //        x.Name = "Uptime:";
        //        x.Value = $"{day} Days, {hour} Hours, {min} Mins";
        //    });
        //    emb.AddField(x =>
        //    {
        //        x.Name = "Opperating system";
        //        x.Value = skey.GetValue("ProductName");
        //    });
        //    emb.AddField(x =>
        //    {
        //        x.Name = $"Build with ";
        //        x.Value = $"Discord.net {DiscordConfig.Version}";

        //    });




        //    await SendEmbedAsync(emb);
        //}

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


                var userInfo = user ?? Context.Message.Author;
                SocketGuildUser guilduser;


                guilduser = userInfo as SocketGuildUser;


                IRole role = null;
                EmbedBuilder emb = new EmbedBuilder();

                EmbedAuthorBuilder embauth = new EmbedAuthorBuilder()
                {
                    Name = userInfo.Username,
                    IconUrl = userInfo.GetAvatarUrl()
                };
                role = guilduser.Roles.FirstOrDefault(x => x.IsHoisted);

                emb.Color = role?.Color;
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
                var invites = await Context.Guild.GetInvitesAsync();
                var used = invites.FirstOrDefault().Uses;
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
                Embed embedded = emb.Build();
                await ReplyAsync("", embed: embedded);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.StackTrace);
            }
        }

        //[Command("haters")]
        //[Description("hate me already?")]
        //public async Task Haters()
        //    => await Context.Channel.SendFileAsync(@"pics\hatersgonnehate.jpg");

        //[Command("lag")]
        //[Description("lag whats that?")]
        //public async Task Lag()
        //    => await Context.Channel.SendFileAsync(@"gifs\lag.gif");


    }
}
