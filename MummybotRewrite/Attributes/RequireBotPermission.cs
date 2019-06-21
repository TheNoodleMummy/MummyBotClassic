using Discord;
using Mummybot.Commands;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Attributes
{
    public class RequireBotPermissionAttribute : CheckBaseAttribute
    {
        public GuildPermission? GuildPermission { get; }
        public ChannelPermission? ChannelPermission { get; }

        
        public RequireBotPermissionAttribute(GuildPermission permission)
        {
            GuildPermission = permission;
            ChannelPermission = null;
        }        

        public RequireBotPermissionAttribute(ChannelPermission permission)
        {
            ChannelPermission = permission;
            GuildPermission = null;
        }

        public override async Task<CheckResult> CheckAsync(ICommandContext ctx, IServiceProvider provider)
        {
            var context = ctx as MummyContext;
            var guildUser = context.Guild.CurrentUser;

            if (GuildPermission.HasValue)
            {
                if (guildUser == null)
                    return await Task.FromResult(new CheckResult("Command must be used in a guild channel"));
                if (!guildUser.GuildPermissions.Has(GuildPermission.Value))
                    return await Task.FromResult(new CheckResult($"Command requires guild permission {GuildPermission.Value}"));
            }

            if (ChannelPermission.HasValue)
            {
                var guildChannel = context.Channel as IGuildChannel;

                ChannelPermissions perms;
                if (guildChannel != null)
                    perms = guildUser.GetPermissions(guildChannel);
                else
                    perms = ChannelPermissions.All(guildChannel);

                if (!perms.Has(ChannelPermission.Value))
                    return await Task.FromResult(new CheckResult($"Command requires channel permission {ChannelPermission.Value}"));
            }

            return await Task.FromResult(CheckResult.Successful);
        }
    }
}
