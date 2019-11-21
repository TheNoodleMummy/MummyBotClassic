using System;
using System.Collections.Generic;
using System.Text;

namespace Mummybot.Database.Entities
{
    public class DBWord
    {
        public int id { get; set; }
        public string word { get; set; }
        public string Issue { get; set; }
        public int used { get; set; }
        public bool Reported { get; set; }
        public ulong ReportedBy { get; set; } 
        public DateTimeOffset ReportedOn { get; set; }
    }
}
