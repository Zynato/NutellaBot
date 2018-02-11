using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot
{
    public class GuildMapping
    {
        public string Name { get; }
        public ulong Id { get; }

        public GuildMapping(string name, ulong id) {
            this.Name = name;
            this.Id = id;
        }
    }
}
