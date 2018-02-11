using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Database
{
    public interface IVariableStorage
    {
        Maybe<string> GetUserVariable(ulong guildId, ulong userId, string variable);
        void SetUserVariable(ulong guildId, ulong userId, string variable, string value);

        Maybe<string> GetGlobalVariable(ulong guildId, string variable);
        void SetGlobalVariable(ulong guildId, string variable, string value);
    }
}
