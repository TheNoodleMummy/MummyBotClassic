using System;
using System.Collections.Generic;
using System.Text;

namespace Mummybot.Database.Entities
{
    public class Star
    {
        public Guild Guild { get; set; }

        public int Id { get; set; }
        public ulong GuildID { get; set; }
        public ulong MessageId { get; set; }
        public ulong StarboardMessageId { get; set; }
        public int Stars { get; set; } = 1;
    }
}
