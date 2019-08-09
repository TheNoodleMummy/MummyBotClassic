﻿using Discord;
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
        [RequirePermissions(Enums.PermissionTarget.Bot,GuildPermission.ManageRoles)]
        public async Task Color(int r,int g,int b,SocketGuildUser user= null)
        {
            user = user ?? Context.User;
            IRole role = user.Roles.OrderBy(r => r.Position).FirstOrDefault(r => r.IsHoisted);
            var color = new Discord.Color(r, g, b);
            if (role is null)
                role = await Context.Guild.CreateRoleAsync(Context.User.GetDisplayName(), color: color, isHoisted: true);
            else
                await role.ModifyAsync(r => r.Color= color);            
        }
        [Command("color")]
        [RequirePermissions(Enums.PermissionTarget.Bot, GuildPermission.ManageRoles)]
        public async Task Color(uint hex, SocketGuildUser user = null)
        {
            user = user ?? Context.User;
            IRole role = user.Roles.OrderBy(r => r.Position).FirstOrDefault(r => r.IsHoisted);
            var color = new Discord.Color(hex);
            if (role is null)
                role = await Context.Guild.CreateRoleAsync(Context.User.GetDisplayName(), color: color, isHoisted: true);
            else
                await role.ModifyAsync(r => r.Color = color);
        }


    }
}