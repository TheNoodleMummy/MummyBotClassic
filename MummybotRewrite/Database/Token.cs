using LiteDB;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mummybot.Database.Models
{
    public class TokenData
    {
        public string Token { get; set; }
        [BsonId]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Name { get; set; }
    }
}
