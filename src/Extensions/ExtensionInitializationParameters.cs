using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Extensions
{
    public class ExtensionInitializationParameters
    {
        public DiscordSocketClient Client { get; }
        public ServiceCollection ServiceCollection { get; }
        public IConfiguration Configuration { get; }

        public ExtensionInitializationParameters(DiscordSocketClient client, ServiceCollection serviceCollection, IConfiguration configuration) {
            this.Client = client;
            this.ServiceCollection = serviceCollection;
            this.Configuration = configuration;
        }
    }
}
