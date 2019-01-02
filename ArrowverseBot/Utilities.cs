using System;
using Discord;
using System.IO;
using System.Net;
using System.Drawing;
using ColorThiefDotNet;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using ArrowverseBot.Handlers;

namespace ArrowverseBot
{
	class Utilities
	{
		// Universal Web Client
		public static WebClient webClient = new WebClient();


		// random number
		public static readonly Random getrandom = new Random();
		public static int GetRandomNumber(int min, int max)
		{
			lock (getrandom) { return getrandom.Next(min, max); }
		}

		// Embeds
		public static Embed Embed(string t, string d, Discord.Color c, string f, string thURL) => new EmbedBuilder()
			.WithTitle(t)
			.WithDescription(d)
			.WithColor(c)
			.WithFooter(f)
			.WithThumbnailUrl(thURL)
			.Build();

		// Picture Embed
		public static Embed ImageEmbed(string t, string d, Discord.Color c, string f, string imageURL) => new EmbedBuilder()
			.WithTitle(t)
			.WithDescription(d)
			.WithColor(c)
			.WithFooter(f)
			.WithImageUrl(imageURL)
			.Build();

		// Print an error
		public static async Task PrintError(ISocketMessageChannel channel, string description) => await channel.SendMessageAsync("", false, Embed("Error", description, new Discord.Color(227, 37, 39), "", ""));





		// Checks if a user is a superadmin, this is to see if they can do a certain command
		public static async Task<bool> CheckForSuperadmin(SocketCommandContext context, SocketUser user)
		{
			if (UserAccounts.GetAccount(user).superadmin)
				return true;
			await PrintError(context.Channel, $"You do not have permission to do that command, {user.Mention}.");
			return false;
		}

		// Checks if the current channel is the required channel (like minigames)
		public static async Task<bool> CheckForChannel(SocketCommandContext context, ulong requiredChannel, SocketUser user)
		{
			if (context.Channel.Id == requiredChannel)
				return true;
			await PrintError(context.Channel, $"Please use the {context.Guild.GetTextChannel(requiredChannel).Mention} chat for that, {user.Mention}.");
			return false;
		}
	}
}

