using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Mummybot.Commands;
using Qmmands;

namespace Mummybot.Attributes.Checks
{
    [Name("Require VoiceChannel")]
    public class RequireVoiceChannel : MummyCheckBase
    {
        public override ValueTask<CheckResult> CheckAsync(MummyContext context)
        {
            if ((context.User as IVoiceState)?.VoiceChannel is null)
                return CheckResult.Unsuccessful("You are Required to be in a voice channel to run this command");
            else
                return CheckResult.Successful;

        }
    }
}
