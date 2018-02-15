using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiscordBot
{
    public class GuildMappingCollection : List<GuildMapping>
    {
        public GuildMapping Find(ulong guildId) {
            return this.Where(x => x.Id == guildId).FirstOrDefault();
        }
    }
}
