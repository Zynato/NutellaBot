using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBot
{
    public class CommandHandler
    {
        DiscordSocketClient discordClient;
        CommandService commands;
        IServiceProvider services;

        public CommandHandler(DiscordSocketClient discordClient, CommandService commands, IServiceProvider services) {
            this.discordClient = discordClient;
            this.commands = commands;
            this.services = services;
        }

        public void StartListening() {
            this.discordClient.MessageReceived += DiscordClient_MessageReceived;
        }

        private async Task DiscordClient_MessageReceived(SocketMessage e) {
            // https://discord.foxbot.me/docs/guides/commands/commands.html
            var message = e as SocketUserMessage;
            if (message == null) {
                return;
            }

            var prefixManager = this.services.GetService<GuildPrefixManager>();
            var context = new SocketCommandContext(discordClient, message);

            bool canActivate = false;
            int argPos = 0;

            if (context.IsPrivate) {
                // Make sure we aren't replying to our own messages...
                if (context.User.Id != discordClient.CurrentUser.Id) {
                    canActivate = true;
                }
            } else if (context.Guild != null) {
                var botPrefix = await prefixManager.GetPrefix(context.Guild.Id);

                if (message.HasStringPrefix(botPrefix, ref argPos) || message.HasMentionPrefix(discordClient.CurrentUser, ref argPos)) {
                    canActivate = true;
                }
            }

            if (canActivate) {
                var result = await commands.ExecuteAsync(context, argPos, services);
                if (!result.IsSuccess) {
                    await context.Channel.SendMessageAsync(result.ErrorReason);
                }
            }
        }
    }
}
