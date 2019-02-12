using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Espeon.Attributes;
using Espeon.Commands.Games;
using Espeon.Commands.ModuleBases;
using Espeon.Commands.Preconditions;
using Espeon.Commands.TypeReaders;
using Espeon.Core;
using Espeon.Services;
using Remarks = Espeon.Attributes.RemarksAttribute;

namespace Espeon.Commands.Modules
{
    [Name("Games")]
    [Summary("Some small games to play in the guild")]
    public class Games : EspeonBase
    {
        private readonly GamesService _games;
        private readonly CandyService _candy;
        private readonly Random _random;

        public Games(GamesService games, CandyService candy, Random random)
        {
            _games = games;
            _candy = candy;
            _random = random;
        }

        [Command("blackjack")]
        [Alias("bj")]
        [Summary("Start a game of blackjack")]
        [Name("Blackjack")]
        [Usage("blackjack 10")]
        [RequireGame(false)]
        public Task BlackJack(
            [Name("Amount To Bet")]
            [Summary("The amount of rare candies you want to bet.")]
            [Remarks("Don't specify for no bet")]
            [OverrideTypeReader(typeof(CandyTypeReader))] int amount = 0)
            => _games.StartGameAsync(Context.User.Id, new Blackjack(Context, Services, amount));

        [Command("coinflip")]
        [Alias("cf")]
        [Summary("Flip a coin")]
        [Name("Coin Flip")]
        [Usage("coinflip heads 100")]
        [RequireGame(false)]
        public async Task CoinFlip(
            [Name("Face")]
            [Summary("Heads or tails")] Face choice,
            [Name("Amount")]
            [Summary("The amount of candies you want to bet")]
            [OverrideTypeReader(typeof(CandyTypeReader))] int amount = 0)
        {
            var flip = _random.Next(100) > 50 ? Face.Heads : Face.Tails;

            await _candy.UpdateCandiesAsync(Context.User.Id, false, flip == choice ? (int)(0.5 * amount) : -amount);
            await SendMessageAsync($"It was {flip}! {(flip == choice ? "You win!" : "You lose!")}");
        }

        [Command("duel")]
        [Summary("Challenge someone to a duel")]
        [Name("Duel")]
        [Usage("Duel Espeon 100")]
        [RequireGame(false)]
        public Task Duel(
            [Name("User")]
            [Summary("The user you want to duel")] SocketGuildUser user,
            [Name("Amount")]
            [Summary("The amount you want to wager")]
            [OverrideTypeReader(typeof(CandyTypeReader))] int amount = 0)
            => _games.StartGameAsync(Context.User.Id, new Duel(Context, user, amount, Services));
    }
}
