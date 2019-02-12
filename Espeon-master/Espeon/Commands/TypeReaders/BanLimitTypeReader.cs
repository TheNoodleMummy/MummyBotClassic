﻿using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace Espeon.Commands.TypeReaders
{
    public class BanLimitTypeReader : TypeReader
    {
        public override Task<TypeReaderResult> ReadAsync(ICommandContext context, CommandInfo command, string input, IServiceProvider services)
        {
            if (!uint.TryParse(input, out var num))
                return Task.FromResult(TypeReaderResult.FromError(command, CommandError.ParseFailed,
                    "Prune amount must be an integer input > 0"));
            if (num > 7)
                return Task.FromResult(TypeReaderResult.FromError(command, CommandError.ParseFailed,
                    "Prune amount cannot be > 7"));
            return Task.FromResult(TypeReaderResult.FromSuccess(command, (int)num));
        }
    }
}
