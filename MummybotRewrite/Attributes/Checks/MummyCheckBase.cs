using Mummybot.Commands;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Attributes.Checks
{
    public abstract class MummyCheckBase : CheckAttribute
    {
        public override ValueTask<CheckResult> CheckAsync(CommandContext context)
      => CheckAsync((MummyContext)context);
        public abstract ValueTask<CheckResult> CheckAsync(MummyContext context);
        
    }
}
