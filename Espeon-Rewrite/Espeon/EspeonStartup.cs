﻿using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Espeon.Core.Attributes;
using Espeon.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Qmmands;

namespace Espeon
{
    public class EspeonStartup
    {
        private readonly IServiceProvider _services;

        [Inject] private readonly DiscordSocketClient _client;

        [Inject] private readonly CommandService _commands;

        public EspeonStartup(IServiceProvider services)
        {
            _services = services;
        }

        public async Task StartBotAsync()
        {
            EventHooks();
            await _client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable("Testeon"));
            await _client.StartAsync();
        }

        public void EventHooks()
        {
            var logger = _services.GetService<ILogService>();
            _client.Log += logger.LogAsync;
        }
    }
}
