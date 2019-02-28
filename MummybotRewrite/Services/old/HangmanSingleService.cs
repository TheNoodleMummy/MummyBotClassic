//using Discord;
//using Discord.Addons.Interactive;
//using Discord.WebSocket;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Mummybot.Database.Models;
//using Mummybot.Database;

//class HangmanSingleService 
//{
//    public List<string> HangmanArt = new List<string>()
//    {
//        "\u200b                           \u200b\n"+//step 0
//        "\u200b  \n"+
//        "\u200b  \n"+
//        "\u200b  \n"+
//        "\u200b  \n"+
//        "\u200b  \n"+
//        "\u200b  \n"+
//        "\u200b  \n"+
//        "\u200b  \n"+
//        "\u200b  \n"+
//        "\u200b  \n"+
//        "\u200b  \n"+
//        "\u200b  \n"+
//        "\u200b  \n"+
//        "\u200b  \n"+
//        "\u200b  \n"+
//        "\u200b  \n"+
//        "\u200b  \n"+
//        "\u200b  \n"+
//        "\u200b  \n"+
//        "\u200b  \n"+
//        "\u200b  \n"+
//        "\u200b  \n"+
//        "",

//        "            \u200B               \n" +//step 1
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "| |\n" +
//        ": :\n"+
//        ". .",

//        "            \u200B               \n" +//step 2
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "| |                     | |\n" +
//        "| |                     | |\n"+
//        ": :                     : : \n"+
//        ". .                     . .",

//        "            \u200B               \n" +//step3
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        "\n"+
//        " _________________________\n" +
//        "|  _____________________  |\n"+
//        "| |                     | |\n" +
//        "| |                     | |\n"+
//        ": :                     : : \n"+
//        ". .                     . .",

//        "\n"+
//        " _\n" + //step 4
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |_______________________\n" +
//        "|  _____________________  |\n"+
//        "| |                     | |\n" +
//        "| |                     | |\n"+
//        ": :                     : : \n"+
//        ". .                     . .",

//        "\n"+
//        " ____________________\n" +//step 5
//        "| .__________________|\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |_______________________\n" +
//        "|  _____________________  |\n"+
//        "| |                     | |\n" +
//        "| |                     | |\n"+
//        ": :                     : : \n"+
//        ". .                     . .",

//        "\n"+
//        " ____________________\n" +//step 6
//        "| .__________________|\n" +
//        "| | / /\n" +
//        "| |/ /\n" +
//        "| | /\n" +
//        "| |/\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |_______________________\n" +
//        "|  _____________________  |\n"+
//        "| |                     | |\n" +
//        "| |                     | |\n"+
//        ": :                     : : \n"+
//        ". .                     . .",

//        "\n"+
//        " ___________.._______\n" + //step 7
//        "| .__________))______|\n" +
//        "| | / /      ||\n" +
//        "| |/ /       ||\n" +
//        "| | /        ||\n" +
//        "| |/         ||\n" +
//        "| |          ||\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |_______________________\n" +
//        "|  _____________________  |\n"+
//        "| |                     | |\n" +
//        "| |                     | |\n"+
//        ": :                     : : \n"+
//        ". .                     . .",

//        "\n"+
//        " ___________.._______\n" +//step 8
//        "| .__________))______|\n" +
//        "| | / /      ||\n" +
//        "| |/ /       ||\n" +
//        "| | /        ||.-''.\n" +
//        "| |/         |/  _  \\\n" +
//        "| |          ||  `/,|\n" +
//        "| |          (\\\\`_.'\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |_______________________\n" +
//        "|  _____________________  |\n"+
//        "| |                     | |\n" +
//        "| |                     | |\n"+
//        ": :                     : : \n"+
//        ". .                     . .",

//        "\n"+
//        " ___________.._______\n" +//step 9
//        "| .__________))______|\n" +
//        "| | / /      ||\n" +
//        "| |/ /       ||\n" +
//        "| | /        ||.-''.\n" +
//        "| |/         |/  _  \\\n" +
//        "| |          ||  `/,|\n" +
//        "| |          (\\\\`_.'\n" +
//        "| |         .-`--'.\n" +
//        "| |          |. .|\n" +
//        "| |          |   |\n" +
//        "| |          | . |\n" +
//        "| |          |___|\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |_______________________\n" +
//        "|  _____________________  |\n"+
//        "| |                     | |\n" +
//        "| |                     | |\n"+
//        ": :                     : : \n"+
//        ". .                     . .",

//        "\n"+
//        " ___________.._______\n" +//step 10
//        "| .__________))______|\n" +
//        "| | / /      ||\n" +
//        "| |/ /       ||\n" +
//        "| | /        ||.-''.\n" +
//        "| |/         |/  _  \\\n" +
//        "| |          ||  `/,|\n" +
//        "| |          (\\\\`_.'\n" +
//        "| |         .-`--'.\n" +
//        "| |        /_ . . _\n" +
//        "| |       // |   |\n" +
//        "| |      //  | . |\n" +
//        "| |     ')   |___|\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |_______________________\n" +
//        "|  _____________________  |\n"+
//        "| |                     | |\n" +
//        "| |                     | |\n"+
//        ": :                     : : \n"+
//        ". .                     . .",

//        "\n"+
//         " ___________.._______\n" +//step 11
//        "| .__________))______|\n" +
//        "| | / /      ||\n" +
//        "| |/ /       ||\n" +
//        "| | /        ||.-''.\n" +
//        "| |/         |/  _  \\\n" +
//        "| |          ||  `/,|\n" +
//        "| |          (\\\\`_.'\n" +
//        "| |         .-`--'.\n" +
//        "| |        /_ . . _\\\n" +
//        "| |       // |   | \\\\\n" +
//        "| |      //  | . |  \\\\\n" +
//        "| |     ')   |___|   (`\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |\n" +
//        "| |_______________________\n" +
//        "|  _____________________  |\n"+
//        "| |                     | |\n" +
//        "| |                     | |\n"+
//        ": :                     : : \n"+
//        ". .                     . .",

//        "\n"+
//        " ___________.._______\n" +//step 12
//        "| .__________))______|\n" +
//        "| | / /      ||\n" +
//        "| |/ /       ||\n" +
//        "| | /        ||.-''.\n" +
//        "| |/         |/  _  \\\n" +
//        "| |          ||  `/,|\n" +
//        "| |          (\\\\`_.'\n" +
//        "| |         .-`--'.\n" +
//        "| |        /_ . . _\\\n" +
//        "| |       // |   | \\\\\n" +
//        "| |      //  | . |  \\\\\n" +
//        "| |     ')   | __|   (`\n" +
//        "| |          ||\n" +
//        "| |          ||\n" +
//        "| |          ||\n" +
//        "| |          ||\n" +
//        "| |_________/_|___________\n" +
//        "|  _____________________  |\n"+
//        "| |                     | |\n" +
//        "| |                     | |\n"+
//        ": :                     : : \n"+
//        ". .                     . .",

//        "\n"+
//        " ___________.._______\n" +//step 13
//        "| .__________))______|\n" +
//        "| | / /      ||\n" +
//        "| |/ /       ||\n" +
//        "| | /        ||.-''.\n" +
//        "| |/         |/  _  \\\n" +
//        "| |          ||  `/,|\n" +
//        "| |          (\\\\`_.'\n" +
//        "| |         .-`--'.\n" +
//        "| |        /_ . . _\\\n" +
//        "| |       // |   | \\\\\n" +
//        "| |      //  | . |  \\\\\n" +
//        "| |     ')   |   |   (`\n" +
//        "| |          ||'||\n" +
//        "| |          || ||\n" +
//        "| |          || ||\n" +
//        "| |          || ||\n" +
//        "| |_________/_|_|_\\_______\n" +
//        "|  _____________________  |\n"+
//        "| |                     | |\n" +
//        "| |                     | |\n"+
//        ": :                     : : \n"+
//        ". .                     . .",

//        "\n"+
//        " ___________.._______\n" +//step 14 your dead
//        "| .__________))______|\n" +
//        "| | / /      ||\n" +
//        "| |/ /       ||\n" +
//        "| | /        ||.-''.\n" +
//        "| |/         |/  _  \\\n" +
//        "| |          ||  `/,|\n" +
//        "| |          (\\\\`_.'\n" +
//        "| |         .-`--'.\n" +
//        "| |        /_ . . _\\\n" +
//        "| |       // |   | \\\\\n" +
//        "| |      //  | . |  \\\\\n" +
//        "| |     ')   |   |   (`\n" +
//        "| |          ||'||\n" +
//        "| |          || ||\n" +
//        "| |          || ||\n" +
//        "| |          || ||\n" +
//        "| |________ / | | \\   _____\n" +
//        "|  ________|\\`-' `-' |_____|\n"+
//        "| |       \\ \\           | |\n" +
//        "| |        \\ \\          | |\n"+
//        ": :         \\ \\         : :\n"+
//        ". .          `'         . ." };

//    public string title = " _                                             \n" +
//                "| |\n" +
//                "| | __   __ _ _ __   __ _ _ __ ___ __ _ _ __  \n" +
//                "| '_  \\/ _` | '_  \\/ _` | '_ ` _  \\/ _` | '_ \\ \n" +
//                "| | | | (_| | | | | (_| | | | | | | (_| | | | |\n" +
//                "|_| |_|\\__,_|_| |_|\\__, |_| |_| |_|\\__,_|_| |_|\n" +
//                "                    __/ |                      \n" +
//                "                   | ___/";
//    private readonly IDiscordClient _Client;
//    private MummyContext Database;

//    private readonly IUser _Player;
//    private IMessageChannel _Channel;
//    private IUserMessage _Message;
//    private readonly bool _CanGuessLetterTwice = false; //prevents the user to get a strike if he Guess the same letter twice
//    //private bool _CanGuessWordsTwice = false; //prevents the user to get a strike is he Guesses the same word twice
//    private readonly bool _GuessWordByletter = false; // prevent the user to Guess the word and has to completly find the word letter per letter
//    private readonly bool _CanSeeGuessedWords = true;//makes the guessedword list public
//    private readonly bool _CanSeeGuessedLetters = true;
//    private TimeSpan _DefaultTimeout => TimeSpan.FromSeconds(15);

//    public HangmanWord TheWord { get; set; }
//    private int WrongGuesses = 0;
//    private int GoodGuesses = 0;
//    private List<string> GuessedWords = new List<string>();
//    private List<char> GuessedLetters = new List<char>();
//    private List<char> CorrectGuessedLetters = new List<char>();
//    private bool HasGuessedWord=false;

//    public HangmanSingleService(IDiscordClient client, ITextChannel channel, IUser user, bool letterGuessing,bool GuessByLetter = false, bool CanSeeWords = true, bool CanSeeLetters = true)
//    {
        
//        _Client = client;
//        (client as DiscordSocketClient).MessageReceived += HangmanSingleService_MessageReceived;
//        _Channel = channel;
//        _Player = user;
//        _CanGuessLetterTwice = letterGuessing;
//        _GuessWordByletter = GuessByLetter;
//        _CanSeeGuessedWords = CanSeeWords;
//        _CanSeeGuessedLetters = CanSeeLetters;
//    }

//    private async Task HangmanSingleService_MessageReceived(SocketMessage msg)
//    {
//        _ = Task.Run(async () =>
//        {
//            if (msg.Channel.Id != _Channel?.Id) return;
//            if (msg.Author.IsBot||msg.Author.IsWebhook) return;
//            if (msg.Author.Id != _Player?.Id)
//            {
//                await ReplyAndDeleteAsync("Mate your not playing this game go away", timeout: TimeSpan.FromSeconds(5));
//                return;
//            }
//            //dubbel rade van letter is borked
//            var Content = msg.Content.ToLower();
//            await msg.DeleteAsync();

//            if (GetWordState() == TheWord.Word)
//                goto finish;
            
//            if (Content.Length != 1)
//            if (Content == TheWord.Word)
//            {
//                HasGuessedWord = true;
//                goto finish;
//            }
//            else
//            {
//                GuessedWords.Add(Content);
//                WrongGuesses++;
//                goto finish;
//            }


//            if (Content.Length == 1)
//            {
//                char.TryParse(Content, out char letter);
//                await CheckLetterAsync(letter);
//            }

//            finish:       
            
//            if (GetWordState() == TheWord.Word||HasGuessedWord)
//            {
//                await _Message.ModifyAsync(x => x.Embed = GetEndResult(true));
//                return;
//            }
//            else if (WrongGuesses >= HangmanArt.Count-1)
//            {
//                await _Message.ModifyAsync(x => x.Embed = GetEndResult(false));
//                return;
//            }
//            else
//            await _Message.ModifyAsync(x => x.Embed = GetGameState());


//        });
//        await Task.CompletedTask;
//    }

//    private async Task CheckLetterAsync(char letter)
//    {
//        if (TheWord.Word.Contains(letter))
//        {
//            if (!CorrectGuessedLetters.Contains(letter))
//                CorrectGuessedLetters.Add(letter);
//            await ReplyAndDeleteAsync("Correct!", timeout: TimeSpan.FromSeconds(2));
//            GoodGuesses++;
//            return;
//        }

//        if (_CanGuessLetterTwice && !TheWord.Word.Contains(letter))
//        {
//            if (GuessedLetters.Contains(letter)&&!_CanGuessLetterTwice)
//            {
//                WrongGuesses++;
//            }
//            else
//            {
//                GuessedLetters.Add(letter);
//                WrongGuesses++;
//            }
           
//            return;
//        }
        
        
//        return;
//    }




//    public Embed GetEndResult(bool Won)
//    {
//        var emb = new EmbedBuilder().WithTitle("Hangman");
//        if (Won)
//        {
//            if (GoodGuesses==0)
//                 GoodGuesses = 1;
            
//            emb.Description = "You won!\n" +
//           $"you needed {GoodGuesses+WrongGuesses} attempts to guess the word.";
//            emb.AddField("The Word was", TheWord.Word, true);
//        }
//        else
//        {
//            emb.WithTitle($"You lost. wordID: {TheWord.ID}");
//            emb.WithDescription($"```\n{HangmanArt.Last()}\n```");
//        }
        
//        (_Client as DiscordSocketClient).MessageReceived -= HangmanSingleService_MessageReceived;
//        return emb.Build();
//    }

//    public Embed GetGameState()
//    {
//        var emb = new EmbedBuilder().WithTitle("Hangman");
//        emb.AddField("The Word", $"``{GetWordState()} {TheWord.ID}``", false);
//        emb.AddField("Wrong guesses", $"{WrongGuesses}/{HangmanArt.Count - 1}");

//        if (_CanSeeGuessedWords)
//            emb.AddField("Guessed Words", GetLastGuessed10(),true);
//        if (_CanGuessLetterTwice||_CanSeeGuessedLetters)
//            emb.AddField("Guessed Letters", GetGuessedLetters(),true);
        
//        emb.AddField("Options", GetOptions(),true);
//        emb.WithDescription($"```\n{HangmanArt[WrongGuesses]}\n```");
//        emb.WithFooter("if the word was not to your liking remember the WordID and you can report this the the word,it might get removed or public vote to keep/remove");

//        return emb.Build();
//    }

//    public async Task StartGameAsync(int wordlenght,ulong userid,ulong msgid)
//    { 
//        _Message = await _Channel.SendMessageAsync("Lets begin.");
        

//        var posiblewords = Database.HangManWords.OrderBy(x => x.Word.Length).Where(x => x.Word.Length <= wordlenght).ToList();
//        TheWord = posiblewords[new Random((int)(userid / msgid)).Next(0,posiblewords.Count)];

//await _Message.ModifyAsync(x => x.Embed = GetGameState());
//    }
    
//    public string GetWordState()
//    {
//        string wordtoreturn = "";
//        foreach (char Char in TheWord.Word)
//        {
//            if (CorrectGuessedLetters.Contains(Char))
//            {
//                wordtoreturn += Char.ToString();
//            }
//            else
//            {
//                wordtoreturn += "*";
//            }
//        }
//        return wordtoreturn;
//    }






    
//    public string GetLastGuessed10()
//    {
//        var sb = new StringBuilder();
//        for (int i = 0; i < 9; i++)
//        {


//            if (GuessedWords.Count != 0)
//            {
//                if (i == 0)
//                {
//                    sb.AppendLine(GuessedWords.First());

//                }
//                else if (GuessedWords.Count > i)
//                {
//                    sb.AppendLine(GuessedWords[GuessedWords.Count - i]);

//                }
//            }
//        }
//        if (sb.ToString()=="")
//        {
//            sb.Append("\u200B");
//        }
//        return sb.ToString();
//    }
//    public string GetGuessedLetters()
//    {
//        var sb = new StringBuilder();
//        foreach (char Letter in GuessedLetters)
//        {
//            sb.AppendLine(Letter.ToString());
//        }
//        if (sb.ToString() == "")
//        {
//            sb.Append("\u200B");
//        }
//        return sb.ToString();
//    }
//    public string GetOptions()
//    {
//        var sb = new StringBuilder();
//        string CanGuessLetterTiwce  = _CanGuessLetterTwice  ? "yes" : "no";
//        string CanSeeGuessedLetters = _CanSeeGuessedLetters ? "yes" : "no";
//        string CanSeeGuessedWords   = _CanSeeGuessedWords   ? "yes" : "no";


//        sb.AppendLine($"Double Guessed LetterStrike: {CanGuessLetterTiwce}");
//        sb.AppendLine($"Can See Last 10 Guessed Words: {CanSeeGuessedWords}");
//        sb.AppendLine($"Can See Guessed Letters: {CanSeeGuessedLetters}");


//        return sb.ToString();
//    }
//    public async Task<IUserMessage> ReplyAndDeleteAsync(string content, bool isTTS = false, Embed embed = null, TimeSpan? timeout = null, RequestOptions options = null)
//    {
//        timeout = timeout ?? _DefaultTimeout;
//        var message = await _Channel.SendMessageAsync(content, isTTS, embed, options).ConfigureAwait(false);
//        _ = Task.Delay(timeout.Value)
//            .ContinueWith(_ => message.DeleteAsync().ConfigureAwait(false))
//            .ConfigureAwait(false);
//        return message;
//    }
//}

//public static class StringExtends
//{
//    public static IEnumerable<int> AllIndexesOf(this string str, string searchstring)
//    {
//        int minIndex = str.IndexOf(searchstring);
//        while (minIndex != -1)
//        {
//            yield return minIndex;
//            minIndex = str.IndexOf(searchstring, minIndex + searchstring.Length);
//        }
//    }
//}
