using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mummybot.Database.Models
{
    public class BlackList
    {

        public BlackList() { }

        public BlackList(ulong userid,string reason)
        {
            UserID = userid;
            Reason = reason;
            DateBlocked = DateTime.UtcNow;
        }
        [NotMapped]
        public ulong UserID { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }        
        

        public DateTime DateBlocked { get; set; }

        public string Reason { get; set; }
    }
}