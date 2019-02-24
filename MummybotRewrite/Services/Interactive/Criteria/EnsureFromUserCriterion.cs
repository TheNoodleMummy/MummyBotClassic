﻿using System.ComponentModel;
using System.Threading.Tasks;
using Qmmands;

namespace Discord.Addons.Interactive
{
    public class EnsureFromUserCriterion : ICriterion<IMessage>
    {
        private readonly ulong _id;

        public EnsureFromUserCriterion(IUser user)
            => _id = user.Id;

        [EditorBrowsable(EditorBrowsableState.Never)]
        public EnsureFromUserCriterion(ulong id)
            => _id = id;

        public Task<bool> JudgeAsync(ICommandContext sourceContext, IMessage parameter)
        {
            bool ok = _id == parameter.Author.Id;
            return Task.FromResult(ok);
        }
    }
}
