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
            var isPlaying = Variables.GetGlobalVariableAsBoolean("hm-playing");

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
            Variables.SetGlobalVariable("hm-playing", true);
            Variables.SetGlobalVariable("hm-word", word);

            await Context.Channel.SendMessageAsync("A new game of hangman has been started!");
            await PrintGameState();
        }

        [Command("stop")]
        [Summary("Stop playing a game in progress")]
        public async Task StopAsync() {
            var isPlaying = Variables.GetGlobalVariableAsBoolean("hm-playing");

            if (!isPlaying) {
                await Context.Channel.SendMessageAsync("There aren't any games currently in progress");
                return;
            }

            ResetGameState();

            await Context.Channel.SendMessageAsync("Game stopped.");
        }

        [Command("state")]
        [Summary("Print the current game state")]
        public async Task StateAsync() {
            var isPlaying = Variables.GetGlobalVariableAsBoolean("hm-playing");

            if (!isPlaying) {
                await Context.Channel.SendMessageAsync("There aren't any games currently in progress");
                return;
            }

            await PrintGameState();
        }

        [Command("guess")]
        [Summary("Guess a letter in the word")]
        public async Task GuessAsync([Summary("The letter to guess")] char letter) {
            var isPlaying = Variables.GetGlobalVariableAsBoolean("hm-playing");

            if (!isPlaying) {
                await Context.Channel.SendMessageAsync("There aren't any games currently in progress");
                return;
            }

            var maybeWord = Variables.GetGlobalVariable("hm-word");

            string word;
            if (!maybeWord.HasValue) {
                return;
            }

            word = maybeWord.Value;

            var normalizedGuess = letter.ToString().ToLower();

            var guesses = Variables.GetGlobalVariable("hm-guesses").Value ?? "";
            var guessCount = Variables.GetGlobalVariableAsInt32("hm-guess-count");

            if (guesses.Contains(normalizedGuess)) {
                await Context.Channel.SendMessageAsync("That letter has already been guessed.");
            } else {
                if (!word.Contains(normalizedGuess)) {
                    guessCount++;

                    Variables.SetGlobalVariable("hm-guess-count", guessCount);
                }

                guesses += letter.ToString().ToLower();
                Variables.SetGlobalVariable("hm-guesses", guesses);
            }

            await PrintGameState();

            if (guessCount == 6) {
                ResetGameState();

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
                    ResetGameState();

                    var embed = new EmbedBuilder()
                                .WithImageUrl("https://ichef.bbci.co.uk/childrens-responsive-ichef-live/r/720/1x/cbbc/win-eurovision-index.jpg");

                    await Context.Channel.SendMessageAsync("", embed: embed);
                }
            }
        }

        private void ResetGameState() {
            Variables.SetGlobalVariable("hm-playing", false);
            Variables.SetGlobalVariable("hm-word", "");
            Variables.SetGlobalVariable("hm-guess-count", 0);
            Variables.SetGlobalVariable("hm-guesses", "");
        }

        private async Task PrintGameState() {
            var maybeWord = Variables.GetGlobalVariable("hm-word");

            string word;
            if (!maybeWord.HasValue) {
                return;
            }

            word = maybeWord.Value;

            var guessCount = Variables.GetGlobalVariableAsInt32("hm-guess-count");

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

            var guesses = Variables.GetGlobalVariable("hm-guesses").Value ?? "";

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
