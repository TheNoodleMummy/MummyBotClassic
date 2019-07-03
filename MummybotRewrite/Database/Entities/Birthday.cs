using System;
using System.Collections.Generic;
using System.Text;

namespace Mummybot.Database.Entities
{
    public class Birthday
    {
        public ulong GuildID { get; set; }
        public Guild Guild { get; set; }

        public ulong Id { get; set; }

        public DateTimeOffset BDay { get; set; }
        public DateTimeOffset NextBdayUTC { get; set; }
        public ulong UserId { get; set; }

        public override string ToString()
        {
            return $"{Id} at {BDay}";
        }
    }
}
