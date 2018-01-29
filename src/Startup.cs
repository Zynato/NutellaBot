using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    class Startup
    {
        IConfiguration configuration;

        DiscordSocketClient client;

        public Startup() {
            this.configuration = BuildConfiguration();
        }

        public async Task RunAsync() {
            client = new DiscordSocketClient();

            var token = this.configuration.GetSection("Discord")["Token"];

            if (string.IsNullOrEmpty(token)) {
                Console.Error.WriteLine("Invalid token specified (null)");
                return;
            }

            client.MessageReceived += Client_MessageReceived;

            await client.LoginAsync(Discord.TokenType.Bot, token);
            await client.StartAsync();

            await Task.Delay(-1);
        }

        private async Task Client_MessageReceived(SocketMessage message) {
            if (message.Content == "!nutella") {
                await message.Channel.SendMessageAsync("Nutella!");
            }
        }

        private IConfiguration BuildConfiguration() {
            var builder = new ConfigurationBuilder()
                              .SetBasePath(Directory.GetCurrentDirectory())
                              .AddJsonFile("appsettings.json", optional: true);
            builder.AddEnvironmentVariables();

            return builder.Build();
        }
    }
}
