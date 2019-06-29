using Discord;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mummybot
{
    public static partial class Utilities
    {
        public static string GetDisplayName(this IUser user)
        {
            return (user as IGuildUser)?.Nickname ?? user.Username;
        }

        public static string GetAvatarOrDefaultUrl(this IUser user)
        {
            return user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl();
        }
    }
}
