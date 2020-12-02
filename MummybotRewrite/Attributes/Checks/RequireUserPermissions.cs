using Discord;
using Discord.WebSocket;
using Mummybot.Commands;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PermissionTarget = Mummybot.Enums.PermissionTarget;

namespace Mummybot.Attributes.Checks
{
    [Name("Require Permissions")]

    public class RequirePermissions : MummyCheckBase
    {
        private readonly PermissionTarget _target;
        private readonly GuildPermission[] _guildPerms;
        private readonly ChannelPermission[] _channelPerms;


        public RequirePermissions(PermissionTarget target, params GuildPermission[] guildPerms)
        {
            _target = target;
            _guildPerms = guildPerms;
            _channelPerms = new ChannelPermission[0];
        }

        public RequirePermissions(PermissionTarget target, params ChannelPermission[] channelPerms)
        {
            _target = target;
            _channelPerms = channelPerms;
            _guildPerms = new GuildPermission[0];
        }

        public override ValueTask<CheckResult> CheckAsync(MummyContext context)
        {
            SocketGuildUser user = null;

            switch (_target)
            {
                case PermissionTarget.User:
                    user = context.User;
                    break;

                case PermissionTarget.Bot:
                    user = context.Guild.CurrentUser;
                    break;
            }

            var failedGuildPerms = _guildPerms.Where(guildPerm => !user.GuildPermissions.Has(guildPerm)).ToArray();

            var channelPerms = context.User.GetPermissions(context.Channel);

            var failedChannelPerms = _channelPerms.Where(channelPerm => !channelPerms.Has(channelPerm)).ToArray();

            if (failedGuildPerms.Length == 0 && failedChannelPerms.Length == 0)
                return CheckResult.Successful;

            var sb = new StringBuilder();

            foreach (var guildPerm in failedGuildPerms)
                sb.AppendLine(guildPerm.ToString());

            foreach (var channelPerm in failedChannelPerms)
                sb.AppendLine(channelPerm.ToString());

            var u = context.User;

            var target = _target == PermissionTarget.User ? "You" : "I";

            return CheckResult.Failed($"{target} lack permissions for {sb}");
        }
    }
}
