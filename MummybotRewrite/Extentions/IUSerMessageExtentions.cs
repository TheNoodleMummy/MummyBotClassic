using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Extentions
{
    public partial class Extentions
    {
        public static  Task AddOkAsync(this IUserMessage message)
            => message.AddReactionAsync(new Emoji("👌"));

        public static Task AddNotOkAsync(this IUserMessage message)
            => message.AddReactionAsync(new Emoji("⛔"));

    }
}
