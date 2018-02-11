using Discord;
using Discord.Commands;
using DiscordBot.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Commands
{
    public class SimpleResponses : ModuleBase<SocketCommandContext>
    {
        public IVariableStorage Variables { get; set; }

        [Command("server")]
        [Summary("Get server details")]
        public async Task ServerAsync() {
            await Context.Channel.SendMessageAsync($"Server id: {Context.Guild.Id}\nServer owner: {Context.Guild.OwnerId}\nCurrent channel id: {Context.Channel.Id}");
        }

        [Command("count")]
        [Summary("Increments a counter by 1")]
        public async Task CountAsync() {
            var count = Variables.GetUserVariableAsInt32(Context.Guild.Id, Context.User.Id, "count");

            count++;

            await Context.Channel.SendMessageAsync($"Counter is now at {count}");

            Variables.SetUserVariable(Context.Guild.Id, Context.User.Id, "count", count);
        }

        [Command("countdown")]
        [Summary("Decrements a counter by 1")]
        public async Task CountdownAsync() {
            var count = Variables.GetUserVariableAsInt32(Context.Guild.Id, Context.User.Id, "count");

            count--;

            await Context.Channel.SendMessageAsync($"Counter is now at {count}");

            Variables.SetUserVariable(Context.Guild.Id, Context.User.Id, "count", count);
        }

        [Command("globalcount")]
        [Summary("Increments the global counter by 1")]
        public async Task GlobalCountAsync() {
            var count = Variables.GetGlobalVariableAsInt32(Context.Guild.Id, "count");

            count++;

            await Context.Channel.SendMessageAsync($"Global counter is now at {count}");

            Variables.SetGlobalVariable(Context.Guild.Id, "count", count.ToString());
        }

        [Command("globalcountdown")]
        [Summary("Decrements the global counter by 1")]
        public async Task GlobalCountdownAsync() {
            var count = Variables.GetGlobalVariableAsInt32(Context.Guild.Id, "count");

            count--;

            await Context.Channel.SendMessageAsync($"Global counter is now at {count}");

            Variables.SetGlobalVariable(Context.Guild.Id, "count", count.ToString());
        }

        [Command("whoami")]
        [Summary("Reponds with the senders username")]
        public async Task WhoAmIAsync() {
            await Context.Channel.SendMessageAsync($"You are {Context.User.Username}:{Context.User.Id}");
        }

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
            await ReplyAsync("**You're pretty good** - Big Boss");
        }

        [Command("demi")]
        [Summary("Who is Demi?")]
        public async Task DemiAsync() {
            await ReplyAsync("A Noob");
        }

        [Command("alextoti")]
        [Summary("Who is alextoti?")]
        public async Task AlexTotiAsync() {
            await ReplyAsync("hm don't know xD");
        }

        [Command("link")]
        [Summary("Who is Link?")]
        public async Task LinkAsync() {
            await ReplyAsync("Bitco(nnect)in millionaire!!!");
        }

        [Command("bitconnect")]
        [Summary("Wassa wassaa wassuuuuppp!")]
        public async Task BitconnectAsync() {
            await ReplyAsync("Wassa wassaa wassuuuuppp!");
            await ReplyAsync("https://www.youtube.com/watch?v=61i2iDz7u04");
        }
    }
}
