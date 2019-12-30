using System;
using System.Collections.Generic;
using System.Text;

namespace Mummybot.Commands.TypeReaders
{
    class Difficulty
    {
        public static Difficulty Easy { get; internal set; }
        public static Difficulty Normal { get; internal set; }
        public static Difficulty Hard { get; internal set; }
        public static Difficulty Custom { get; internal set; }
    }
}
