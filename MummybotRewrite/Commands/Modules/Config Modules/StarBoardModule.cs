using Discord;
using Discord.Addons.Interactive;
using Mummybot.Attributes;
using Mummybot.overrrides_extentions;
using Qmmands;
using System.Linq;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{
    public class StarBoardModule : MummyBase
    {
        public InteractiveService InteractiveService { get; set; }

        [Command("starboard")]
        [RunMode(RunMode.Parallel)]
        public async Task EnableStarboard()
        {
            if (GuildConfig.UsesStarboard)
            {
                var msg = await ReplyAsync("Are you sure you wane to disable the starboard?");
                var response = await InteractiveService.NextMessageAsync(Context, true, true);
                if (response?.Content.ToLower() == "yes")
                {
                    GuildConfig.UsesStarboard = false;
                }
                await Messages.DeleteMessageAsync(Context, msg);
                await response.DeleteAsync();
            }
            else
            {
                var msg = await ReplyAsync("Are you sure you wane to enable the starboard?");
                var response = await InteractiveService.NextMessageAsync(Context, true, true);
                if (response?.Content.ToLower() == "yes")
                {
                    GuildConfig.UsesStarboard = true;
                }
                await Messages.DeleteMessageAsync(Context, msg);
                await response.DeleteAsync();
            }
        }


        [Command("starshow"), RequireActiveStarboard]
        public async Task Showstar(ulong id)
        {
            var star = GuildConfig.Stars.FirstOrDefault(x => x.StaredMessageId == id);

            var emb = new EmbedBuilder();
            var starboardmessage = await Context.Guild.GetTextChannel(star.StaredMessageChannelID).GetMessageAsync(star.StaredMessageId);

            emb.WithAuthor(starboardmessage.Author.GetName(), starboardmessage.Author.GetAvatarUrl());
            emb.WithDescription(starboardmessage.Content);
            emb.AddField("Stars", star.Stars);
            await Messages.SendMessageAsync(Context, "", embed: emb.Build());
        }

        [Command("starboardchannel"), RequireActiveStarboard]
        public async Task Setchannelid(ulong channel)
        {
            GuildConfig.StarBoardChannelID = channel;
            await Context.Message.AddReactionAsync(new Emoji("✅"));
        }



    }
}
