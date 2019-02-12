﻿using Discord;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Espeon.Attributes;
using Espeon.Commands.Contexts;
using Espeon.Core.Entities;

namespace Espeon.Services
{
    [Service]
    public class EvalService
    {
        private readonly MessageService _message;
        private readonly IEnumerable<string> _usings = new[]
        {
            "Discord", "Discord.Commands", "Discord.WebSocket",
            "Microsoft.Extensions.DependencyInjection",
            "System", "System.Collections.Generic", "System.Linq", "System.Text", "System.Threading.Tasks",
            "SharpLink"
        };

        public EvalService(MessageService message)
        {
            _message = message;
        }

        public async Task EvaluateAsync(EspeonContext context, string code, IServiceProvider services)
        {
            var scriptOptions = ScriptOptions.Default.WithReferences(GetAssemblies()
                .Select(x => MetadataReference.CreateFromFile(x.Location))).AddImports(GetNamespaces());
            var globals = new Globals
            {
                Context = context,
                Message = _message,
                Services = services,
                HttpClient = context.HttpClient
            };
            var message = await _message.SendMessageAsync(context, "Debugging...");
            var sw = new Stopwatch();
            sw.Start();
            try
            {
                var eval = await CSharpScript.EvaluateAsync(
                    $"{string.Join("", _usings.Select(x => $"using {x};"))} {code}", scriptOptions, globals,
                    typeof(Globals));
                sw.Stop();
                await message.ModifyAsync(x => x.Content = $"Completed! Time taken: {sw.ElapsedMilliseconds}ms\n" +
                                                           $"Returned Results: {eval ?? "none"}");
            }
            catch (Exception ex)
            {
                await message.ModifyAsync(x => x.Content = "Completed! There was an error though:\n" +
                                                           $"{Format.Sanitize(ex.ToString().Substring(0, 500))}");
            }
        }

        private static IEnumerable<string> GetNamespaces()
             => Assembly.GetEntryAssembly().GetTypes().Select(x => x.Namespace).Distinct();

        private static IEnumerable<Assembly> GetAssemblies()
        {
            var entries = Assembly.GetEntryAssembly();
            foreach (var assembly in entries.GetReferencedAssemblies())
                yield return Assembly.Load(assembly);
            yield return entries;
        }
    }
}
