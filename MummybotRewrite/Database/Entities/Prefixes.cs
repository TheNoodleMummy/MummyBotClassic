using System;
using System.Collections.Generic;
using System.Text;

namespace Mummybot.Database.Entities
{
    public class Prefixes
    {

        public Prefixes()
        {

        }

        public Prefixes(ulong id, Guild guild, ulong guildID, string prefix)
        {
            Id = id;
            Guild = guild;
            this.guildID = guildID;
            Prefix = prefix;
        }

        public ulong Id { get; set; }
        public Guild Guild { get; set; }
        public ulong guildID { get; set; }
        public string Prefix { get; set; }
    }
}
