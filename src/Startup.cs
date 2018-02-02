using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    class Startup
    {
        IConfiguration configuration;
        IServiceProvider services;

        DiscordSocketClient client;
        CommandService commands;

        public Startup() {
            this.configuration = BuildConfiguration();
        }

        public async Task RunAsync() {
            client = new DiscordSocketClient();
            commands = new CommandService();

            var token = this.configuration.GetSection("Discord")["Token"];

            if (string.IsNullOrEmpty(token)) {
                Console.Error.WriteLine("Invalid token specified (null)");
                return;
            }

            var debug = Convert.ToBoolean(this.configuration.GetSection("Discord")["Debug"]);

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(client);
            serviceCollection.AddSingleton(commands);

            if (debug) {
                serviceCollection.AddSingleton<IVariableStorage>(new InMemoryVariableStorage());
            }

            services = serviceCollection.BuildServiceProvider();

            client.MessageReceived += Client_MessageReceived;
            await commands.AddModulesAsync(Assembly.GetEntryAssembly());

            await client.LoginAsync(Discord.TokenType.Bot, token);
            await client.StartAsync();

            Console.WriteLine("Bot loaded and waiting.");

            await Task.Delay(-1);
        }

        private async Task Client_MessageReceived(SocketMessage e) {
            // https://discord.foxbot.me/docs/guides/commands/commands.html
            var message = e as SocketUserMessage;
            if (message == null) {
                return;
            }
            int argPos = 0;
            if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(client.CurrentUser, ref argPos))) {
                return;
            }
            var context = new SocketCommandContext(client, message);
            var result = await commands.ExecuteAsync(context, argPos, services);
            if (!result.IsSuccess) {
                await context.Channel.SendMessageAsync(result.ErrorReason);
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
