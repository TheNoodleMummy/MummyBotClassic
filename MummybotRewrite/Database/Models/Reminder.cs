using Discord.WebSocket;
using Mummybot.interfaces;
using Mummybot.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Database.Models

{
    public class Reminder : IRemoveable
    {
        [NotMapped]
        private readonly IRemoveableService _service;
        public Reminder() { }
        public Reminder(IRemoveableService service)
        {
            _service = service;
        }
        public Reminder(Reminder reminder, IRemoveableService service)
        {
            Message = reminder.Message;
            Guildid = reminder.Guildid;
            ChannelID = reminder.ChannelID;
            UserID = reminder.UserID;
            Identifier = reminder.Identifier;
            When = reminder.When;
            JumpUrl = reminder.JumpUrl;
            _service = service;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Identifier { get; set; }
        public ulong Guildid { get; set; }
        public ulong UserID { get; set; }
        public ulong ChannelID { get; set; }
        public string Message { get; set; }
        public DateTime SetOnUTC { get; set; }        
        public DateTime When { get; set; }
        public string JumpUrl { get; set; }
       
        public Task RemoveAsync()
            => _service.RemoveAsync(this);
    }
}