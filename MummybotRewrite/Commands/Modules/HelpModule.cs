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
            var options = PaginatedAppearanceOptions.Default;
            options.InformationText = "```\n" +
                "all symbols are just a sign and are not accepted by commands\n" +
                "<value> optional \n" +
                "\"value\" Remainders \n" +
                "**value** required   \n" +
                "```";

            var msg = new PaginatedMessage { Options = options };

            foreach (var module in Commands.GetAllModules())
            {
                var modulecheck = await module.RunChecksAsync(Context);
                if (modulecheck.IsSuccessful)
                {
                    if (module.Commands.Count == 0)
                        continue; //skip module if commands are 0
                    var emb = new EmbedBuilder();
                    emb.WithTitle(module.Name);
                    emb.WithAuthor(Context.User.GetDisplayName(), Context.User.GetAvatarUrl() ?? Context.User.GetDefaultAvatarUrl());
                    var sb = new StringBuilder();
                    var commands = GetAllCommandsIterator(module);
                    foreach (var command in commands)
                    {
                        var checks = await command.RunChecksAsync(Context);
                        if (checks.IsSuccessful)
                        {
                            sb.Append(Context.PrefixUsed).Append(command.Name).Append(" ");
                            foreach (var parameter in command.Parameters)
                            {
                                if (parameter.IsOptional && parameter.IsRemainder)//optional remiander
                                {
                                    sb.Append($"<\"{parameter.Name}\"> ");
                                }
                                else if (parameter.IsOptional && !parameter.IsRemainder)//optional
                                {
                                    sb.Append($"<{parameter.Name}> ");
                                }
                                else if (!parameter.IsOptional && parameter.IsRemainder) //required remainder
                                {
                                    sb.Append($"\"{parameter.Name}\"");
                                }
                                else if (!parameter.IsOptional && !parameter.IsRemainder)//required
                                {
                                    sb.Append($"**{parameter.Name}** ");
                                }
                            }
                            sb.AppendLine();
                        }
                    }
                    emb.WithDescription(sb.ToString());
                    msg.Pages.Add(emb);
                }

            }
            await new PaginatedMessageCallback(Iservice, Context, msg).DisplayAsync();
        }

        [Command("help"), Description("Specific help for a module")]
        public async Task HelpAsync([Description("Command you want the help for"),Remainder]Module module)
        {

            var builder = new EmbedBuilder()
            {
                Color = Context.User.Roles.FirstOrDefault(x => x.IsHoisted)?.Color ?? Color.DarkRed,
                Description = $"Here are some commands in the  **{module.Name}**"
            };
            builder.WithAuthor(Context.User.GetDisplayName(), Context.User.GetAvatarUrl());
            StringBuilder sb = new StringBuilder();
            foreach (var cmd in module.Commands)
            {
                if (!(await cmd.RunChecksAsync(Context)).IsSuccessful) return;
                sb.Append(Context.PrefixUsed).AppendLine(cmd.Name);
            }
            builder.AddField("\u200B", sb.ToString());
            await ReplyAsync(embed: builder);
        }

        [Command("help"), Description("Specific help for a command")]
        public async Task HelpAsync([Description("Command you want the help for"),Remainder]string command)
        {
            var result = Commands.FindCommands(command).ToArray();
            if (result.Length == 0)
            {
                await ReplyAsync($"Sorry, I couldn't find a command like **{command}**.");
                return;
            }

            var builder = new EmbedBuilder()
                .WithAuthor(Context.User)
                .WithColor(Context.User.Roles.FirstOrDefault(x => x.IsHoisted)?.Color ?? Color.DarkRed);            

            foreach (var match in result.Take(4))
            {
                var sb = new StringBuilder();

                if (Context.PrefixUsed.Length == 1)
                    sb.Append(Context.PrefixUsed).Append(match.Command.Name).Append(" ");                
                else
                    sb.Append(Context.PrefixUsed).Append(" ").Append(match.Command.Name).Append(" ");

                foreach (var parameter in match.Command.Parameters)
                {
                    if (parameter.IsOptional && parameter.IsRemainder)//optional remiander
                    {
                        sb.Append($"<\"{parameter.Name}\"> ");
                    }
                    else if (parameter.IsOptional && !parameter.IsRemainder)//optional
                    {
                        sb.Append($"<{parameter.Name}> ");
                    }
                    else if (!parameter.IsOptional && parameter.IsRemainder) //required remainder
                    {
                        sb.Append($"\"{parameter.Name}\"");
                    }
                    else if (!parameter.IsOptional && !parameter.IsRemainder)//required
                    {
                        sb.Append($"**{parameter.Name}** ");
                    }
                }
                sb.AppendLine();
                builder.AddField("\u200B", sb.ToString());
                foreach (var item in match.Command.Parameters)
                {
                    builder.AddField(item.Name, item.Description??item.Remarks??"missing info",true);
                }
            }

            await ReplyAsync(embed: builder);
        }

        private static IEnumerable<Command> GetAllCommandsIterator(Module module)
        {
            IEnumerable<Command> GetCommands(Module rModule)
            {
                for (var i = 0; i < rModule.Commands.Count; i++)
                    yield return rModule.Commands[i];
            }
            foreach (var command in GetCommands(module))
                yield return command;
        }
    }
}