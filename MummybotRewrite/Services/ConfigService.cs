using Mummybot.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mummybot.Services
{
    public static class ConfigService
    {
        public static string GetTokenDB()
        {
            return JsonConvert.DeserializeObject<ConnectionStrings>(File.ReadAllText("Connections.json")).TokenDB;
        }

        public static string GetRuntimeDB()
        {
            return JsonConvert.DeserializeObject<ConnectionStrings>(File.ReadAllText("Connections.json")).RuntimeDB;
        }
    }

    public class ConnectionStrings
    {
        public string TokenDB { get; set; } = "";
        public string RuntimeDB { get; set; } = "";
        public string DebugDB { get; set; } = "";
    }
}
