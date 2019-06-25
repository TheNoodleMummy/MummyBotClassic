using Discord;
using Discord.WebSocket;
using Humanizer;
using System;
using System.Threading.Tasks;

namespace Mummybot
{
    public static partial class Utilities
    {
        public static Task<Embed> QuoteFromString(DiscordSocketClient client, string content)
        {
            var split = content.Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (split.Length != 6 || !ulong.TryParse(split[4], out var id) ||
                !(client.GetChannel(id) is ITextChannel channel) || !ulong.TryParse(split[5], out id))
                return Task.FromResult<Embed>(null);

            return QuoteFromMessageId(channel, id);
        }

        public static async Task<Embed> QuoteFromMessageId(ITextChannel channel, ulong id)
        {
            var message = await channel.GetMessageAsync(id);

            if (message is null)
                return null;

            var imageUrl = GetImageUrl(message);

            var builder = BaseEmbed(message.Author, imageUrl, message.Content)
                .AddField("\u200b", $"Sent {message.CreatedAt.Humanize()} " +
                                    $"in {channel.Guild.Name} / <#{message.Channel.Id}> /" +
                                    $"{Format.Url($"{message.Id}", message.GetJumpUrl())}");

            return builder.Build();
        }

        private static EmbedBuilder BaseEmbed(IUser author, string imageUrl, string content)
        {
            return new EmbedBuilder
            {
                Author = new EmbedAuthorBuilder
                {
                    IconUrl = author.GetAvatarOrDefaultUrl(),
                    Name = (author as IGuildUser)?.GetDisplayName() ?? author.Username
                },
                ImageUrl = imageUrl,
                Description = content,
            };
        }
    }
}
