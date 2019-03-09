using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mummybot.Database.Models

{
    public class Star
    {
        public Star() { }//required by EF Core

        public int Stars { get; set; }


        public ulong StaredMessageChannelID { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public ulong StaredMessageId { get; set; }


        public ulong StartboardMessageId { get; set; }
    }
}