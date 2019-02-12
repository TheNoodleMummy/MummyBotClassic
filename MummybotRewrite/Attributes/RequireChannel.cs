using System.Threading.Tasks;
using Qmmands;
using Discord.WebSocket;
using Discord;
using System.Collections.Generic;
using System.Linq;
using System;


namespace Mummybot.Commands.TypeReaders
{
    public class RequireChannelAttribute : CheckBaseAttribute
    {
        private string _channelName { get; set; }
        private ulong _ChannelId { get; set; }

        public RequireChannelAttribute(string channelname)
            => _channelName = channelname;
        public RequireChannelAttribute(ulong ID) => _ChannelId = ID;

        public override Task<CheckResult> CheckAsync(ICommandContext ctx, IServiceProvider provider)
        {
            var context = ctx as MummyContext;
            ITextChannel _channel;
            if (!string.IsNullOrEmpty(_channelName))
                _channel = context.Guild.TextChannels.FirstOrDefault(x => x.Name.ToLower() == _channelName.ToLower());
            else
                _channel = context.Guild.TextChannels.FirstOrDefault(x => x.Id == _ChannelId);


            if (_channel != null)
                return Task.FromResult(CheckResult.Successful);
            else
                return Task.FromResult(new CheckResult($"please only use this command in {(_channel as ITextChannel)?.Mention}"));
        }

        //public override async Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        //{
        //    ITextChannel _channel;
        //    if (!string.IsNullOrEmpty(_channelName))
        //        _channel = (await context.Guild.GetTextChannelsAsync()).FirstOrDefault(x => x.Name.ToLower() == _channelName.ToLower());
        //    else
        //        _channel = (await context.Guild.GetTextChannelsAsync()).FirstOrDefault(x => x.Id == _ChannelId);


        //    if (_channel != null)
        //        return PreconditionResult.FromSuccess();
        //    else
        //        return PreconditionResult.FromError($"please only use this command in {(_channel as ITextChannel)?.Mention}");
        //}

    }
}