using System;
using System.Collections.Generic;
using System.Text;

namespace Mummybot.Services
{
    public class SnowFlakeService
    {
        public uint sequence = 0;
        public DateTime epoch = new DateTime(2019, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc);//epoch of this snowflake generator 1 march 2019 0:00.0.0
        public DateTime lastegenerated;

        public ulong NewSnowflake()
        {

            return 0;
        }
    }
}
