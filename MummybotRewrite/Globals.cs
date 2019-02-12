using Discord;
using Mummybot.Commands;
using Mummybot.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot
{
    public class Globals
    {
        public MummyContext Context { get; set; }
        public DBService Db { get; set; }
        public MessagesService Messages { get; set; }
        public async Task<IUserMessage> ReplyAsync(string message = null, bool isTTS = false, Embed embed = null, RequestOptions options = null)
        {
            return await Messages.SendMessageAsync(Context, message, isTTS, embed).ConfigureAwait(false);
        }
    }
}
