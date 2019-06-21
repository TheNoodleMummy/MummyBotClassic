using Discord;
using Mummybot.Commands;
using Qmmands;
using System;
using System.Threading.Tasks;

namespace Mummybot.Attributes
{
    class RequireUserPermissionAttribute : CheckBaseAttribute
    {
        public GuildPermission? GuildPermission { get; }
        public ChannelPermission? ChannelPermission { get; }


        public RequireUserPermissionAttribute(GuildPermission permission)
        {
            GuildPermission = permission;
            ChannelPermission = null;
        }

        public RequireUserPermissionAttribute(ChannelPermission permission)
        {
            ChannelPermission = permission;
            GuildPermission = null;
        }

        public override Task<CheckResult> CheckAsync(ICommandContext ctx, IServiceProvider provider)
        {
            var context = ctx as MummyContext;
            var guildUser = context.User as IGuildUser;

            if (GuildPermission.HasValue)
            {
                if (guildUser == null)
                    return Task.FromResult(new CheckResult(("Command must be used in a guild channel")));
                if (!guildUser.GuildPermissions.Has(GuildPermission.Value))
                    return Task.FromResult(new CheckResult(($"Command requires guild permission {GuildPermission.Value}")));
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
                    return Task.FromResult(new CheckResult($"Command requires channel permission {ChannelPermission.Value}"));
            }

            return Task.FromResult(CheckResult.Successful);
        }
    }
}
