using Discord;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Mummybot.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Immutable;
using Mummybot.Attributes;

namespace Mummybot.Services
{
    public class EvalService : BaseService
    {
        public Script<object> Build(string code)
        {
            var codes = Utilities.GetCodes(code);

            var namespaces = Assembly.GetEntryAssembly()?.GetTypes()
              .Where(x => !string.IsNullOrWhiteSpace(x.Namespace))
              .Select(x => x.Namespace)
              .Distinct();

            var scriptOptions = ScriptOptions.Default.WithReferences(GetAssemblies().Select(x => MetadataReference.CreateFromFile(x.Location))).AddImports(namespaces);

            var usings = new[]
            {
                "Discord", "Discord.WebSocket",
                "Microsoft.Extensions.DependencyInjection",
                "System", "System.Collections.Generic", "System.Linq", "System.Text", "System.Threading.Tasks",
                "Qmmands"
            };

            var toEval = codes.Count == 0 ? code : string.Join('\n', codes);

            return CSharpScript
                .Create($"{string.Join("", usings.Select(x => $"using {x};"+Environment.NewLine))} {toEval}",
                    scriptOptions,
                    typeof(RoslynContext));
        }

        internal IEnumerable<Assembly> GetAssemblies()
        {
            var assm = AppDomain.CurrentDomain.GetAssemblies()
                .Where(x => !x.IsDynamic && !string.IsNullOrWhiteSpace(x.Location));

            foreach (var assembly in assm)
                yield return assembly;
        }
    }

    public class RoslynContext
    {
        public MummyContext Ctx;
        public IServiceProvider Services { get; set; }
        public MessageService MessageService { get; set; }

        public RoslynContext(MummyContext context,IServiceProvider services)
        {
            Ctx = context;
            Services = services;
            MessageService = services.GetRequiredService<MessageService>();
        }
    }
}
