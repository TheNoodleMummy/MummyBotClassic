using Discord;
using Discord.Addons.Interactive;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{
    [Name("Help Commands"), Description("This Module helps with commands as in what they need and if its required or optional")]
    public class HelpModule : MummyModule
    {
        public CommandService Commands { get; set; }
        public InteractiveService Iservice { get; set; }

        [Command("commands", "help"), Description("All Command for Mummybot")]
        public async Task HelpAsync()
        {
            try
            {
                var prefixes = GuildConfig.Prefixes;

                var pages = new List<string>();
                foreach (var module in Commands.GetAllModules())
                {
                    var modulechecks = await module.RunChecksAsync(Context, Services);
                    if (modulechecks.IsSuccessful)
                    {
                        string description = null;
                        foreach (var cmd in module.Commands)
                        {
                            var commandchecks = await cmd.RunChecksAsync(Context, Services);
                            if (commandchecks.IsSuccessful)
                                description += $"{prefixes?.FirstOrDefault()?.Prefix ?? Context.Client.CurrentUser.Mention}{cmd?.FullAliases?.First()}\n";
                        }

                        if (!string.IsNullOrWhiteSpace(description))
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append("**__").Append(module.Name).Append("__\n").Append(module.Description).AppendLine("**");
                            sb.AppendLine(description);
                            pages.Add(sb.ToString());
                        }
                    }
                    
                }

                var msg = new PaginatedMessage
                {
                    Pages = pages,
                    Author = new EmbedAuthorBuilder() { Name = Context.User.GetDisplayName(), IconUrl = Context.User.GetAvatarUrl() ?? Context.User.GetDefaultAvatarUrl() },
                    Title = "Some Commands you can run"
                };
                await new PaginatedMessageCallback(Iservice, Context, msg).DisplayAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        [Command("help"), Description("Specific help for a module")]
        public async Task HelpAsync([Description("Command you want the help for")]Module module)
        {
            var prefixes = GuildConfig.Prefixes;

            var builder = new EmbedBuilder()
            {
                Color = Context.User.Roles.FirstOrDefault(x => x.IsHoisted)?.Color ?? Color.DarkRed,
                Description = $"Here are some commands in the  **{module.Name}**"
            };
            builder.WithAuthor(Context.User.GetDisplayName(), Context.User.GetAvatarUrl());
            StringBuilder sb = new StringBuilder();
            foreach (var cmd in module.Commands)
            {
                if (!(await cmd.RunChecksAsync(Context, Services)).IsSuccessful) return;
                sb.Append(prefixes[0]).AppendLine(cmd.Name);
            }
            builder.AddField("\u200B", sb.ToString());
            await ReplyAsync(embed: builder);
        }

        [Command("help"), Description("Specific help for a command")]
        public async Task HelpAsync([Description("Command you want the help for")]string command)
        {
            var result = Commands.FindCommands(command).ToArray();
            if (result.Length == 0)
            {
                await ReplyAsync($"Sorry, I couldn't find a command like **{command}**.");
                return;
            }

            var builder = new EmbedBuilder()
            {
                Color = Context.User.Roles.FirstOrDefault(x => x.IsHoisted)?.Color ?? Color.DarkRed,
                Description = $"Here are some commands like **{command}**"
            };

            foreach (var match in result)
            {
                var cmd = match.Command;
                if (!(await cmd.RunChecksAsync(Context)).IsSuccessful) return;
                builder.AddField(x =>
                {
                    x.Name = cmd.Name;
                    x.Value = $"Alias: {string.Join(",", cmd.Aliases) ?? "none"}\n" +
                              $"Parameters: {string.Join(", ", cmd.Parameters.Select(p => p.Name)) ?? " none"}\n" +
                              $"Remarks: {cmd.Description ?? " none"}";
                    x.IsInline = false;
                });
            }

            await ReplyAsync(embed: builder);
        }
    }
}