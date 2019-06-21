using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Mummybot.Database.Models

{
    public class Tag
    {
        public Tag() { }
        public Tag(ulong userid, string username, string value, string key, ulong GuildID = 0, bool ReqNSFW = false)
        {

            UserID = userid;
            Name = username;
            Value = value;
            Key = key;
            this.GuildID = GuildID;
            Req_NSFW = ReqNSFW;


        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public int Id { get; set; }


        public ulong UserID { get; set; }


        public ulong GuildID { get; set; }

        public string Name { get; set; }
        public string Value { get; set; }

        [Required]
        public string Key { get; set; }
        public int Uses { get; set; } = 0;

        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        public bool Req_NSFW { get; set; } = false;

    }
}