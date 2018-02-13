using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Database
{
    public interface IVariableStorage
    {
        Maybe<string> GetUserVariable(ulong guildId, ulong userId, string variable);
        void SetUserVariable(ulong guildId, ulong userId, string variable, string value);

        Maybe<string> GetGlobalVariable(ulong guildId, string variable);
        void SetGlobalVariable(ulong guildId, string variable, string value);

        Task<VariableCollection> GetUserVariableSet(ulong guildId, ulong userId, params string[] variableNames);
        Task SetUserVariableSet(ulong guildId, ulong userId, VariableCollection variables);

        Task<VariableCollection> GetGlobalVariableSet(ulong guildId, params string[] variableNames);
        Task SetGlobalVariableSet(ulong guildId, VariableCollection variables);
    }
}
