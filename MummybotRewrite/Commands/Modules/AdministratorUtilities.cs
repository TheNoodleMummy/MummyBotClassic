﻿using Casino.Common;
using Discord;
using Discord.WebSocket;
using Mummybot.Attributes.Checks;
using Mummybot.Enums;
using Mummybot.Extentions;
using Mummybot.Services;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{
    public class AdministratorUtilities : MummyModule
    {
        [Group("voice")]
        public class AdministratorVoiceUtilities : MummyModule
        {
            [Group("mute")]
            public class AdministratorMuteUtilities : MummyModule
            {
                public AdministratorUitilitiesService AdministratorService { get; set; }

                [Command]
                [RequirePermissions(Enums.PermissionTarget.Bot, guildPerms: GuildPermission.ManageRoles)]
                public async Task VoiceMute(
                    [Description("User to mute")]SocketGuildUser user,
                    [Description("well how long duh..")]TimeSpan howlong
                    )
                {
                    await AdministratorService.VoiceMute(Context, user, howlong);
                    await Context.Message.AddOkAsync();
                }
            }

            [Group("deafen")]
            public class AdministratorDeafenUtilities : MummyModule
            {
                public AdministratorUitilitiesService AdministratorService { get; set; }

                [Command]
                [RequirePermissions(Enums.PermissionTarget.Bot, guildPerms: GuildPermission.ManageRoles)]
                public async Task VoiceDeafen(
                    [Description("User to deafen")]SocketGuildUser user,
                    [Description("well how long duh..")]TimeSpan howlong
                    )
                {
                    await AdministratorService.VoiceDeafen(Context, user, howlong);
                    await Context.Message.AddOkAsync();
                }
            }

            [Command("Kick")]
            [RequirePermissions(Enums.PermissionTarget.Bot, guildPerms: GuildPermission.KickMembers)]
            public async Task VoiceKickAsync(SocketGuildUser user)
            {
                await user.ModifyAsync(u => u.Channel = null);
                await Context.Message.AddOkAsync();
            }
        }
    }
}
