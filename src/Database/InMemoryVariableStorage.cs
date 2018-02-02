using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Database
{
    public class InMemoryVariableStorage : IVariableStorage
    {
        // TODO: Should this be thread-safe?

        Dictionary<ulong, Dictionary<string, string>> variables;
        Dictionary<string, string> globalVariables;

        public InMemoryVariableStorage() {
            this.variables = new Dictionary<ulong, Dictionary<string, string>>();
            this.globalVariables = new Dictionary<string, string>();
        }

        public Maybe<string> GetUserVariable(ulong userId, string variable) {
            if (variables.TryGetValue(userId, out var variablesCollection)) {
                if (variablesCollection.TryGetValue(variable, out var value)) {
                    return new Maybe<string>(value);
                }
            }

            return new Maybe<string>();
        }

        public void SetUserVariable(ulong userId, string variable, string value) {
            if (!variables.TryGetValue(userId, out var variablesCollection)) {
                variablesCollection = new Dictionary<string, string>();
                variables.Add(userId, variablesCollection);
            }

            if (!variablesCollection.ContainsKey(variable)) {
                variablesCollection.Add(variable, value);
            } else {
                variablesCollection[variable] = value;
            }
        }

        public Maybe<string> GetGlobalVariable(string variable) {
            if (globalVariables.TryGetValue(variable, out var value)) {
                return new Maybe<string>(value);
            }

            return new Maybe<string>();
        }

        public void SetGlobalVariable(string variable, string value) {
            if (!globalVariables.ContainsKey(variable)) {
                globalVariables.Add(variable, value);
            } else {
                globalVariables[variable] = value;
            }
        }
    }
}
