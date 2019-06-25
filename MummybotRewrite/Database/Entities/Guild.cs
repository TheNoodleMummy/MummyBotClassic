using System;
using System.Collections.Generic;
using System.Text;

namespace Mummybot.Database.Entities
{
    public class Guild
    {
        public List<string> Prefixes { get; set; }
        public ulong GuildID { get; set; }
        public bool AutoQuotes { get; set; }
    }
}
