using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Database
{
    public interface IVariableStorage
    {
        Task<VariableCollection> GetUserVariableSet(ulong guildId, ulong userId, params string[] variableNames);
        Task SetUserVariableSet(ulong guildId, ulong userId, VariableCollection variables);

        Task<VariableCollection> GetGlobalVariableSet(ulong guildId, params string[] variableNames);
        Task SetGlobalVariableSet(ulong guildId, VariableCollection variables);
    }
}
