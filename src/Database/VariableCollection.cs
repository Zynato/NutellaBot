using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Database
{
    public class VariableCollection : IEnumerable<Variable>
    {
        Dictionary<string, Variable> variables;

        HashSet<string> modifiedVariables;

        public VariableCollection() {
            this.variables = new Dictionary<string, Variable>();
            this.modifiedVariables = new HashSet<string>();
        }

        public Variable this[string name] {
            get { return this.variables[name]; }
        }

        public VariableCollection SubsetModified() {
            return Subset(modifiedVariables);
        }

        public VariableCollection Subset(params string[] variables) {
            return Subset((IEnumerable<string>)variables);
        }

        public VariableCollection Subset(IEnumerable<string> variables) {
            var variableCollection = new VariableCollection();
            foreach (var variable in variables) {
                if (this.variables.TryGetValue(variable, out var value)) {
                    variableCollection.variables.Add(variable, value);
                }
            }
            return variableCollection;
        }

        public void AddWithoutTracking(Variable variable) {
            this.variables.Add(variable.Name, variable);
        }

        public void Upsert(string name, string value) {
            if (!modifiedVariables.Contains(name)) {
                modifiedVariables.Add(name);
            }

            if (this.variables.TryGetValue(name, out var variable)) {
                variable.Value = new Maybe<string>(value);
            } else {
                this.variables.Add(name, new Variable(name, new Maybe<string>(value)));
            }
        }

        public void Upsert(string name, int value) {
            Upsert(name, value.ToString());
        }

        public void Upsert(string name, bool value) {
            Upsert(name, value.ToString());
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return variables.Values.GetEnumerator();
        }

        public IEnumerator<Variable> GetEnumerator() {
            return variables.Values.GetEnumerator();
        }
    }
}
