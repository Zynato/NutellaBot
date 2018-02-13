using Discord.Commands;
using DiscordBot.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class SimpleCommands : ModuleBase<SocketCommandContext>
    {
        public IVariableStorage Variables { get; set; }

        [Command("tagline")]
        [Summary("Gets or sets your tagline")]
        public async Task TaglineAsync([Remainder] string tagline = null) {
            var localVariables = await Variables.GetUserVariableSet(Context.Guild.Id, Context.User.Id, "tagline");

            if (string.IsNullOrEmpty(tagline)) {
                if (localVariables["tagline"].Value.HasValue) {
                    var taglineMessage = $"{Context.User.Username}'s tagline:\n{localVariables["tagline"].Value.Value}";

                    await Context.Channel.SendMessageAsync(taglineMessage);
                } else {
                    await Context.Channel.SendMessageAsync("You don't have a tagline saved!");
                }
            } else {
                localVariables.Upsert("tagline", tagline);

                await Variables.SetUserVariableSet(Context.Guild.Id, Context.User.Id, localVariables);

                await Context.Channel.SendMessageAsync("Tagline updated!");
            }
        }
    }
}
