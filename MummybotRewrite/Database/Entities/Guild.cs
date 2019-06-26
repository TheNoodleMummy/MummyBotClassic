using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Mummybot.Database.Entities
{
    public class Guild
    {
        public List<Prefixes> Prefixes { get; set; } = new List<Prefixes>();
        public ulong GuildID { get; set; }
        public bool AutoQuotes { get; set; }
    }
}
