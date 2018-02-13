using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Database
{
    public class VariableCollection : Dictionary<string, Variable>
    {
        public VariableCollection Subset(params string[] variables) {
            var variableCollection = new VariableCollection();
            foreach (var variable in variables) {
                if (this.TryGetValue(variable, out var value)) {
                    variableCollection.Add(variable, value);
                }
            }
            return variableCollection;
        }
    }
}
