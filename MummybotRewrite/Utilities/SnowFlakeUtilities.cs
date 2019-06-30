//using Mummybot.Services;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Mummybot
//{
//    public static partial class Utilities
//    {
//        private static DateTime Epoch = new DateTime(2019, 6, 29, 0, 0, 0);
//        private static SnowFlakeGeneratorService InternalGenerator = new SnowFlakeGeneratorService(Extensions.GetWorkerId(), Epoch);
//        private static readonly object generateLock = new object();

//        public static ulong Generate()
//        {
//            lock (generateLock)
//            {
//                return InternalGenerator.NextLong();
//            }
//        }

//        public static DateTimeOffset FromSnowflake(ulong value)
//            => DateTimeOffset.FromUnixTimeMilliseconds((long)((value >> 22) + 1548979200000UL));

//        public static ulong ToSnowflake(DateTimeOffset value)
//            => ((ulong)value.ToUnixTimeMilliseconds() - 1548979200000UL) << 22;
//    }
//}
