using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class SimpleResponses : ModuleBase<SocketCommandContext>
    {
        [Command("nutella")]
        [Summary("Responds with nutella")]
        public async Task NutellaAsync() {
            var embed = new EmbedBuilder()
                            .WithImageUrl("https://target.scene7.com/is/image/Target/14774450?wid=520&hei=520&fmt=pjpeg");

            await Context.Channel.SendMessageAsync("", embed: embed);
        }

        [Command("pika")]
        [Summary("Responds with pika")]
        public async Task PikaAsync() {
            await ReplyAsync("Pika!");
        }

        [Command("creator?")]
        [Summary("Responds with the creator")]
        public async Task CreatorAsync() {
            await ReplyAsync("ZYN!");
        }

        [Command("daro")]
        [Summary("Who is Daro?")]
        public async Task DaroAsync() {
            await ReplyAsync("Einstein V2 and a good helper");
        }

        [Command("solid")]
        [Summary("Who is Solid?")]
        public async Task SolidAsync() {
            await ReplyAsync("Good Helper");
        }

        [Command("demi")]
        [Summary("Who is Demi?")]
        public async Task DemiAsync() {
            await ReplyAsync("A Noob");
        }
    }
}
