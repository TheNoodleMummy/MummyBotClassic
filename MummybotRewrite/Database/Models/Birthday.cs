using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mummybot.Database.Models
{
    public class Birthday
    {
        public Birthday() { }

        public DateTime Bday { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong UserID { get; set; }


    }

}