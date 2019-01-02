using Discord;
using ArrowverseBot.Minigames;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace ArrowverseBot.Handlers
{
	class MinigameHandler
	{
		private static readonly Color color = new Color(31, 139, 76);

		public static Trivia Trivia = new Trivia();
		
		public static async Task DisplayGames(SocketCommandContext context)
		{
			if (!await Utilities.CheckForChannel(context, 525378972989521948, context.User)) return;
			await context.Channel.SendMessageAsync("", false, Utilities.Embed("MiniGames", "Trivia\n`!trivia`\n\nTic-Tac-Toe\n`!ttt`\n\nNumber Guess\n`!play ng`\n\nRussian Roulette\n`!rr`\n\n8-Ball\n`!8ball`", new Color(31, 139, 76), "", ""));
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
				await context.Channel.SendMessageAsync("", false, Utilities.Embed("MiniGames", $"{context.User.Mention} has reset Trivia.", color, "", ""));
				Trivia.ResetTrivia();
			}
		
			else if (game == "")
				await Utilities.PrintError(context.Channel, "Please specify a game to reset.");
			else
				await Utilities.PrintError(context.Channel, $"I was unable to find the `{game}` game.\n\nAvailable games to reset:\nTrivia\n`!trivia`\n\nTic-Tac-Toe\n`!ttt`\n\nNumber Guess\n`!play ng`\n\nRussian Roulette\n`!rr`");
		}
	}
}