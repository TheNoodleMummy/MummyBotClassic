using System;

namespace Mummybot.Entities
{
    class Message
    {
        public ulong UserId { get; set; }
        public ulong TriggerID { get; set; }
        public ulong ResponseId { get; set; }
        public ulong ChannelId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
