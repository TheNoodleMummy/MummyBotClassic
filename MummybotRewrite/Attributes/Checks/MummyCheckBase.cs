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
        public override ValueTask<CheckResult> CheckAsync(CommandContext context, IServiceProvider provider)
      => CheckAsync((MummyContext)context, provider);
        public abstract ValueTask<CheckResult> CheckAsync(MummyContext context, IServiceProvider provider);
        
    }
}
