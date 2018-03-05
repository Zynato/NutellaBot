using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    [Group("admin")]
    [RequireOwner]
    public class BotGlobalAdmin : ModuleBase<SocketCommandContext>
    {
        public DiscordSocketClient Client { get; set; }

        [Command("username")]
        [Summary("Change the bot username")]
        [RequireContext(ContextType.DM)]
        public async Task ChangeUsernameAsync(string username) {
            await Client.CurrentUser.ModifyAsync(x => x.Username = username, new Discord.RequestOptions() { AuditLogReason = "Performed by bot owner" });

            await Context.Channel.SendMessageAsync("My username has been changed!");
        }
    }
}
