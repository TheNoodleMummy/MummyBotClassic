using Discord;
using Discord.WebSocket;
using Mummybot.Attributes.Checks;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{
    public class UtilitieModule : MummyModule
    {
        [Command("color")]
        [RequirePermissions(Enums.PermissionTarget.Bot, GuildPermission.ManageRoles)]
        [RequirePermissions(Enums.PermissionTarget.User, GuildPermission.ManageRoles)]
        public Task Color(Color color, SocketGuildUser user = null)
            => ChangeColorAsync(color, user);       

        [Command("color")]
        [RequirePermissions(Enums.PermissionTarget.Bot, GuildPermission.ManageRoles)]
        public Task Color([Remainder]Color color)
           => ChangeColorAsync(color);
       
        private async Task ChangeColorAsync(Color color, SocketGuildUser user=null)
        {
            user ??= Context.User;
            IRole role = user.Roles.FirstOrDefault(r => r.Name.Contains(user.Username, StringComparison.OrdinalIgnoreCase));
            if (role is null)
            {
                role = await user.Guild.CreateRoleAsync(user.GetDisplayName(), color: color,isMentionable: false);
                await role.ModifyAsync(role => role.Position = user.Roles.OrderByDescending(roles => roles.Position).FirstOrDefault().Position - 1);
                await user.AddRoleAsync(role);
            }
            else
            {
                await role.ModifyAsync(r => r.Color = color);
            }
        }
    }
}
