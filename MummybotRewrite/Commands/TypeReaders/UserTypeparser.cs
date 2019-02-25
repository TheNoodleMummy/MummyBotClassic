using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Qmmands;

namespace Mummybot.Commands.TypeReaders
{
    [DoNotAutoAdd]
     class UserTypeparser<TUser> : TypeParser<TUser> where TUser : SocketGuildUser
    {
        public override Task<TypeParserResult<TUser>> ParseAsync(Parameter param, string value, ICommandContext context, IServiceProvider provider)
        {
            var ctx = (MummyContext)context;
            var type = typeof(TUser);
            List<TUser> users;
            users = ctx.Guild.Users.OfType<TUser>().ToList();

            TUser user = null;

            if (ulong.TryParse(value, out var id) || MentionUtils.TryParseUser(value, out id))
                user = users.FirstOrDefault(x => x.Id == id);

            if (user is null) user = users.FirstOrDefault(x => x.ToString().Equals(value,StringComparison.OrdinalIgnoreCase));

            if (user is null)
            {
                var match = users.Where(x =>
                    x.Username.Equals(value, StringComparison.OrdinalIgnoreCase)
                    || (x as SocketGuildUser).Nickname.Equals(value, StringComparison.OrdinalIgnoreCase)).ToList();
                if (match.Count > 1)
                    return Task.FromResult(TypeParserResult<TUser>.Unsuccessful(
                        "Multiple users found, try mentioning the user or using their ID.")
                    );

                user = match.FirstOrDefault();
            }

            return user is null
                ? Task.FromResult(TypeParserResult<TUser>.Unsuccessful("User not found."))
                : Task.FromResult(TypeParserResult<TUser>.Successful(user));
        }
    }
}