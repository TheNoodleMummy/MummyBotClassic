using Discord;
using Discord.WebSocket;
using Mummybot.Attributes;
using Qmmands;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Commands.TypeReaders
{
        [DontAutoAdd]
        public class UserTypeparser<TUser> : MummyTypeParser<TUser> where TUser : SocketGuildUser
        {
            public override ValueTask<TypeParserResult<TUser>> ParseAsync(Parameter param, string value, MummyContext ctx)
            {
                var type = typeof(TUser);
                List<TUser> users;
                users = ctx.Guild.Users.OfType<TUser>().ToList();

                TUser user = null;

                if (ulong.TryParse(value, out var id) || MentionUtils.TryParseUser(value, out id))
                    user = users.Find(x => x.Id == id);

                if (user is null) user = users.Find(x => x.ToString().Equals(value, StringComparison.OrdinalIgnoreCase));

                if (user is null)
                {
                    var match = users.Where(x =>
                        x.Username.Equals(value, StringComparison.OrdinalIgnoreCase)
                        || x.Nickname != null && x.Nickname.Equals(value, StringComparison.OrdinalIgnoreCase)).ToList();
                if (match.Count > 1)
                    return TypeParserResult<TUser>.Failed(
                        "Multiple users found, try mentioning the user or using their ID.");

                    user = match.FirstOrDefault();
                }

                return user is null
                    ? TypeParserResult<TUser>.Failed("User not found.")
                    : TypeParserResult<TUser>.Successful(user);
            }
        }
    }
