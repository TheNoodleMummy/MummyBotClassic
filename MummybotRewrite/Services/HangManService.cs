using Casino.Common;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Mummybot.Commands;
using Mummybot.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        {
            return Task.CompletedTask;
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
                if (guildconfig.HangManRoleId == 0 )
                {
                    role = await ctx.Guild.CreateRoleAsync("hangman");
                    guildconfig.HangManRoleId = role.Id;
                    var position = ctx.Guild.CurrentUser.Roles.OrderBy(r => r.Position).Last().Position -1;
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
                    await channel.AddPermissionOverwriteAsync(ctx.Guild.EveryoneRole,OverwritePermissions.DenyAll(channel));
                    await channel.AddPermissionOverwriteAsync(role, new OverwritePermissions(viewChannel:PermValue.Allow,sendMessages:PermValue.Allow));
                }
                else //channel already made
                {
                    channel = ctx.Guild.TextChannels.FirstOrDefault(x => x.Id == guildconfig.HangManChannelID);

                }
                MessageService.SendAsync(ctx, x => x.Content = $"a game has been made please go to {channel.Mention} ");
                
                await user.AddRoleAsync(role);
                ActiveGames[ctx.GuildId] = new HangmanGame() { Channel = channel, User = user, /*Word = GetRandomWord(),*/GuildId = ctx.GuildId };
            }
            else
            {
                //game already in progress handle it here
            }
            
            guildstore.Update(guildconfig);
            guildstore.SaveChanges();
        }

        public Word GetRandomWord()
        {
            using var guildstore = Services.GetRequiredService<GuildStore>();
            var maxlenghgt = guildstore.Words.ToList().Where(x => x.Reported == false).Count();
            var rnd = Random.Next(0, maxlenghgt);
            var word = guildstore.Words.FirstOrDefault(x => x.id == rnd);
            word.used++;
            guildstore.Update(word);
            guildstore.SaveChanges();
            return new Word() { MaskedWord = word.word };
        }

        public async Task TaskCallback(HangmanGame game)
        {

        }

    }

    public class HangmanGame
    {
        public ushort State { get; set; } = 0;
        public ulong GuildId { get; set; }

        public DateTimeOffset startedAt = DateTimeOffset.UtcNow;
        public ITextChannel Channel { get; set; }
        public SocketGuildUser User { get; set; }
        public Word Word { get; set; }
    }

    public class Word
    {
        public string MaskedWord
        {
            get
            {
                string strng = "";
                foreach (var item in word.ToCharArray())
                {
                    if (GuessedLetters[item])
                    {
                        strng += item;
                    }
                    else
                    {
                        strng += "*";
                    }
                }
                return strng;
            }
            set
            {
                word = value;
            }
        }
        private string word;

        public Dictionary<char, bool> GuessedLetters = new Dictionary<char, bool>();
        public Word()
        {
            GuessedLetters.Add('a', false);
            GuessedLetters.Add('b', false);
            GuessedLetters.Add('c', false);
            GuessedLetters.Add('d', false);
            GuessedLetters.Add('e', false);
            GuessedLetters.Add('f', false);
            GuessedLetters.Add('g', false);
            GuessedLetters.Add('h', false);
            GuessedLetters.Add('i', false);
            GuessedLetters.Add('j', false);
            GuessedLetters.Add('k', false);
            GuessedLetters.Add('l', false);
            GuessedLetters.Add('m', false);
            GuessedLetters.Add('n', false);
            GuessedLetters.Add('o', false);
            GuessedLetters.Add('p', false);
            GuessedLetters.Add('q', false);
            GuessedLetters.Add('r', false);
            GuessedLetters.Add('s', false);
            GuessedLetters.Add('t', false);
            GuessedLetters.Add('u', false);
            GuessedLetters.Add('v', false);
            GuessedLetters.Add('w', false);
            GuessedLetters.Add('x', false);
            GuessedLetters.Add('y', false);
            GuessedLetters.Add('z', false);
        }


    }
}
