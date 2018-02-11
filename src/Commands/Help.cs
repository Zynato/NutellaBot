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
        public IServiceProvider ServiceProvider { get; set; }

        public Help(IServiceProvider serviceProvider) {
            this.ServiceProvider = serviceProvider;
        }

        [Command("help")]
        [Summary("Print help information")]
        public async Task HelpAsync() {
            var helpBuilder = new StringBuilder();
            helpBuilder.AppendLine("**Available commands:**");
            helpBuilder.Append("```");

            foreach (var module in Commands.Modules) {
                var moduleAlias = module.Aliases.First();
                
                var commandsBuilder = new StringBuilder();
                foreach (var command in module.Commands) {
                    var result = await command.CheckPreconditionsAsync(Context, ServiceProvider);
                    if (result.IsSuccess) {
                        commandsBuilder.Append("!"); // TODO: Don't hardcode the command prefix
                        if (!string.IsNullOrEmpty(moduleAlias)) {
                            commandsBuilder.Append($"{moduleAlias} ");
                        }
                        commandsBuilder.Append($"{command.Name}");
                        if (command.Parameters.Count > 0) {
                            commandsBuilder.Append(' ');
                            commandsBuilder.AppendJoin(" ", command.Parameters.Select(x => $"[{x.Name}]"));
                        }
                        if (!string.IsNullOrEmpty(command.Summary)) {
                            commandsBuilder.Append($" - {command.Summary}");
                        }
                        commandsBuilder.AppendLine();
                    }
                }

                if (commandsBuilder.Length > 0) {
                    helpBuilder.AppendLine();
                    helpBuilder.Append(commandsBuilder.ToString());
                }
            }

            helpBuilder.Append("```");

            await Context.Channel.SendMessageAsync(helpBuilder.ToString());
        }
    }
}
