using System;
using System.Collections.Generic;
using System.Text;

namespace Mummybot.Database.Entities
{
    public class Prefixes
    {
        public ulong Id { get; set; }
        public Guild Guild { get; set; }
        public ulong guildID { get; set; }
        public string Prefix { get; set; }
    }
}
