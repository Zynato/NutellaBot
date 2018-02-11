using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class Help : ModuleBase<SocketCommandContext>
    {
        public CommandService Commands { get; set; }

        [Command("help")]
        [Summary("Print help information")]
        public async Task HelpAsync() {
            var helpBuilder = new StringBuilder();
            helpBuilder.AppendLine("**Available commands:**");
            helpBuilder.Append("```");

            foreach (var module in Commands.Modules) {
                var moduleAlias = module.Aliases.First();
                
                helpBuilder.AppendLine();
                foreach (var command in module.Commands) {
                    var result = await command.CheckPreconditionsAsync(Context);
                    if (result.IsSuccess) {
                        helpBuilder.Append("!"); // TODO: Don't hardcode the command prefix
                        if (!string.IsNullOrEmpty(moduleAlias)) {
                            helpBuilder.Append($"{moduleAlias} ");
                        }
                        helpBuilder.Append($"{command.Name}");
                        if (command.Parameters.Count > 0) {
                            helpBuilder.Append(' ');
                            helpBuilder.AppendJoin(" ", command.Parameters.Select(x => $"[{x.Name}]"));
                        }
                        helpBuilder.Append($" - {command.Summary}");
                        helpBuilder.AppendLine();
                    }
                }
            }

            helpBuilder.Append("```");

            await Context.Channel.SendMessageAsync(helpBuilder.ToString());
        }
    }
}
