using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Database;
using DiscordBot.Extensions;
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
    public class Startup
    {
        IConfiguration configuration;
        IServiceProvider services;

        DiscordSocketClient client;
        CommandService commands;

        CommandHandler commandHandler;

        public List<IExtension> Extensions { get; }

        public Startup() {
            this.configuration = BuildConfiguration();
            this.Extensions = new List<IExtension>();
        }

        public async Task RunAsync() {
            client = new DiscordSocketClient();
            commands = new CommandService();

            var token = this.configuration.GetSection("Discord")["Token"];

            if (string.IsNullOrEmpty(token)) {
                Console.Error.WriteLine("Invalid token specified (null)");
                return;
            }

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(client);
            serviceCollection.AddSingleton(commands);
            serviceCollection.AddSingleton(this.configuration);
            serviceCollection.AddSingleton(new Random());
            serviceCollection.AddSingleton<GuildPrefixManager>();

            serviceCollection.AddSingleton(LoadGuildMappings(configuration));

            var extensionInitialiationParameters = new ExtensionInitializationParameters(client, serviceCollection, configuration);
            foreach (var extension in Extensions) {
                await extension.Initialize(extensionInitialiationParameters);
            }

            var hasVariableStorage = false;
            foreach (var serviceDescriptor in serviceCollection) {
                if (serviceDescriptor.ServiceType.IsAssignableFrom(typeof(IVariableStorage))) {
                    hasVariableStorage = true;
                    break;
                }
            }

            // Add the default variable storage provider if one has not already been added
            if (!hasVariableStorage) {
                serviceCollection.AddSingleton<IVariableStorage>(new InMemoryVariableStorage());
            }

            services = serviceCollection.BuildServiceProvider();

            commandHandler = new CommandHandler(client, commands, services);
            commandHandler.StartListening();

            await commands.AddModulesAsync(Assembly.GetExecutingAssembly());

            foreach (var extension in Extensions) {
                await commands.AddModulesAsync(extension.ExtensionAssembly);
            }

            foreach (var extension in Extensions) {
                await extension.Startup(services);
            }

            await client.LoginAsync(Discord.TokenType.Bot, token);
            await client.StartAsync();

            Console.WriteLine("Bot loaded.");
        }

        private GuildMappingCollection LoadGuildMappings(IConfiguration configuration) {
            var guildMappings = new GuildMappingCollection();
            var section = configuration.GetSection("Discord").GetSection("Guilds");

            foreach (var item in section.GetChildren()) {
                var guildName = item.Key;
                var guildId = item.Value;

                guildMappings.Add(new GuildMapping(guildName, ulong.Parse(guildId)));
            }

            return guildMappings;
        }

        private IConfiguration BuildConfiguration() {
            var builder = new ConfigurationBuilder()
                              .SetBasePath(Directory.GetCurrentDirectory())
#if DEBUG
                              .AddJsonFile("appsettings.debug.json", optional: true)
#endif
                              ; // This needs to be here when DEBUG is not defined
            builder.AddEnvironmentVariables();

            return builder.Build();
        }
    }
}
