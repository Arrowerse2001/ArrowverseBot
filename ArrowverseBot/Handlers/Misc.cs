using System;
using Discord;
using System.Text;
using System.Linq;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ArrowverseBot.Handlers;
using ArrowverseBot.Minigames;

namespace ArrowverseBot.Handlers
{
	[RequireContext(ContextType.Guild)]
	public class Misc : ModuleBase<SocketCommandContext>
	{
		//Reset A Game

		[Command("reset")]
		public async Task ResetAGame([Remainder]string game = "") => await MinigameHandler.ResetGame(Context, game);



		[Command("pickpocket")]
		public async Task PickPocketCoins(SocketGuildUser user) => await CoinsHandler.PickPocket(Context, user);

		// Start a Coins Lottery
		[Command("coins lottery start")]
		public async Task StartCoinsLottery(int amount, int cost) => await CoinsHandler.StartCoinsLottery(Context, amount, cost);

		// Join the Coins Lottery
		[Command("coins lottery join")]
		public async Task JoinCoinsLottery() => await CoinsHandler.JoinCoinsLottery(Context);

		// Draw the Coins Lottery
		[Command("coins lottery draw")]
		public async Task DrawCoinsLottery() => await CoinsHandler.DrawLottery(Context);

		// Reset Coins Lottery
		[Command("coins lottery reset")]
		public async Task ResetCoinsLottery() => await CoinsHandler.ResetCoinsLottery(Context, true);

		// Spawn Coins for a user
		[Command("coins spawn")]
		public async Task SpawnCoins(SocketGuildUser user, [Remainder]int amount)
		{
			if (!await Utilities.CheckForSuperadmin(Context, Context.User)) return;
			await CoinsHandler.SpawnCoins(Context, user, amount);
		}

 

		// Remove Coins for a user
		[Command("coins remove")]
		public async Task RemoveCoins(SocketGuildUser user, [Remainder]int amount)
		{
			if (!await Utilities.CheckForSuperadmin(Context, Context.User)) return;
			await CoinsHandler.RemoveCoins(Context, user, amount);
		}

		// See how many Coins another user has
		[Command("coins")]
		public async Task SeeCoins(SocketGuildUser user = null) => await CoinsHandler.DisplayCoins(Context, user ?? (SocketGuildUser)Context.User, Context.Channel);

		// Give Coins to another user (not spawning them)
		[Command("coins give")]
		public async Task GiveCoins(SocketGuildUser user, [Remainder]int amount) => await CoinsHandler.GiveCoins(Context, (SocketGuildUser)Context.User, user, amount);

		// Coins Store
		[Command("coins store")]
		public async Task CoinsStore() => await CoinsHandler.DisplayCoinsStore(Context, (SocketGuildUser)Context.User, Context.Channel);

		// Leaderboard Shortcut
		[Command("lb coins")]
		public async Task CoinsLBShortcut() => await CoinsLeaderboard();

		// Print the Coins leaderboard
		[Command("leaderboard coins")]
		public async Task CoinsLeaderboard() => await CoinsHandler.PrintCoinsLeaderboard(Context);



		//Server Stats
		[Command("serverstats")]
		public async Task ServerStats() => await StatsHandler.DisplayServerStats(Context);


		


		// Trivia menu & modes
		[Command("trivia")]
		public async Task TryToStartTrivia(string input = null) => await MinigameHandler.Trivia.TryToStartTrivia((SocketGuildUser)Context.User, Context, input ?? "trivia");


		

	}
}