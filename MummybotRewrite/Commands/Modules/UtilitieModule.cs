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
        public async Task Color(int red, int green, int blue, SocketGuildUser user = null)
            => ChangeColorRGB(new Color(red, green, blue), user);       

        [Command("color")]
        [RequirePermissions(Enums.PermissionTarget.Bot, GuildPermission.ManageRoles)]
        [RequirePermissions(Enums.PermissionTarget.User, GuildPermission.ManageRoles)]
        public async Task Color(uint rawhex, SocketGuildUser user = null)
            => ChangeColorRGB(new Color(rawhex), user);


        [Command("color")]
        [RequirePermissions(Enums.PermissionTarget.Bot, GuildPermission.ManageRoles)]
        public async Task Color(uint rawhex)
           => ChangeColorRGB(new Color(rawhex));

        [Command("color")]
        [RequirePermissions(Enums.PermissionTarget.Bot, GuildPermission.ManageRoles)]
        public async Task Color(int red, int green, int blue)
            => ChangeColorRGB(new Color(red, green, blue));

        internal async Task ChangeColorRGB(Color color, SocketGuildUser user=null)
        {
            user ??= Context.User;
            IRole role = user.Roles.FirstOrDefault(r => r.Name.Contains(user.Username, StringComparison.InvariantCultureIgnoreCase));
            if (role is null)
            {
                role = await user.Guild.CreateRoleAsync(user.GetDisplayName(), color: color);
                await role.ModifyAsync(role => role.Position = user.Roles.OrderByDescending(roles => roles.Position).FirstOrDefault().Position - 1);
                await user.AddRoleAsync(role);
            }
            else
            {
                await role.ModifyAsync(r => color = color);
            }
        }

    }
}
