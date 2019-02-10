using System;
using ArrowverseBot.Minigames;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ArrowverseBot.Handlers
{
    class MinigameHandler
    {
        public static Trivia Trivia = new Trivia();
        public static TriviaQuestions TriviaQuestions;


       
       


        // Set up Trivia Questions
        public static void InitialTriviaSetup()
        {
            TriviaQuestions = JsonConvert.DeserializeObject<TriviaQuestions>(System.IO.File.ReadAllText("Minigames/Trivia/trivia_questions.json"));
        }

        // Display available minigames
        public static async Task DisplayGames(SocketCommandContext context)
        {
            if (!await Utilities.CheckForChannel(context, 509099311661842433, context.User)) return;
            await Utilities.SendEmbed(context.Channel, "MiniGames", "Trivia\n`!trivia`\n\nTic-Tac-Toe\n`!ttt`\n\nNumber Guess\n`!play ng`\n\nRussian Roulette\n`!rr`\n\n8-Ball\n`!8ball`", Colours.Green, "", "");
        }

        public async Task TryToStartTrivia(SocketCommandContext context, string input)
        {
            if (input == "all")
                await Trivia.TryToStartTrivia((SocketGuildUser)context.User, context, "all");
        }

        
        // Reset a game
        public static async Task ResetGame(SocketCommandContext context, string game)
        {
            if (!await Utilities.CheckForSuperadmin(context, context.User)) return;
            else if (game == "trivia")
            {
                await Utilities.SendEmbed(context.Channel, "MiniGames", $"{context.User.Mention} has reset Trivia.", Colours.Green, "", "");
                Trivia.ResetTrivia();
            }
            
            
            else if (game == "")
                await Utilities.PrintError(context.Channel, "Please specify a game to reset.");
            else
                await Utilities.PrintError(context.Channel, $"I was unable to find the `{game}` game.\n\nAvailable games to reset:\nTrivia\n`!trivia`\n\nTic-Tac-Toe\n`!ttt`\n\nNumber Guess\n`!play ng`\n\nRussian Roulette\n`!rr`");
        }
    }

   
    // Trivia Questions
    public partial class TriviaQuestions
    {
        [JsonProperty("Questions")]
        public TriviaQuestion[] Questions { get; set; }
    }

    public partial class TriviaQuestion
    {
        [JsonProperty("Question")]
        public string QuestionQuestion { get; set; }

        [JsonProperty("Answer")]
        public string Answer { get; set; }

        [JsonProperty("IncorrectAnswers")]
        public string[] IncorrectAnswers { get; set; }
    }
}
