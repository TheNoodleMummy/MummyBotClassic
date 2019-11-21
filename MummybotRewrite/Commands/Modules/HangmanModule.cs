using Mummybot.Attributes.Checks;
using Mummybot.Services;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Commands.Modules
{
    [Group("hangman")]
    [RequireHangman]
    public class HangmanModule : MummyModule
    {
        public HangManService hangman { get; set; }

        [Command,Priority(10)]
        public async Task Hangman()
        {
            await hangman.StartNewGame(Context.User, Context);
        }

        [Command("report")]
        public async Task ReportWord(int id, string reason = "undefinded")
        {
            var word = GuildStore.Words.FirstOrDefault(x => x.id == id);
            if (word.Reported)
            {
                await ReplyAsync($"the word: {word.word} is already reported.");
            }
            else
            {
                word.Reported = true;
                word.Issue = reason;
                word.ReportedBy = Context.UserId;
                word.ReportedOn = DateTimeOffset.UtcNow;
            }
        }

        [Command("top")]
        public async Task RecordUses()
        {
            var words = GuildStore.Words.ToList().OrderByDescending(x => x.used).ThenByDescending(x => x.word).Take(10);
            var sb = new StringBuilder();
            int lenght = 0;
            foreach (var item in words)
            {
                if (item.word.Length > lenght)
                    lenght = item.word.Length;
            }
            
            sb.AppendLine("```");
            for (int i = 0; i < 10; i++)
            {
                var item = words.ElementAt(i);
                
                var placeholder = item.word;
                for (int b = 0; placeholder.Length < lenght; b++)
                {
                    placeholder += ' ';
                }
                sb.AppendLine($"{placeholder} has {item.used} uses");
                
            }            
            sb.AppendLine("```");

            await ReplyAsync(sb.ToString());
        }
    }

}
