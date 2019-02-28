//using Discord;
//using Qmmands;
//using Discord.WebSocket;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//[Name("Hangman"), Remarks("Will maybe support multiplayer"),RequireChannel(441765989512904706)]
//public class HangmanModule : MummyInteractiveBase
//{
//    public SendedMessagesService SendMsgService { get; set; }

//    [Command("hangman", RunMode = RunMode.Async)]
//    public async Task Hangman()
//    {
//        await Context.Message.DeleteAsync();
//        bool _CanGuessLetterTiwce;
//        bool _GuessWordByletter; // prevent the user to Guess the word and has to completly find the word letter per letter
//        bool _CanSeeGuessedWords = false; ;//makes the guessedword list public
//        bool _CanSeeGuessedLetters;


//        first:
//        SendMsgService.Add(MessageSource.Hangman, await ReplyAsync("Do you want me to strike you if you guess a single **letter** twice? (y/n)"));
//        var msg = (await NextMessageAsync() as SocketUserMessage);
//        SendMsgService.Add(MessageSource.Hangman, msg);
//        if (msg.Content.ToLower() == "y" || msg.Content.ToLower() == "n")
//        {
//            if (msg.Content.ToLower() == "y")
//                _CanGuessLetterTiwce = true;
//            else
//                _CanGuessLetterTiwce = false;

//        }
//        else
//            goto first;

//        second:
//        SendMsgService.Add(MessageSource.Hangman, await ReplyAsync("Do you want to guess the word by only letters?(no word guesses, no points lost if you would) (y/n)"));
//        var msg2 = (await NextMessageAsync() as SocketUserMessage);
//        SendMsgService.Add(MessageSource.Hangman, msg2);
//        if (msg.Content.ToLower() == "y" || msg.Content.ToLower() == "n")
//        {
//            if (msg2.Content.ToLower() == "y")
//                _GuessWordByletter = true;
//            else
//                _GuessWordByletter = false;

//        }
//        else
//            goto second;


//        third:
//        SendMsgService.Add(MessageSource.Hangman, await ReplyAsync("Do you want me to show the letters you have guessed wrong? (y/n)"));
//        var msg4 = (await NextMessageAsync() as SocketUserMessage);
//        SendMsgService.Add(MessageSource.Hangman, msg4);
//        if (msg.Content.ToLower() == "y" || msg.Content.ToLower() == "n")
//        {
//            if (msg4.Content.ToLower() == "y")
//                _CanSeeGuessedLetters = true;
//            else
//                _CanSeeGuessedLetters = false;
//        }
//        else
//            goto third;


//        if (!_GuessWordByletter)
//        {
//            forth:
//            SendMsgService.Add(MessageSource.Hangman, await ReplyAsync("Do you want me to show the last 10 guessed words? (y/n)"));
//            var msg5 = (await NextMessageAsync() as SocketUserMessage);
//            SendMsgService.Add(MessageSource.Hangman, msg5);
//            if (msg.Content.ToLower() == "y" || msg.Content.ToLower() == "n")
//            {
//                if (msg5.Content.ToLower() == "y")
//                    _CanSeeGuessedWords = true;
//                else
//                    _CanSeeGuessedWords = false;
//            }
//            else
//                goto forth;
//        }

//        numba:
//        SendMsgService.Add(MessageSource.Hangman, await ReplyAsync($"whats the mex lenght of words you wanne play with (current longest word is {Database.HangManWords.OrderBy(x => x.Word.Length).Last().Word.Length})"));
//        var msg6 = (await NextMessageAsync() as SocketUserMessage);
//        SendMsgService.Add(MessageSource.Hangman, msg6);
//        if (int.TryParse(msg6.Content, out int numba))
//        {
//            await SendMsgService.Delete(MessageSource.Hangman);

//            var textchannels = Context.Guild.Channels.Where(x => x is ITextChannel);
//            await ReplyAsync("ok that was all ready to begin?");
//            var hangman = new HangmanSingleService(Context.Client, (Context.Channel as SocketTextChannel), Context.User, _CanGuessLetterTiwce, _GuessWordByletter, _CanSeeGuessedWords, _CanSeeGuessedLetters);
//            //var hangman = new HangmanSingleService(Context.Client, (Context.Channel as SocketTextChannel), Context.User, true, false, true, true);

//            await hangman.StartGameAsync(2, Context.User.Id, Context.Message.Id);
//            hangman = null;
//        }
//        else
//            goto numba;
        



//    }


//    [Command("reportword", RunMode = RunMode.Async)]
//    public async Task Reportasync(int wordid)
//    {
//        var idk = Database.HangManWords.FirstOrDefault(x => x.ID == wordid);
//        if (idk != null)
//            await (await Context.Client.GetApplicationInfoAsync()).Owner.SendMessageAsync($"{Context.User} reported {idk.Word}/{idk.ID}");
//        else
//            await ReplyAsync("goof that id doesnt exist");
//    }
//}

    


