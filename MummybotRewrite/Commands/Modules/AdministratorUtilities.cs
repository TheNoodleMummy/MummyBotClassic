using Casino.Common;
using Discord;
using Discord.WebSocket;
using Mummybot.Attributes.Checks;
using Mummybot.Enums;
using Mummybot.Services;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{
    class AdministratorUtilities : MummyModule
    {

        [Group("voice")]
        public class AdministratorVoiceUtilities : MummyModule
        {
            public AdministratorUitilitiesService AdministratorService { get; set; }

            [Command("mute")]
            [RequirePermissions(Enums.PermissionTarget.Bot,guildPerms: GuildPermission.ManageRoles)]
            public async Task VoiceMute(TimeSpan time, SocketGuildUser user)
            {
                await AdministratorService.VoiceMute(Context, user, time);
            }
        }
    }
}
