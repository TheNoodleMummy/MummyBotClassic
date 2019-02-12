//using Discord;
//using Qmmands;
//using System;
//using System.Collections.Generic;
//using System.Collections.Immutable;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Mummybot.Commands.TypeReaders
//{
//    public class UserTypeReader<T> : TypeParser<IUser> where T :class,IUser
//    {
//        public override async Task<TypeParserResult<TypeReaderValue>> ParseAsync(string value, ICommandContext ctx, IServiceProvider provider)
//        {
//            var context = ctx as MummyContext;
//            var results = new Dictionary<ulong, TypeReaderValue>();
//            var  channelUsers = context.Channel.Users; // it's better
//            IReadOnlyCollection<IGuildUser> guildUsers = ImmutableArray.Create<IGuildUser>();

//            if (context.Guild != null)
//                guildUsers =  context.Guild.Users;

//            //By Mention (1.0)
//            if (MentionUtils.TryParseUser(value, out var id))
//            {
//                if (context.Guild != null)
//                    AddResult(results,  context.Guild.GetUser(id) as T, 1.00f);
//                else
//                    AddResult(results,  context.Channel.GetUser(id) as T, 1.00f);
//            }

//            //By Id (0.9)
//            if (ulong.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out id))
//            {
//                if (context.Guild != null)
//                    AddResult(results,  context.Guild.GetUser(id) as T, 0.90f);
//                else
//                    AddResult(results,  context.Channel.GetUser(id) as T, 0.90f);
//            }

//            //By Username + Discriminator (0.7-0.85)
//            int index = value.LastIndexOf('#');
//            if (index >= 0)
//            {
//                string username = value.Substring(0, index);
//                if (ushort.TryParse(value.Substring(index + 1), out ushort discriminator))
//                {
//                    var channelUser = channelUsers.FirstOrDefault(x => x.DiscriminatorValue == discriminator &&
//                        string.Equals(username, x.Username, StringComparison.OrdinalIgnoreCase));
//                    AddResult(results, channelUser as T, channelUser?.Username == username ? 0.85f : 0.75f);

//                    var guildUser = guildUsers.FirstOrDefault(x => x.DiscriminatorValue == discriminator &&
//                        string.Equals(username, x.Username, StringComparison.OrdinalIgnoreCase));
//                    AddResult(results, guildUser as T, guildUser?.Username == username ? 0.80f : 0.70f);
//                }
//            }

//            //By Username (0.5-0.6)
//            {
//                 channelUsers
//                    .Where(x => string.Equals(value, x.Username, StringComparison.OrdinalIgnoreCase))
//                    .ForEach(channelUser => AddResult(results, channelUser as T, channelUser.Username == value ? 0.65f : 0.55f))
//                    ;

//                foreach (var guildUser in guildUsers.Where(x => string.Equals(value, x.Username, StringComparison.OrdinalIgnoreCase)))
//                    AddResult(results, guildUser as T, guildUser.Username == value ? 0.60f : 0.50f);
//            }

//            //By Nickname (0.5-0.6)
//            {
//                await channelUsers
//                    .Where(x => string.Equals(value, (x as IGuildUser)?.Nickname, StringComparison.OrdinalIgnoreCase))
//                    .ForEachAsync(channelUser => AddResult(results, channelUser as T, (channelUser as IGuildUser).Nickname == value ? 0.65f : 0.55f))
//                    .ConfigureAwait(false);

//                foreach (var guildUser in guildUsers.Where(x => string.Equals(value, x.Nickname, StringComparison.OrdinalIgnoreCase)))
//                    AddResult(results, guildUser as T, guildUser.Nickname == value ? 0.60f : 0.50f);
//            }

//            if (results.Count > 0)
//                return new TypeParserResult<TypeReaderValue>()
//            return new TypeParserResult<TypeReaderValue>("User not found.");
//        }

//        private void AddResult(Dictionary<ulong, TypeReaderValue> results, T user, float score)
//        {
//            if (user != null && !results.ContainsKey(user.Id))
//                results.Add(user.Id, new TypeReaderValue(user, score));
//        }
//    }
//}
