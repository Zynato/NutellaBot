using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Database
{
    public interface IVariableStorage
    {
        Maybe<string> GetUserVariable(ulong userId, string variable);
        void SetUserVariable(ulong userId, string variable, string value);

        Maybe<string> GetGlobalVariable(string variable);
        void SetGlobalVariable(string variable, string value);
    }
}
