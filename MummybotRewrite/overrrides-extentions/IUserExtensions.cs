using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mummybot.overrrides_extentions
{
    public static class IUserExtensions
    {

        public static string GetName(this IUser user)
            =>(user as SocketGuildUser).Nickname ?? user.Username;
        
    }
}
