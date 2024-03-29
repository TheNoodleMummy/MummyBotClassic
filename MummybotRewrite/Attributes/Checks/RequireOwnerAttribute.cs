﻿using Mummybot.Commands;
using Qmmands;
using System;
using System.Threading.Tasks;

namespace Mummybot.Attributes.Checks
{
    [Name("Require Owner")]
    public class RequireOwnerAttribute : MummyCheckBase
    {
        public override async ValueTask<CheckResult> CheckAsync(MummyContext context)
        {
            if ((await context.Client.GetApplicationInfoAsync()).Owner.Id == context.User.Id)
                return CheckResult.Successful;
            else
                return CheckResult.Failed("Only my owner can run this command");
        }
    }
}
