using Discord;
using System.Linq;

namespace Mummybot
{
    public static partial class Utilities
    {
        public static string GetImageUrl(IMessage message)
        {
            string imageUrl = "";

            if (message.Embeds.FirstOrDefault() is IEmbed embed)
            {
                if (embed.Type == EmbedType.Image || embed.Type == EmbedType.Gifv)
                    imageUrl = embed.Url;
            }

            if (message.Attachments.FirstOrDefault() is IAttachment attachment)
            {
                var extensions = new[] { "png", "jpeg", "jpg", "gif", "webp" };

                if (extensions.Any(x => attachment.Url.EndsWith(x)))
                    imageUrl = attachment.Url;
            }

            return imageUrl;
        }
    }
}
