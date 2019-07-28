using Casino.Common;
using Discord;
using Humanizer;
using Microsoft.CodeAnalysis;
using Mummybot.Attributes.Checks;
using Mummybot.Database.Entities;
using Mummybot.Extentions;
using Mummybot.Services;
using Qmmands;
using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{
    [RequireOwner]
    public class OwnerModule : MummyModule
    {
        public EvalService EvalService { get; set; }

        public TaskQueue TaskQueue { get; set; }

        [Command("eval")]
        public async Task Eval([Remainder]string code)
        {
            var builder = new EmbedBuilder
            {
                Title = "Evaluating Code...",
                Color = Color.DarkRed,
                Description = "Waiting for completion...",
                Author = new EmbedAuthorBuilder
                {
                    IconUrl = Context.User.GetAvatarOrDefaultUrl(),
                    Name = Context.User.GetDisplayName()
                },
                Timestamp = DateTimeOffset.UtcNow,
                ThumbnailUrl = Context.Guild.CurrentUser.GetAvatarOrDefaultUrl()
            };
            var msg = await ReplyAsync(embed: builder);
            var sw = Stopwatch.StartNew();
            var script = EvalService.Build(code);

            string snippet = string.Join(Environment.NewLine, script.Code.Split(Environment.NewLine.ToCharArray()).Where(line => !line.StartsWith("using")));

            var diagnostics = script.Compile();
            var compilationTime = sw.ElapsedMilliseconds;

            if (diagnostics.Any(x => x.Severity == DiagnosticSeverity.Error))
            {
                builder.WithDescription($"Compilation finished in: {compilationTime}ms");
                builder.WithColor(Color.Red);
                builder.WithTitle("Failed Evaluation");

                builder.AddField("Code", $"```cs\n{snippet}```");
                builder.AddField("Compilation Errors", string.Join('\n', diagnostics.Select(x => $"{x}")));

                await msg.ModifyAsync(x => x.Embed = builder.Build());

                return;
            }
            sw.Restart();

            var context = new RoslynContext(Context, Services);

            try
            {
                var result = await script.RunAsync(context);

                sw.Stop();
                builder.WithColor(Color.Green);
                builder.AddField("Code", $"```cs{Environment.NewLine}{snippet}```");

                builder.WithDescription($"Code compiled in {compilationTime}ms and ran in {sw.ElapsedMilliseconds}ms");
                builder.WithTitle("Code Evaluated");

                if (!(result.ReturnValue is null))
                {
                    var sb = new StringBuilder();
                    var type = result.ReturnValue.GetType();
                    var rValue = result.ReturnValue;

                    switch (rValue)
                    {
                        case Color col:
                            builder.WithColor(col);
                            builder.AddField("Colour", $"{col.RawValue}");
                            break;

                        case string str:
                            builder.AddField($"{type}", $"\"{str}\"");
                            break;

                        case IEnumerable enumerable:

                            var list = enumerable.Cast<object>().ToList();
                            var enumType = enumerable.GetType();

                            if (list.Count > 10)
                            {
                                builder.AddField($"{enumType}", "Enumerable has more than 10 elements");
                                break;
                            }

                            if (list.Count > 0)
                            {
                                sb.AppendLine("```css");

                                foreach (var element in list)
                                    sb.Append('[').Append(element).AppendLine("]");

                                sb.AppendLine("```");
                            }
                            else
                            {
                                sb.AppendLine("Collection is empty");
                            }

                            builder.AddField($"{enumType}", sb.ToString());

                            break;

                        case Enum @enum:

                            builder.AddField($"{@enum.GetType()}", $"```\n{@enum.Humanize()}\n```");

                            break;

                        default:

                            var messages = rValue.Inspect();

                            if (type.IsValueType && messages.Count == 0)
                            {
                                builder.AddField($"{type}", rValue);
                            }

                            foreach (var message in messages)
                                await ReplyAsync($"```css\n{message}\n```");

                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                sw.Stop();

                builder.AddField("Code", $"```cs\n{snippet}```");

                builder.WithDescription($"Code evaluated in {sw.ElapsedMilliseconds}ms but there was a issue tho");
                builder.WithColor(Color.Red);
                builder.WithTitle("Failed Evaluation");

                var str = ex.ToString();

                builder.AddField("Exception", Format.Sanitize(str.Length >= 1000 ? str.Substring(0, 1000) : str));
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            await msg.ModifyAsync(x => x.Embed = builder.Build());
        }

        [Command("addusing")]
        public async Task AddUsingsAsync(string ns)
        {
            if (ns.Contains(" "))
            {
                await ReplyAsync($"the namespace {ns} contains a space => no beuno");
                return;
            }
            EvalService.usings.Add(ns);
            EvalService.SaveUsings();
            await Context.Message.AddOkAsync();
        }

        [Command("removeusing")]
        public async Task RemoveUsingsAsync(string ns)
        {
            if (EvalService.usings.Contains(ns))
            {
                EvalService.usings.Remove(ns);
                EvalService.SaveUsings();
            }
            await Context.Message.AddOkAsync();
        }

        [Command("usings")]
        public Task UsingsAsync()
        => ReplyAsync(string.Join('\n', EvalService.usings));

        [Command("Tasks")]
        public async Task GetTasks()
        {
            var tasks = TaskQueue.Queue.ToArray();
            var current = TaskQueue.CurrentTask;
            if (tasks is null && current is null)
                await ReplyAsync("Currently not tracking anything");
            else if (tasks is null && current !=null)
            {
                var sb = new StringBuilder();

                if (current is ScheduledTask<Birthday> bdaytask)
                {
                    sb.AppendLine(bdaytask.State.ToString());
                }
                else if (current is ScheduledTask<Reminder> remindertask)
                {
                    sb.Append(remindertask.State.ToString()).Append(" at ").AppendLine(remindertask.ExecutionTime.ToString());
                }
                else if (current is ScheduledTask scheduledTask)
                {
                    sb.AppendLine(scheduledTask.ToString());
                }
                await ReplyAsync(sb.ToString());
            }
            else
            {
                var sb = new StringBuilder();
                sb.AppendLine("```");
                foreach (var task in tasks)
                {
                    if (task is ScheduledTask<Birthday> bdaytask)
                    {
                        sb.AppendLine(bdaytask.State.ToString());
                    }
                    else if (task is ScheduledTask<Reminder> remindertask)
                    {
                        sb.Append(remindertask.State.ToString()).Append(" at ").AppendLine(remindertask.ExecutionTime.ToString());
                    }
                    else if (task is ScheduledTask scheduledTask)
                    {
                        sb.AppendLine(scheduledTask.ToString());
                    }
                }
                sb.AppendLine("```");
                await ReplyAsync(sb.ToString());
            }
        }
    }
}

