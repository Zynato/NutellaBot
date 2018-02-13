using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Database
{
    public class Variable
    {
        public string Name { get; }
        public Maybe<string> Value { get; set; }

        public Variable(string name) {
            this.Name = name;
        }

        public Variable(string name, Maybe<string> value) : this(name) {
            this.Value = value;
        }

        public bool AsBoolean() {
            bool value = false;
            if (Value.HasValue) {
                value = Convert.ToBoolean(Value.Value);
            }

            return value;
        }

        public int AsInt() {
            int value = 0;
            if (Value.HasValue) {
                value = Convert.ToInt32(Value.Value);
            }

            return value;
        }
    }
}
