using Discord;
using Qmmands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Addons.Interactive;
using System.Linq;
using Discord.Rest;

//[RequireOwner,Name("Update Log")]
//public class UpdatelogModule : MummyInteractiveBase
//{
//    public UpdateBuilder _UpdateBuilder { get; set; }
    

//    [Command("bug",RunMode = RunMode.Async)]
//    public async Task ReportBug()
//    {
//        await ReplyAsync("title?");
//        var response = await NextMessageAsync(true, true, new TimeSpan(0, 0, 5, 0, 0));
//        await ReplyAsync("description");
//        var response1 = await NextMessageAsync(true, true, new TimeSpan(0, 0, 5, 0, 0));

//        EmbedBuilder emb = new EmbedBuilder().WithTitle(response.Content).WithDescription(response1.Content);

//        await Context.Guild.TextChannels.FirstOrDefault(x => x.Name.ToLower().Contains("bugs")).SendMessageAsync("", false, emb.Build());
        
//    }

//    [Command("update", RunMode = RunMode.Async)]
//    public async Task SetDescription()
//    {
//        try
//        {
            
//            _UpdateBuilder = new UpdateBuilder();
//            await ReplyAsync("yeey new update, whats the version/title? (editing doesnt work)");
//            var response = await NextMessageAsync(true,true,new TimeSpan(0,0,5,0,0));
//            _UpdateBuilder.WithTitle(response.Content);

//            await ReplyAsync("and what description does it have?");
//            var response2 = await NextMessageAsync(true, true, new TimeSpan(0, 0, 5, 0, 0));
//            _UpdateBuilder.WithDescrition(response2.Content);

//            await ReplyAsync("can i post this update now?");
//            var topostornot = (await NextMessageAsync(true, true, new TimeSpan(0, 0, 5, 0, 0))).Content;
//            if (topostornot.ToLower() == "yes")
//            {
//                await Context.Guild.TextChannels.FirstOrDefault(x => x.Name.ToLower().Contains("mummybot_update")).SendMessageAsync("", false, _UpdateBuilder.GetEmbed());
//            }
//        }
//        catch (Exception ex)
//        {

//            Log(LogSeverity.Error, LogSource.UpdateService, ex.ToString(), true);
//        }
//    }
//}

