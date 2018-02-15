using DiscordBot.Database;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class GuildPrefixManager
    {
        string defaultPrefix;

        IVariableStorage variables;

        Dictionary<ulong, string> cachedPrefixes;

        public GuildPrefixManager(IVariableStorage variables, IConfiguration configuration) {
            this.variables = variables;

            this.cachedPrefixes = new Dictionary<ulong, string>();

            this.defaultPrefix = configuration.GetSection("Discord")["DefaultPrefix"];
        }

        public async Task<string> GetPrefix(ulong guildId) {
            if (this.cachedPrefixes.TryGetValue(guildId, out var prefix)) {
                return prefix;
            } else {
                var localVariables = await variables.GetGlobalVariableSet(guildId, "bot-prefix");

                var botPrefix = defaultPrefix;
                if (localVariables["bot-prefix"].Value.HasValue) {
                    botPrefix = localVariables["bot-prefix"].Value.Value;
                }

                this.cachedPrefixes.Add(guildId, botPrefix);

                return botPrefix;
            }
        }

        public async Task SetPrefix(ulong guildId, string prefix) {
            var localVariables = await variables.GetGlobalVariableSet(guildId, "bot-prefix");

            localVariables.Upsert("bot-prefix", prefix);

            await variables.SetGlobalVariableSet(guildId, localVariables);

            if (this.cachedPrefixes.ContainsKey(guildId)) {
                this.cachedPrefixes[guildId] = prefix;
            }
        }
    }
}
