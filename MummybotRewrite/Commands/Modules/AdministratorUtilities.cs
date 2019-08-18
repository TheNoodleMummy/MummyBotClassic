using Casino.Common;
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
                public async Task VoiceMute(SocketGuildUser user, TimeSpan howlong)
                {
                    await AdministratorService.VoiceMute(Context, user, howlong);
                    Context.Message.AddOkAsync();
                }

                //[Command("cancel")]
                //[RequirePermissions(Enums.PermissionTarget.Bot, guildPerms: GuildPermission.ManageRoles)]
                //public async Task VocieMuteCancel(SocketGuildUser user)
                //{
                //    await AdministratorService.CancelMuteAsync(user);
                //}

            }
        }
    }
}
