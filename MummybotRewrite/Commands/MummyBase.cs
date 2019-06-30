﻿using Discord;
using Mummybot.Database;
using Mummybot.Database.Entities;
using Mummybot.Services;
using Qmmands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Mummybot.Commands
{
    public class MummyBase : ModuleBase<MummyContext>
    {
        public MessageService MessageService { get; set; }
        public Guild GuildConfig { get; private set; }
        public GuildStore GuildStore { get; set; }
        public IServiceProvider Services { get; set; }

        protected Task<IUserMessage> ReplyAsync(string content = "", EmbedBuilder embed = null, string FileName = "", Stream Stream = null)
            => MessageService.SendAsync(Context, x => { x.Content = content; x.Embed = embed?.Build(); x.FileName = FileName; x.Stream = Stream; });

        protected override async Task BeforeExecutedAsync()
        {
            GuildConfig = await GuildStore.GetOrCreateGuildAsync(Context.Guild, x=> x.Stars);
        }

        protected override async Task AfterExecutedAsync()
        {
            GuildStore.Update(GuildConfig);
            await GuildStore.SaveChangesAsync();
        }
    }
}
