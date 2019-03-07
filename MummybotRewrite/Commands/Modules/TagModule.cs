using Discord;
using Discord.WebSocket;
using Mummybot.Attributes;
using Mummybot.Database.Models;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{
    [Group("tag"), Name("Tag Commands"), Description("its some sort of dictionary with userinputed keys and values")]
    public class TagModule : MummyBase
    {
        [Command("all")]
        [Description("lists all tags for this guild(includes global tags")]
        public async Task Tags()
        {

            string response = "```\n";
            foreach (var p in GuildConfig.Tags)
            {
                if (p.GuildID == Context.Guild.Id || p.GuildID == 0)
                {
                    response += $"{p.Key}\n";
                }
            }
            response += "```";
            await ReplyAsync(response);

        }

        [Command("global"), Description("makes a tag non guild specific"), RequireUserPermission(GuildPermission.ManageChannels)]
        public async Task MakeTagGlobal([Remainder]string tag)
        {
            ulong botownerid = (await Context.Client.GetApplicationInfoAsync()).Owner.Id;

            if (Context.Message.Author.Id == botownerid)
            {
                GuildConfig.Tags.FirstOrDefault(x => x.Key == tag).GuildID = 0;
                await Context.Message.AddReactionAsync(new Emoji("✅"));
            }
            else
            {

                await ReplyAsync($"{ (await Context.Client.GetApplicationInfoAsync()).Owner.Mention} {Context.User.Mention} wants you to make {tag} global.");
            }
        }

        [Command("nsfw"), Description("makes a tag NSFW required")]
        public async Task MakeTagNSFW(string tag, bool Bool = true)
        {
            ulong botownerid = (await Context.Client.GetApplicationInfoAsync()).Owner.Id;

            try
            {
                if (Context.Message.Author.Id != botownerid)
                {
                    GuildConfig.Tags.FirstOrDefault(x => x.Key == tag).Req_NSFW = true;
                    await Context.Message.AddReactionAsync(new Emoji("✅"));
                }

            }
            catch (KeyNotFoundException)
            {

                await ReplyAsync(":no_entry: Key not found! :no_entry: ");
            }
            if (Context.Message.Author.Id == botownerid)
            {
                try
                {
                    GuildConfig.Tags.FirstOrDefault(x => x.Key == tag).Req_NSFW = Bool;
                    await Context.Message.AddReactionAsync(new Emoji("✅"));
                }
                catch (KeyNotFoundException)
                {

                    await ReplyAsync(":no_entry: Key not found! :no_entry: ");
                }
            }
            else if (GuildConfig.Tags.FirstOrDefault(x => x.Key == tag).Req_NSFW)
            {
                await ReplyAsync($"The tag is already NSFW only {(await Context.Client.GetApplicationInfoAsync()).Owner.Mention} can undo this");
            }
        }


        [Command("add")]
        [Description("add a new tag")]
        public async Task Tagadd(string tag, [Remainder]string key)
        {
            try
            {
                GuildConfig.Tags.Add(new Tag(Context.User.Id, Context.User.Username, key, tag, Context.Guild.Id));
                await Context.Message.AddReactionAsync(new Emoji("✅"));
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException boop)
                {
                    await ReplyAsync("a tag already exist with this key");

                }
                Logs.LogError("it broke", Enums.LogSource.Tags, ex);
            }

        }

        [Command("remove")]
        [Description("remove a tag only the tag owner can remove he's own tag (admins can to)"),]
        public async Task Tagremove([Remainder]string tag)
        {

            if (Context.User.Id == GuildConfig.Tags.FirstOrDefault(x => x.Key == tag).UserID || (Context.User as IGuildUser).GuildPermissions.ManageGuild)
            {
                GuildConfig.Tags.Remove(GuildConfig.Tags.FirstOrDefault(x => x.Key == tag));
                await Context.Message.AddReactionAsync(new Emoji("✅"));
            }
            else
            {
                await ReplyAsync(":no_entry: you dont own this tag or have insufficient admin rights to do this :no_entry:");
                await Context.Message.AddReactionAsync(new Emoji("⛔"));
            }
        }

        [Command("editkey")]
        [Description("edit the key of a tag")]
        public async Task Tageditkey(string tag, string newkey)
        {
            if (Context.User.Id == GuildConfig.Tags.FirstOrDefault(x => x.Key == tag).UserID || (Context.User as IGuildUser).GuildPermissions.ManageGuild)
            {
                try
                {
                    Tag value = GuildConfig.Tags.FirstOrDefault(x => x.Key == tag);
                    GuildConfig.Tags.Remove(GuildConfig.Tags.FirstOrDefault(x => x.Key == tag));
                    value.Key = newkey;
                    GuildConfig.Tags.Add(value);
                    await Context.Message.AddReactionAsync(new Emoji("✅"));
                }
                catch (KeyNotFoundException)
                {

                    await ReplyAsync(":no_entry: Tag not found! :no_entry: ");
                }

            }
            else
            {
                await Context.Message.AddReactionAsync(new Emoji("⛔"));
                await ReplyAsync(":no_entry: you dont own this tag or have insufficient admin rights to do this :no_entry:");
            }

        }

        [Command("editvalue")]
        [Description("edit the value of a tag")]
        public async Task Tageditvalue(string tag, string editvalue)
        {
            if (Context.User.Id == GuildConfig.Tags.FirstOrDefault(x => x.Key == tag).UserID || (Context.User as IGuildUser).GuildPermissions.ManageGuild)
            {
                try
                {
                    GuildConfig.Tags.FirstOrDefault(x => x.Key == tag).Value = editvalue;
                    await Context.Message.AddReactionAsync(new Emoji("✅"));
                }
                catch (KeyNotFoundException)
                {

                    await ReplyAsync(":no_entry: Key not found! :no_entry: ");
                }

            }
            else
            {
                await ReplyAsync(":no_entry: you dont own this tag or have insufficient admin rights to do this :no_entry:");
            }
        }


        [Command("reset")]
        [RunMode(RunMode.Parallel)]
        [Description("Reset all tags"), RequireOwner]
        public async Task Tagreset()
        {


            GuildConfig.Tags = new List<Tag>();
            await Context.Message.AddReactionAsync(new Emoji("✅"));
        }

        [Command, Description("call a tag"), Priority(-1)]
        public async Task Tag([Remainder]string tag)
        {
            try
            {
                Tag calledtag = GuildConfig.Tags.FirstOrDefault(x => x.Key == tag);

                if (Context.Guild.Id == calledtag.GuildID || calledtag.GuildID == 0)//==0 means global tag
                {
                    if ((Context.Channel as SocketTextChannel).IsNsfw && calledtag.Req_NSFW)
                    {
                        await ReplyAsync(calledtag.Value);
                        calledtag.Uses++;
                    }
                    else if (calledtag.Req_NSFW && !(Context.Channel as SocketTextChannel).IsNsfw)
                    {
                        await ReplyAsync("this tag contains NSFW/18+ and can not be displayed in this channel");
                    }
                    else if (!calledtag.Req_NSFW)
                    {
                        await ReplyAsync(calledtag.Value);
                        calledtag.Uses++;
                    }
                }
                else
                {
                    await ReplyAsync(":no_entry: Key not found! :no_entry: ");
                }

            }
            catch (KeyNotFoundException)
            {
                await ReplyAsync(":no_entry: Key not found! :no_entry: ");
            }
        }

        [Command("info"), Description("info about tag")]
        public async Task Taginfo([Remainder]string tag)
        {
            EmbedBuilder emb = new EmbedBuilder();
            Tag tagy = null;
            try
            {
                tagy = GuildConfig.Tags.FirstOrDefault(x => x.Key == tag);

            }
            catch (Exception)
            {
                await ReplyAsync(":no_entry: Key not found! :no_entry: ");
            }
            IUser user = Context.Client.GetUser(tagy.UserID);
            emb.WithAuthor(x =>
            {
                x.Name = user.Username;
                x.IconUrl = user.GetAvatarUrl();
            });
            if (tagy.Req_NSFW && !(Context.Channel as SocketTextChannel).IsNsfw)
            {
                emb.AddField(x =>
                {
                    x.Name = "this channel is not NSFW tagged";
                    x.Value = "This tag contains NSFW content and can not be displayed";
                });
            }
            else if (tagy.Req_NSFW && (Context.Channel as SocketTextChannel).IsNsfw)
            {
                emb.AddField(x =>
                           {
                               x.Name = tagy.Key;
                               x.Value = tagy.Value;
                           });
            }
            else if (!tagy.Req_NSFW)
            {
                emb.AddField(x =>
                {
                    x.Name = tagy.Key;
                    x.Value = tagy.Value;
                });
            }

            emb.AddField(x =>
            {
                x.Name = "uses";
                x.Value = tagy.Uses.ToString(); ;
            });
            emb.AddField(x =>
            {
                x.Name = "Created At";
                x.Value = tagy.CreatedAt;
            });
            emb.AddField(x =>
            {
                x.Name = "Require NSFW";
                x.Value = tagy.Req_NSFW ? " Yes" : " No";
            });
            await ReplyAsync(embed: emb.Build());
        }
    }

}