using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Qmmands;
using Discord.WebSocket;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using Mummybot.Attributes;
using Mummybot.Commands.TypeReaders;
using Mummybot.Database.Models;
using Victoria;
using Mummybot.Extentions;
using System.Diagnostics;

namespace Mummybot.Services
{
    class StartupService
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;
        private readonly EventsService _eventService;
        public StartupService(IServiceProvider provider)
        {
            _client = provider.GetService<DiscordSocketClient>();
            _commands = provider.GetService<CommandService>();
            _services = provider;
            _eventService = provider.GetService<EventsService>();
        }

        public async Task StartAsync()
        {
            _eventService.HookEvents();
            InstallCommandAsync();
            await StartClientAsync();
            await Task.Delay(-1);
        }

        public void InstallCommandAsync()
        {
            var asembly = _services.GetService<Assembly>();
            _commands.AddModules(asembly);
            _commands.AddTypeParsers(asembly);
        }

        public async Task StartClientAsync()
        {            
            var db = new LiteDatabase(new DatabaseDetails().LoadDetials().TokenDB);
#if DEBUG
            await _client.LoginAsync(TokenType.Bot, (db.GetCollection<TokenData>("Tokens").FindOne(x => x.Name.ToLower() == "dev")).Token);
#else 
            await _client.LoginAsync(TokenType.Bot, (db.GetCollection<TokenData>("Tokens").FindOne(x=>x.Name.ToLower() =="live")).Token);
#endif
            db.Dispose();
            await _client.StartAsync();
        }
    }
}

