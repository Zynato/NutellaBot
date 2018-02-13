using Discord;
using Discord.Commands;
using DiscordBot.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Commands.Games
{
    [Group("hm")]
    public class Hangman : ModuleBase<SocketCommandContext>
    {
        public IVariableStorage Variables { get; set; }
        public Random Random { get; set; }

        [Command("play")]
        [Summary("Start playing a new game of hangman")]
        public async Task PlayAsync() {
            var localVariables = await Variables.GetGlobalVariableSet(Context.Guild.Id, "hm-playing");

            var isPlaying = localVariables["hm-playing"].AsBoolean();

            if (isPlaying) {
                await Context.Channel.SendMessageAsync("A game is already in progress!");
                return;
            }

            string word;

            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream("DiscordBot.Resources.HangmanWordList.txt")) {
                using (var reader = new StreamReader(stream)) {
                    var wordList = reader.ReadToEnd().Replace("\r", "").Split("\n");

                    word = wordList[Random.Next(0, wordList.Length)].ToLower();
                }
            }

            Console.WriteLine($"The selected word is {word}");

            // Start playing a new game
            localVariables.Upsert("hm-playing", true);
            localVariables.Upsert("hm-word", word);

            await Variables.SetGlobalVariableSet(Context.Guild.Id, localVariables);

            await Context.Channel.SendMessageAsync("A new game of hangman has been started!");
            await PrintGameState();
        }

        [Command("stop")]
        [Summary("Stop playing a game in progress")]
        public async Task StopAsync() {
            var localVariables = await Variables.GetGlobalVariableSet(Context.Guild.Id, "hm-playing");

            var isPlaying = localVariables["hm-playing"].AsBoolean();

            if (!isPlaying) {
                await Context.Channel.SendMessageAsync("There aren't any games currently in progress");
                return;
            }

            await ResetGameState();

            await Context.Channel.SendMessageAsync("Game stopped.");
        }

        [Command("state")]
        [Summary("Print the current game state")]
        public async Task StateAsync() {
            var localVariables = await Variables.GetGlobalVariableSet(Context.Guild.Id, "hm-playing");

            var isPlaying = localVariables["hm-playing"].AsBoolean();

            if (!isPlaying) {
                await Context.Channel.SendMessageAsync("There aren't any games currently in progress");
                return;
            }

            await PrintGameState();
        }

        [Command("guess")]
        [Summary("Guess a letter in the word")]
        public async Task GuessAsync([Summary("The letter to guess")] char letter) {
            var localVariables = await Variables.GetGlobalVariableSet(Context.Guild.Id, "hm-playing", "hm-word", "hm-guesses", "hm-guess-count");

            var isPlaying = localVariables["hm-playing"].AsBoolean();

            if (!isPlaying) {
                await Context.Channel.SendMessageAsync("There aren't any games currently in progress");
                return;
            }

            var maybeWord = localVariables["hm-word"].Value;

            string word;
            if (!maybeWord.HasValue) {
                return;
            }

            word = maybeWord.Value;

            var normalizedGuess = letter.ToString().ToLower();

            var guesses = localVariables["hm-guesses"].Value.Value ?? "";
            var guessCount = localVariables["hm-guess-count"].AsInt();

            if (guesses.Contains(normalizedGuess)) {
                await Context.Channel.SendMessageAsync("That letter has already been guessed.");
            } else {
                if (!word.Contains(normalizedGuess)) {
                    guessCount++;

                    localVariables.Upsert("hm-guess-count", guessCount);
                }

                guesses += letter.ToString().ToLower();
                localVariables.Upsert("hm-guesses", guesses);
            }

            await Variables.SetGlobalVariableSet(Context.Guild.Id, localVariables);

            await PrintGameState();

            if (guessCount == 6) {
                await ResetGameState();

                var embed = new EmbedBuilder()
                                .WithImageUrl("http://sonomasun.com/wp-content/uploads/2016/12/game_over.png");

                await Context.Channel.SendMessageAsync("", embed: embed);
            } else {
                // Check if the game is over
                bool missingLetter = false;
                for (var i = 0; i < word.Length; i++) {
                    if (!guesses.Contains(word[i].ToString())) {
                        missingLetter = true;
                        break;
                    }
                }

                if (!missingLetter) {
                    await ResetGameState();

                    var embed = new EmbedBuilder()
                                .WithImageUrl("https://ichef.bbci.co.uk/childrens-responsive-ichef-live/r/720/1x/cbbc/win-eurovision-index.jpg");

                    await Context.Channel.SendMessageAsync("", embed: embed);
                }
            }
        }

        private async Task ResetGameState() {
            var localVariables = await Variables.GetGlobalVariableSet(Context.Guild.Id);

            localVariables.Upsert("hm-playing", false);
            localVariables.Upsert("hm-word", "");
            localVariables.Upsert("hm-guess-count", 0);
            localVariables.Upsert("hm-guesses", "");

            await Variables.SetGlobalVariableSet(Context.Guild.Id, localVariables);
        }

        private async Task PrintGameState() {
            var localVariables = await Variables.GetGlobalVariableSet(Context.Guild.Id, "hm-word", "hm-guess-count", "hm-guesses");

            var maybeWord = localVariables["hm-word"].Value;

            string word;
            if (!maybeWord.HasValue) {
                return;
            }

            word = maybeWord.Value;

            var guessCount = localVariables["hm-guess-count"].AsInt();

            var hangmanStateBuilder = new StringBuilder();

            hangmanStateBuilder.Append("```");
            hangmanStateBuilder.AppendLine(" _________     ");
            hangmanStateBuilder.AppendLine("|         |    ");
            if (guessCount == 0) {
                hangmanStateBuilder.AppendLine("|");
                hangmanStateBuilder.AppendLine("|");
                hangmanStateBuilder.AppendLine("|");
                hangmanStateBuilder.AppendLine("|");
            } else if (guessCount == 1) {
                hangmanStateBuilder.AppendLine("|         0    ");
                hangmanStateBuilder.AppendLine("|              ");
                hangmanStateBuilder.AppendLine("|              ");
                hangmanStateBuilder.AppendLine("|              ");
            } else if (guessCount == 2) {
                hangmanStateBuilder.AppendLine("|         0    ");
                hangmanStateBuilder.AppendLine("|         |    ");
                hangmanStateBuilder.AppendLine("|              ");
                hangmanStateBuilder.AppendLine("|              ");
            } else if (guessCount == 3) {
                hangmanStateBuilder.AppendLine("|         0    ");
                hangmanStateBuilder.AppendLine("|        /|    ");
                hangmanStateBuilder.AppendLine("|              ");
                hangmanStateBuilder.AppendLine("|              ");
            } else if (guessCount == 4) {
                hangmanStateBuilder.AppendLine("|         0    ");
                hangmanStateBuilder.AppendLine("|        /|\\  ");
                hangmanStateBuilder.AppendLine("|              ");
                hangmanStateBuilder.AppendLine("|              ");
            } else if (guessCount == 5) {
                hangmanStateBuilder.AppendLine("|         0    ");
                hangmanStateBuilder.AppendLine("|        /|\\  ");
                hangmanStateBuilder.AppendLine("|        /     ");
                hangmanStateBuilder.AppendLine("|              ");
            } else if (guessCount == 6) {
                hangmanStateBuilder.AppendLine("|         0    ");
                hangmanStateBuilder.AppendLine("|        /|\\  ");
                hangmanStateBuilder.AppendLine("|        / \\  ");
                hangmanStateBuilder.AppendLine("|              ");
            }

            var guesses = localVariables["hm-guesses"].Value.Value ?? "";

            hangmanStateBuilder.Append("| ");
            for (var i = 0; i < word.Length; i++) {
                if (guesses.Contains(word[i].ToString())) {
                    hangmanStateBuilder.Append(word[i]);
                    hangmanStateBuilder.Append(' ');
                } else {
                    hangmanStateBuilder.Append("_ ");
                }
            }

            hangmanStateBuilder.Append("```");

            await Context.Channel.SendMessageAsync(hangmanStateBuilder.ToString());
        }
    }
}
