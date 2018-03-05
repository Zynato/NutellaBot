using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace DiscordBot.Preconditions
{
    public class RequireGuildAttribute : PreconditionAttribute
    {
        public string GuildName { get; }

        public RequireGuildAttribute(string guildName) {
            this.GuildName = guildName;
        }

        public override Task<PreconditionResult> CheckPermissions(ICommandContext context, CommandInfo command, IServiceProvider services) {
            var guildMappingCollection = services.GetService<GuildMappingCollection>();

            var guildMapping = guildMappingCollection.Where(x => x.Name == GuildName).FirstOrDefault();

            if (guildMapping != null) {
                if (context.Guild.Id == guildMapping.Id) {
                    return Task.FromResult(PreconditionResult.FromSuccess());
                }
            }

            return Task.FromResult(PreconditionResult.FromError("Unknown command."));
        }
    }
}
