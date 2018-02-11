using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Extensions
{
    public interface IExtension
    {
        Assembly ExtensionAssembly { get; }

        Task Initialize(ExtensionInitializationParameters parameters);
    }
}
