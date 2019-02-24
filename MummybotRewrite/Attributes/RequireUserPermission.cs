using Discord;
using Mummybot.Commands;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Attributes
{
    class RequireUserPermissionAttribute : CheckBaseAttribute
    {
        public GuildPermission? GuildPermission { get; }
        public ChannelPermission? ChannelPermission { get; }

        /// <summary>
        /// Require that the user invoking the command has a specified GuildPermission
        /// </summary>
        /// <remarks>This precondition will always fail if the command is being invoked in a private channel.</remarks>
        /// <param name="permission">The GuildPermission that the user must have. Multiple permissions can be specified by ORing the permissions together.</param>
        public RequireUserPermissionAttribute(GuildPermission permission)
        {
            GuildPermission = permission;
            ChannelPermission = null;
        }
        /// <summary>
        /// Require that the user invoking the command has a specified ChannelPermission.
        /// </summary>
        /// <param name="permission">The ChannelPermission that the user must have. Multiple permissions can be specified by ORing the permissions together.</param>
        /// <example>
        /// <code language="c#">
        ///     [Command("permission")]
        ///     [RequireUserPermission(ChannelPermission.ReadMessageHistory | ChannelPermission.ReadMessages)]
        ///     public async Task HasPermission()
        ///     {
        ///         await ReplyAsync("You can read messages and the message history!");
        ///     }
        /// </code>
        /// </example>
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
