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

        #region Coin Related Commands
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
        #endregion


        //Server Stats
        [Command("serverstats")]
		public async Task ServerStats() => await StatsHandler.DisplayServerStats(Context);

		// View Stats for a movie
		[Command("movie")]
		public async Task SearchMovie([Remainder]string search)
		{
			MediaFetcher.Movie media = MediaFetcher.FetchMovie(search);

			string RTScore = "N/A", IMDBScore;

			for (int i = 0; i < media.Ratings.Length; i++)
				if (media.Ratings[i].Source == "Rotten Tomatoes") RTScore = media.Ratings[i].Value;

			IMDBScore = media.imdbRating == "N/A" ? "N/A" : $"{media.imdbRating}/10";

			await Context.Channel.SendMessageAsync(null, false, new EmbedBuilder()
				.WithTitle($":film_frames: {media.Title} ({media.Year})")
				.WithThumbnailUrl(media.Poster)
				.WithDescription(media.Plot)
				.AddField("Director", media.Director)
				.AddField("Runtime", media.Runtime)
				.AddField("Box Office", media.BoxOffice)
				.AddField("IMDB Score", IMDBScore)
				.AddField("Rotten Tomatoes", RTScore)
				.WithColor(Utilities.DomColorFromURL(media.Poster))
				.Build());
		}

		// View stats for a TV show
		[Command("tv")]
		public async Task SearchShows([Remainder]string search)
		{
			MediaFetcher.Movie media = MediaFetcher.FetchMovie(search);

			string IMDBScore = media.imdbRating == "N/A" ? "N/A" : $"{media.imdbRating}/10";
			media.Year = media.Year.Replace("â€“", "-");

			await Context.Channel.SendMessageAsync(null, false, new EmbedBuilder()
				.WithTitle($":film_frames: {media.Title} ({media.Year})")
				.WithThumbnailUrl(media.Poster)
				.WithDescription(media.Plot)
				.AddField("Runtime", media.Runtime)
				.AddField("IMDB Score", IMDBScore)
				.WithColor(Utilities.DomColorFromURL(media.Poster))
				.Build());
		}
    }
}