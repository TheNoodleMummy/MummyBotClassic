﻿using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace Espeon.Commands.Preconditions
{
    public class RequireGuildOwnerAttribute : PreconditionAttribute
    {
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            return context.User.Id == context.Guild.OwnerId
                ? Task.FromResult(PreconditionResult.FromSuccess(command))
                : Task.FromResult(PreconditionResult.FromError(command, "Only the guild owner can execute this commands"));
        }
    }
}
