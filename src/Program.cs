using System;
using System.Threading.Tasks;

namespace DiscordBot
{
    class Program
    {
        public static async Task Main() {
            await new Startup().RunAsync();

            await Task.Delay(-1);
        }
    }
}
