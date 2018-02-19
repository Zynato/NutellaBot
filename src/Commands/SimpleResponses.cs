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
        public Random Random { get; set; }

        [Command("server")]
        [Summary("Get server details")]
        public async Task ServerAsync() {
            await Context.Channel.SendMessageAsync($"Server id: {Context.Guild.Id}\nServer owner: {Context.Guild.OwnerId}\nCurrent channel id: {Context.Channel.Id}");
        }

        [Command("count")]
        [Summary("Increments a counter by 1")]
        public async Task CountAsync() {
            var localVariables = await Variables.GetUserVariableSet(Context.Guild.Id, Context.User.Id, "count");
            var count = localVariables["count"].AsInt();

            count++;

            await Context.Channel.SendMessageAsync($"Counter is now at {count}");

            localVariables.Upsert("count", count);

            await Variables.SetUserVariableSet(Context.Guild.Id, Context.User.Id, localVariables);
        }

        [Command("countdown")]
        [Summary("Decrements a counter by 1")]
        public async Task CountdownAsync() {
            var localVariables = await Variables.GetUserVariableSet(Context.Guild.Id, Context.User.Id, "count");
            var count = localVariables["count"].AsInt();

            count--;

            await Context.Channel.SendMessageAsync($"Counter is now at {count}");

            localVariables.Upsert("count", count);

            await Variables.SetUserVariableSet(Context.Guild.Id, Context.User.Id, localVariables);
        }

        [Command("globalcount")]
        [Summary("Increments the global counter by 1")]
        public async Task GlobalCountAsync() {
            var localVariables = await Variables.GetGlobalVariableSet(Context.Guild.Id, "count");

            var count = localVariables["count"].AsInt();

            count++;

            await Context.Channel.SendMessageAsync($"Global counter is now at {count}");

            localVariables.Upsert("count", count);

            await Variables.SetGlobalVariableSet(Context.Guild.Id, localVariables);
        }

        [Command("globalcountdown")]
        [Summary("Decrements the global counter by 1")]
        public async Task GlobalCountdownAsync() {
            var localVariables = await Variables.GetGlobalVariableSet(Context.Guild.Id, "count");

            var count = localVariables["count"].AsInt();

            count--;

            await Context.Channel.SendMessageAsync($"Global counter is now at {count}");

            localVariables.Upsert("count", count);

            await Variables.SetGlobalVariableSet(Context.Guild.Id, localVariables);
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
            var result = Random.Next(0, 5);
            switch (result) {
                case 0: {
                        await ReplyAsync("Heey heeey heeeeyyyyy");
                        await ReplyAsync("https://www.youtube.com/watch?v=61i2iDz7u04");
                    }
                    break;
                case 1: {
                        await ReplyAsync("Wassa wassaa wassuuuuppp!");
                        await ReplyAsync("https://youtu.be/vhyAREaWfyU?t=31s");
                    }
                    break;
                case 2: {
                        await ReplyAsync("WWOOOAAAHHH IT'S AMAZINGGG!!!");
                        await ReplyAsync("https://www.youtube.com/watch?v=hXRhIXp4idM");
                    }
                    break;
                case 3: {
                        await ReplyAsync("Bitconnect is in the world around me!!");
                        await ReplyAsync("https://www.youtube.com/watch?v=SerREQ93g_I");
                    }
                    break;
                case 4: {
                        await ReplyAsync("Friendship is magic!");
                        await ReplyAsync("https://www.youtube.com/watch?v=ZcBNxuKZyN4");
                    }
                    break;
            }
        }
    }
}
