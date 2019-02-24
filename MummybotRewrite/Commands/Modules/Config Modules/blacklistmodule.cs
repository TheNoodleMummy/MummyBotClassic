using Discord;
using Mummybot.Attributes;
using Mummybot.Database.Models;
using Mummybot.Enums;
using Qmmands;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{
    [Group("blacklist"), RequireActiveBlackList, RequireUserPermission(ChannelPermission.ManageChannels)]
    public class Blacklisting : MummyBase
    {
        [Command("add"), Description("blacklist a user from using any bot command"),]
        public async Task Add([Description("UserToBlacklist")]IUser user, [Remainder]string reason = null)
        {

            if (GuildConfig.BlackList.Any(x => x.UserID == user.Id))
            {
                await Messages.SendMessageAsync(Context, "This user is already blacklisted");
            }
            else
            {

                GuildConfig.BlackList.Add(new BlackList(user.Id, reason));
                Logs.LogInformation($"{Context.User}/{Context.User.Id} added {user} in {Context.Guild}/{Context.Guild.Id} ", LogSource.BlackList);
                await Context.Message.AddReactionAsync(new Emoji("✅"));
            }
        }

        [Command("remove"), Description("remove a user from the blacklist"),]
        public async Task Remove(IUser user)
        {
            if (GuildConfig.BlackList.Any(x => x.UserID == user.Id))
            {
                var entry = GuildConfig.BlackList.FirstOrDefault(bl => bl.UserID == user.Id);
                GuildConfig.BlackList.Remove(entry);
                Logs.LogInformation($"{Context.User}/{Context.User.Id} removed {user} in {Context.Guild}/{Context.Guild.Id} ", LogSource.BlackList);
                await Context.Message.AddReactionAsync(new Emoji("✅"));
            }
            else
            {
                await Messages.SendMessageAsync(Context, "this user is not blacklisted");

            }
        }

        [Command, Description("list all users that have been blacklist and why")]
        public async Task Listblacklists()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var blacklistentry in GuildConfig.BlackList)
                sb.AppendLine($"{Context.Client.GetUser(blacklistentry.UserID).Username} - {blacklistentry.DateBlocked} - {blacklistentry.Reason}");

            await ReplyAsync(sb.ToString());
        }
    }
}
