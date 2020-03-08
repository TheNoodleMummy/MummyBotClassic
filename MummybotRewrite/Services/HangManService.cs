

using Casino.Common;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Mummybot.Commands;
using Mummybot.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mummybot.Services
{
    public class HangManService : BaseService
    {
        public List<string> HangmanArt = new List<string>()
            {
                "\u200b                           \u200b\n"+//step 0
                "\u200b  \n"+
                "\u200b  \n"+
                "\u200b  \n"+
                "\u200b  \n"+
                "\u200b  \n"+
                "\u200b  \n"+
                "\u200b  \n"+
                "\u200b  \n"+
                "\u200b  \n"+
                "\u200b  \n"+
                "\u200b  \n"+
                "\u200b  \n"+
                "\u200b  \n"+
                "\u200b  \n"+
                "\u200b  \n"+
                "\u200b  \n"+
                "\u200b  \n"+
                "\u200b  \n"+
                "\u200b  \n"+
                "\u200b  \n"+
                "\u200b  \n"+
                "\u200b  \n"+
                "",

                "            \u200B               \n" +//step 1
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "| |\n"+
                "| |\n" +
                ": :\n"+
                ". .",

                "            \u200B               \n" +//step 2
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "| |                     | |\n" +
                "| |                     | |\n"+
                ": :                     : : \n"+
                ". .                     . .",

                "            \u200B               \n" +//step3
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                "\n"+
                " _________________________\n" +
                "|  _____________________  |\n"+
                "| |                     | |\n" +
                "| |                     | |\n"+
                ": :                     : : \n"+
                ". .                     . .",

                "\n"+
                " _\n" + //step 4
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |_______________________\n" +
                "|  _____________________  |\n"+
                "| |                     | |\n" +
                "| |                     | |\n"+
                ": :                     : : \n"+
                ". .                     . .",

                "\n"+
                " ____________________\n" +//step 5
                "| .__________________|\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |_______________________\n" +
                "|  _____________________  |\n"+
                "| |                     | |\n" +
                "| |                     | |\n"+
                ": :                     : : \n"+
                ". .                     . .",

                "\n"+
                " ____________________\n" +//step 6
                "| .__________________|\n" +
                "| | / /\n" +
                "| |/ /\n" +
                "| | /\n" +
                "| |/\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |_______________________\n" +
                "|  _____________________  |\n"+
                "| |                     | |\n" +
                "| |                     | |\n"+
                ": :                     : : \n"+
                ". .                     . .",

                "\n"+
                " ___________.._______\n" + //step 7
                "| .__________))______|\n" +
                "| | / /      ||\n" +
                "| |/ /       ||\n" +
                "| | /        ||\n" +
                "| |/         ||\n" +
                "| |          ||\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |_______________________\n" +
                "|  _____________________  |\n"+
                "| |                     | |\n" +
                "| |                     | |\n"+
                ": :                     : : \n"+
                ". .                     . .",

                "\n"+
                " ___________.._______\n" +//step 8
                "| .__________))______|\n" +
                "| | / /      ||\n" +
                "| |/ /       ||\n" +
                "| | /        ||.-''.\n" +
                "| |/         |/  _  \\\n" +
                "| |          ||  `/,|\n" +
                "| |          (\\\\`_.'\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |_______________________\n" +
                "|  _____________________  |\n"+
                "| |                     | |\n" +
                "| |                     | |\n"+
                ": :                     : : \n"+
                ". .                     . .",

                "\n"+
                " ___________.._______\n" +//step 9
                "| .__________))______|\n" +
                "| | / /      ||\n" +
                "| |/ /       ||\n" +
                "| | /        ||.-''.\n" +
                "| |/         |/  _  \\\n" +
                "| |          ||  `/,|\n" +
                "| |          (\\\\`_.'\n" +
                "| |         .-`--'.\n" +
                "| |          |. .|\n" +
                "| |          |   |\n" +
                "| |          | . |\n" +
                "| |          |___|\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |_______________________\n" +
                "|  _____________________  |\n"+
                "| |                     | |\n" +
                "| |                     | |\n"+
                ": :                     : : \n"+
                ". .                     . .",

                "\n"+
                " ___________.._______\n" +//step 10
                "| .__________))______|\n" +
                "| | / /      ||\n" +
                "| |/ /       ||\n" +
                "| | /        ||.-''.\n" +
                "| |/         |/  _  \\\n" +
                "| |          ||  `/,|\n" +
                "| |          (\\\\`_.'\n" +
                "| |         .-`--'.\n" +
                "| |        /_ . . _\n" +
                "| |       // |   |\n" +
                "| |      //  | . |\n" +
                "| |     ')   |___|\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |_______________________\n" +
                "|  _____________________  |\n"+
                "| |                     | |\n" +
                "| |                     | |\n"+
                ": :                     : : \n"+
                ". .                     . .",

                "\n"+
                 " ___________.._______\n" +//step 11
                "| .__________))______|\n" +
                "| | / /      ||\n" +
                "| |/ /       ||\n" +
                "| | /        ||.-''.\n" +
                "| |/         |/  _  \\\n" +
                "| |          ||  `/,|\n" +
                "| |          (\\\\`_.'\n" +
                "| |         .-`--'.\n" +
                "| |        /_ . . _\\\n" +
                "| |       // |   | \\\\\n" +
                "| |      //  | . |  \\\\\n" +
                "| |     ')   |___|   (`\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |\n" +
                "| |_______________________\n" +
                "|  _____________________  |\n"+
                "| |                     | |\n" +
                "| |                     | |\n"+
                ": :                     : : \n"+
                ". .                     . .",

                "\n"+
                " ___________.._______\n" +//step 12
                "| .__________))______|\n" +
                "| | / /      ||\n" +
                "| |/ /       ||\n" +
                "| | /        ||.-''.\n" +
                "| |/         |/  _  \\\n" +
                "| |          ||  `/,|\n" +
                "| |          (\\\\`_.'\n" +
                "| |         .-`--'.\n" +
                "| |        /_ . . _\\\n" +
                "| |       // |   | \\\\\n" +
                "| |      //  | . |  \\\\\n" +
                "| |     ')   | __|   (`\n" +
                "| |          ||\n" +
                "| |          ||\n" +
                "| |          ||\n" +
                "| |          ||\n" +
                "| |_________/_|___________\n" +
                "|  _____________________  |\n"+
                "| |                     | |\n" +
                "| |                     | |\n"+
                ": :                     : : \n"+
                ". .                     . .",

                "\n"+
                " ___________.._______\n" +//step 13
                "| .__________))______|\n" +
                "| | / /      ||\n" +
                "| |/ /       ||\n" +
                "| | /        ||.-''.\n" +
                "| |/         |/  _  \\\n" +
                "| |          ||  `/,|\n" +
                "| |          (\\\\`_.'\n" +
                "| |         .-`--'.\n" +
                "| |        /_ . . _\\\n" +
                "| |       // |   | \\\\\n" +
                "| |      //  | . |  \\\\\n" +
                "| |     ')   |   |   (`\n" +
                "| |          ||'||\n" +
                "| |          || ||\n" +
                "| |          || ||\n" +
                "| |          || ||\n" +
                "| |_________/_|_|_\\_______\n" +
                "|  _____________________  |\n"+
                "| |                     | |\n" +
                "| |                     | |\n"+
                ": :                     : : \n"+
                ". .                     . .",

                "\n"+
                " ___________.._______\n" +//step 14 your dead
                "| .__________))______|\n" +
                "| | / /      ||\n" +
                "| |/ /       ||\n" +
                "| | /        ||.-''.\n" +
                "| |/         |/  _  \\\n" +
                "| |          ||  `/,|\n" +
                "| |          (\\\\`_.'\n" +
                "| |         .-`--'.\n" +
                "| |        /_ . . _\\\n" +
                "| |       // |   | \\\\\n" +
                "| |      //  | . |  \\\\\n" +
                "| |     ')   |   |   (`\n" +
                "| |          ||'||\n" +
                "| |          || ||\n" +
                "| |          || ||\n" +
                "| |          || ||\n" +
                "| |________ / | | \\   _____\n" +
                "|  ________|\\`-' `-' |_____|\n"+
                "| |       \\ \\           | |\n" +
                "| |        \\ \\          | |\n"+
                ": :         \\ \\         : :\n"+
                ". .          `'         . ."
        };

        private Random Random;
        private IServiceProvider Services;
        private DiscordSocketClient discordclient;
        private TaskQueue taskqueue;
        private MessageService MessageService;
        public Dictionary<ulong, HangmanGame> ActiveGames = new Dictionary<ulong, HangmanGame>();

        public TimeSpan Timeout = TimeSpan.FromMinutes(1);

        public override Task InitialiseAsync(IServiceProvider services)
        {
            Random = services.GetRequiredService<Random>();
            Services = services;
            discordclient = services.GetRequiredService<DiscordSocketClient>();
            taskqueue = services.GetRequiredService<TaskQueue>();
            MessageService = services.GetRequiredService<MessageService>();
            discordclient.MessageReceived += Discordclient_MessageReceived;
            return Task.CompletedTask;
        }

        private Task Discordclient_MessageReceived(SocketMessage arg)
        => HandleGuessAsync(arg);

        private async Task HandleGuessAsync(SocketMessage arg)
        {
            if (arg.Channel is SocketTextChannel && ActiveGames.TryGetValue((arg.Channel as SocketTextChannel).Guild.Id, out var game))
            {
                var ctx = MummyContext.Create(discordclient, arg as IUserMessage, null, Services, "", false);
                if (ctx.UserId == discordclient.CurrentUser.Id)
                    return;
                if (ctx.UserId != game.User.Id)
                    return;

                var result = game.Word.GeussLetter(ctx.Message.Content);
                if (!result.correct)
                {
                    game.State++;
                    if (game.State == 14)
                    {
                        await game.Message.ModifyAsync(x => x.Embed = new EmbedBuilder()
                        .WithDescription($"```{HangmanArt[14]}```")
                        .AddField("you are dead", ":(")
                        .AddField("The word was:", game.Word.UnMaskedWord)
                        .AddField("Word Id:", game.Word.Id)
                        .Build());
                        _ = Task.Delay(Timeout)
                                .ContinueWith(async _ =>
                                {
                                    await game.User.RemoveRoleAsync(game.Role);
                                });
                    }

                }
                else if (game.Word.MaskedWord == game.Word.UnMaskedWord)
                {
                    await game.Message.ModifyAsync(x => x.Embed = new EmbedBuilder()
                    /*.WithDescription(HangmanArt[game.State])*/
                    .AddField("Word", game.Word.UnMaskedWord)
                    .Build());
                    _ = Task.Delay(Timeout)
                            .ContinueWith(async _ =>
                            {
                                await game.User.RemoveRoleAsync(game.Role);
                            });
                }
                else
                {
                    await game.Message.ModifyAsync(x => x.Embed = new EmbedBuilder()
                    .WithDescription($"```{HangmanArt[game.State]}```")
                    .AddField("Word", game.Word.MaskedWord)
                    .Build());
                }
                await arg.DeleteAsync();
            }
        }

        public async Task StartNewGame(SocketGuildUser user, MummyContext ctx)
        {
            using var guildstore = Services.GetRequiredService<GuildStore>();
            var guildconfig = await guildstore.GetOrCreateGuildAsync(ctx.GuildId);
            if (!guildconfig.UsesHangman)
                return;

            if (!ActiveGames.TryGetValue(ctx.GuildId, out var game))
            {
                IRole role;
                ITextChannel channel;
                if (guildconfig.HangManRoleId == 0)
                {
                    role = await ctx.Guild.CreateRoleAsync("hangman", isMentionable: false);
                    guildconfig.HangManRoleId = role.Id;
                    var position = ctx.Guild.CurrentUser.Roles.OrderBy(r => r.Position).Last().Position - 1;
                    await role.ModifyAsync(r => r.Position = position);
                }
                else
                {
                    role = ctx.Guild.GetRole(guildconfig.HangManRoleId);
                }
                if (guildconfig.HangManChannelID == 0)
                {
                    channel = await ctx.Guild.CreateTextChannelAsync("HangMan");
                    guildconfig.HangManChannelID = channel.Id;
                    await channel.AddPermissionOverwriteAsync(ctx.Guild.EveryoneRole, OverwritePermissions.DenyAll(channel));
                    await channel.AddPermissionOverwriteAsync(role, new OverwritePermissions(viewChannel: PermValue.Allow, sendMessages: PermValue.Allow));
                }
                else //channel already made
                {
                    channel = ctx.Guild.TextChannels.FirstOrDefault(x => x.Id == guildconfig.HangManChannelID);

                }
                var msg = (await MessageService.SendAsync(ctx, x => x.Content = $"a game has been made please go to {channel.Mention} "));



                await user.AddRoleAsync(role);
                game = new HangmanGame() { Channel = channel, User = user, Word = GetRandomWord(), GuildId = ctx.GuildId };
                var hangmanmsg = await channel.SendMessageAsync("", embed: new EmbedBuilder()
                    .WithDescription($"```{HangmanArt[game.State]}```")
                    .AddField("Word", game.Word.MaskedWord)
                    .Build());
                game.Message = hangmanmsg;
                ActiveGames[ctx.GuildId] = game;

            }
            else
            {
                await MessageService.SendAsync(ctx, x => x.Content = $"{game.User.GetDisplayName()} is already palying a game please try again later");
            }

            await guildstore.SaveChangesAsync();
        }

        public Word GetRandomWord()
        {
            using var guildstore = Services.GetRequiredService<GuildStore>();
            var maxlenghgt = guildstore.Words.Where(x => x.Reported == false).Count();
            var rnd = Random.Next(0, maxlenghgt);
            var word = guildstore.Words.Where(x => x.Reported == false).FirstOrDefault(x => x.id == rnd);
            word.used++;
            guildstore.SaveChanges();
            return new Word() { MaskedWord = word.word.ToLower(), Id = word.id };
        }

        public async Task TaskCallback(HangmanGame game)
        {

        }

    }

    public class HangmanGame
    {
        public IUserMessage Message { get; set; }
        public ushort State { get; set; } = 0;
        public ulong GuildId { get; set; }

        public DateTimeOffset startedAt = DateTimeOffset.UtcNow;
        public IRole Role { get; set; }
        public ITextChannel Channel { get; set; }
        public SocketGuildUser User { get; set; }
        public Word Word { get; set; }
    }

    public class Word
    {
        public Word()
        {
            GuessedLetters.Add("a", false);
            GuessedLetters.Add("b", false);
            GuessedLetters.Add("c", false);
            GuessedLetters.Add("d", false);
            GuessedLetters.Add("e", false);
            GuessedLetters.Add("f", false);
            GuessedLetters.Add("g", false);
            GuessedLetters.Add("h", false);
            GuessedLetters.Add("i", false);
            GuessedLetters.Add("j", false);
            GuessedLetters.Add("k", false);
            GuessedLetters.Add("l", false);
            GuessedLetters.Add("m", false);
            GuessedLetters.Add("n", false);
            GuessedLetters.Add("o", false);
            GuessedLetters.Add("p", false);
            GuessedLetters.Add("q", false);
            GuessedLetters.Add("r", false);
            GuessedLetters.Add("s", false);
            GuessedLetters.Add("t", false);
            GuessedLetters.Add("u", false);
            GuessedLetters.Add("v", false);
            GuessedLetters.Add("w", false);
            GuessedLetters.Add("x", false);
            GuessedLetters.Add("y", false);
            GuessedLetters.Add("z", false);
        }

        public int Id { get; set; }

        public Dictionary<string, bool> GuessedLetters = new Dictionary<string, bool>();

        public string UnMaskedWord { get; private set; }

        public string MaskedWord
        {
            get
            {
                string strng = "`";
                foreach (char item in UnMaskedWord.Trim())
                {
                    if (GuessedLetters[item.ToString()])
                    {
                        strng += item;
                    }
                    else
                    {
                        strng += "*";
                    }
                }
                strng += "`";
                return strng;
            }
            set
            {
                UnMaskedWord = value.ToLower();
            }
        }

        public (bool correct, string letter, FailedReasons reason) GeussLetter(string letter)
        {

            if (GuessedLetters[letter])
                return (false, letter, FailedReasons.LetterWasAllreadyGuessed);




            GuessedLetters[letter] = true;
            var indexes = UnMaskedWord.AllIndexesOf(letter);
            if (!indexes.Any())
                return (false, letter, FailedReasons.ThisLetterWasNotInTheWord);
            else
                return (true, letter, FailedReasons.None);
        }


    }

    public enum FailedReasons
    {
        LetterWasAllreadyGuessed,
        ThisLetterWasNotInTheWord,
        None
    }

    public static class StringExtends
    {
        public static IEnumerable<int> AllIndexesOf(this string str, string searchstring)
        {
            int minIndex = str.IndexOf(searchstring);
            while (minIndex != -1)
            {
                yield return minIndex;
                minIndex = str.IndexOf(searchstring, minIndex, searchstring.Length);
            }
        }
    }
}
