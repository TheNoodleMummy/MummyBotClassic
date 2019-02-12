﻿using SharpLink;
using System.Collections.Concurrent;

namespace Espeon.Core.Entities
{
    public class LavalinkObject
    {
        public LavalinkPlayer Player { get; set; }
        public bool IsPaused { get; set; }
        public ulong ChannelId { get; set; }
        public ulong UserId { get; set; }
        public ConcurrentQueue<LavalinkTrack> Queue { get; set; }
    }
}
