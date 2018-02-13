using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Database
{
    public class InMemoryVariableStorage : IVariableStorage
    {
        // TODO: Should this be thread-safe?

        Dictionary<ulong, Dictionary<ulong, Dictionary<string, string>>> variables;
        Dictionary<ulong, Dictionary<string, string>> globalVariables;

        public InMemoryVariableStorage() {
            this.variables = new Dictionary<ulong, Dictionary<ulong, Dictionary<string, string>>>();
            this.globalVariables = new Dictionary<ulong, Dictionary<string, string>>();
        }

        private Maybe<string> GetUserVariable(ulong guildId, ulong userId, string variable) {
            if (variables.TryGetValue(guildId, out var guildVariablesCollection)) {
                if (guildVariablesCollection.TryGetValue(userId, out var variablesCollection)) {
                    if (variablesCollection.TryGetValue(variable, out var value)) {
                        return new Maybe<string>(value);
                    }
                }
            }

            return new Maybe<string>();
        }

        private void SetUserVariable(ulong guildId, ulong userId, string variable, string value) {
            if (!variables.TryGetValue(guildId, out var guildVariablesCollection)) {
                guildVariablesCollection = new Dictionary<ulong, Dictionary<string, string>>();
                variables.Add(guildId, guildVariablesCollection);
            }

            if (!guildVariablesCollection.TryGetValue(userId, out var variablesCollection)) {
                variablesCollection = new Dictionary<string, string>();
                guildVariablesCollection.Add(userId, variablesCollection);
            }

            if (!variablesCollection.ContainsKey(variable)) {
                variablesCollection.Add(variable, value);
            } else {
                variablesCollection[variable] = value;
            }
        }

        private Maybe<string> GetGlobalVariable(ulong guildId, string variable) {
            if (globalVariables.TryGetValue(guildId, out var guildGlobalVariables)) {
                if (guildGlobalVariables.TryGetValue(variable, out var value)) {
                    return new Maybe<string>(value);
                }
            }

            return new Maybe<string>();
        }

        private void SetGlobalVariable(ulong guildId, string variable, string value) {
            if (!globalVariables.TryGetValue(guildId, out var guildGlobalVariables)) {
                guildGlobalVariables = new Dictionary<string, string>();
                globalVariables.Add(guildId, guildGlobalVariables);
            }

            if (!guildGlobalVariables.ContainsKey(variable)) {
                guildGlobalVariables.Add(variable, value);
            } else {
                guildGlobalVariables[variable] = value;
            }
        }

        public Task<VariableCollection> GetUserVariableSet(ulong guildId, ulong userId, params string[] variableNames) {
            var variableCollection = new VariableCollection();
            foreach (var variableName in variableNames) {
                var result = GetUserVariable(guildId, userId, variableName);

                variableCollection.AddWithoutTracking(new Variable(variableName, result));
            }

            return Task.FromResult(variableCollection);
        }

        public Task SetUserVariableSet(ulong guildId, ulong userId, VariableCollection variables) {
            foreach (var variable in variables.SubsetModified()) {
                SetUserVariable(guildId, userId, variable.Name, variable.Value.Value);
            }

            return Task.CompletedTask;
        }

        public Task<VariableCollection> GetGlobalVariableSet(ulong guildId, params string[] variableNames) {
            var variableCollection = new VariableCollection();
            foreach (var variableName in variableNames) {
                var result = GetGlobalVariable(guildId, variableName);

                variableCollection.AddWithoutTracking(new Variable(variableName, result));
            }

            return Task.FromResult(variableCollection);
        }

        public Task SetGlobalVariableSet(ulong guildId, VariableCollection variables) {
            foreach (var variable in variables.SubsetModified()) {
                SetGlobalVariable(guildId, variable.Name, variable.Value.Value);
            }

            return Task.CompletedTask;
        }
    }
}
