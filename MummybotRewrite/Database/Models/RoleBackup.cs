using System.ComponentModel.DataAnnotations;

namespace Mummybot.Database.Models
{
    public class RoleBackup
    {
        [Key]
        public ulong RoleID { get; set; }

        public string Name { get; set; }


        public RoleBackup() { }

        public RoleBackup(ulong id, string name)
        {
            RoleID = id;
            Name = name;
        }
    }
}
