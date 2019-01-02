﻿using System;
using Discord;
using System.Linq;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ArrowverseBot.Handlers
{
	class CoinsHandler
	{
		private static Color color = new Color(215, 154, 14);
		private const string icon = "https://i.imgur.com/w09rWQg.png";

		private static async Task PrintEmbed(SocketCommandContext context, string description) => await context.Channel.SendMessageAsync("", false, Utilities.Embed("Coins", description, color, "", icon));
		private static async Task PrintEmbedNoFooter(SocketCommandContext context, string description) => await context.Channel.SendMessageAsync("", false, Utilities.Embed("Coins", description, color, "", ""));

		public static async Task GiveCoins(SocketCommandContext context, SocketGuildUser sender, SocketGuildUser reciever, int amount)
		{
			var SenderAccount = UserAccounts.GetAccount(sender);
			var RecieverAccount = UserAccounts.GetAccount(reciever);
			if (amount < 1)
			{
				await PrintEmbed(context, $"You must enter an amount greater than 1, {sender.Mention}.");
				return;
			}
			else if (amount > SenderAccount.coins)
			{
				await PrintEmbed(context, $"You do not have that many coins to send, {sender.Mention}.");
				return;
			}
			SenderAccount.coins -= amount;
			RecieverAccount.coins += amount;
			UserAccounts.SaveAccounts();
			await PrintEmbedNoFooter(context, $"{sender.Mention} gave {reciever.Mention} {amount} coins.");
		}

		public static async Task SpawnCoins(SocketCommandContext context, SocketGuildUser user, int amount)
		{
			UserAccounts.GetAccount(user).coins += amount;
			UserAccounts.SaveAccounts();
			await PrintEmbedNoFooter(context, $"Spawned {user.Mention} {amount} coins.");
		}

		public static async Task RemoveCoins(SocketCommandContext context, SocketGuildUser user, int amount)
		{
			AdjustCoins(user, -amount);
			await PrintEmbedNoFooter(context, $"{user.Mention} lost {amount} coins.");
		}

		public static void AdjustCoins(SocketGuildUser user, int amount)
		{
			var account = UserAccounts.GetAccount(user);
			account.coins += amount;
			if (account.coins < 0)
				account.coins = 0;
			UserAccounts.SaveAccounts();
		}

		public static async Task DisplayCoins(SocketCommandContext context, SocketGuildUser user, ISocketMessageChannel channel)
		{
			string name = user.Nickname != null ? user.Nickname : user.Username;
			await channel.SendMessageAsync("", false, Utilities.Embed($"{name}", $"{UserAccounts.GetAccount(user).coins.ToString("#,##0")} Coins", color, "", icon));
		}

		// Still in development
		public static async Task DisplayCoinsStore(SocketCommandContext context, SocketGuildUser user, ISocketMessageChannel channel)
		{
			await channel.SendMessageAsync("", false, Utilities.Embed($"Coins Store", $"500 XP - ??? Coins\n1000 XP - ???", color, $"You have {UserAccounts.GetAccount(user).coins} Coins.", icon));
		}

		private struct PickPocketUser { public SocketGuildUser user; public DateTime timeStamp; }
		private static List<PickPocketUser> PickPocketHistory = new List<PickPocketUser>();
		public static async Task PickPocket(SocketCommandContext context, SocketGuildUser target)
		{
			SocketGuildUser self = (SocketGuildUser)context.User;
			if (self == target)
			{
				await PrintEmbed(context, "You cannot pickpocket yourself");
				return;
			}
			foreach (PickPocketUser ppu in PickPocketHistory)
			{
				if (ppu.user == self)
				{
					if ((DateTime.Now - ppu.timeStamp).TotalHours <= 12)
					{
						string timeLeft = "";
						if ((12 - ((DateTime.Now - ppu.timeStamp).TotalHours)) < 1)
							timeLeft = $"{Math.Round(12 - ((DateTime.Now - ppu.timeStamp).TotalMinutes), 0)} minutes";
						else
							timeLeft = $"{Math.Round(12 - ((DateTime.Now - ppu.timeStamp).TotalHours), 0)} hours";
						await context.Channel.SendMessageAsync("", false, Utilities.Embed($"PickPocket", $"You must wait {timeLeft} before pickpocketing again.", color, "", icon));
						return;
					}
					PickPocketHistory.Remove(ppu);
				}
			}
			if (Utilities.GetRandomNumber(0, 2) == 0)
			{
				// Successful pickpocket
				int CoinsGained = (int)(UserAccounts.GetAccount(target).coins * 0.1);
				await context.Channel.SendMessageAsync("", false, Utilities.Embed($"PickPocket", $"{self.Mention} successfully pickpocketed {CoinsGained} coins from {target.Mention}.", color, "", icon));
				AdjustCoins(self, CoinsGained);
				AdjustCoins(target, -CoinsGained);
			}
			else
			{
				// Failed pickpocket
				int CoinsLost = (int)(UserAccounts.GetAccount(self).coins * 0.1);
				await context.Channel.SendMessageAsync("", false, Utilities.Embed($"PickPocket", $"{self.Mention} attempted to pickpocket {target.Mention} and failed, losing {CoinsLost} coins.", color, "", icon));
				AdjustCoins(self, -CoinsLost);
			}
			PickPocketHistory.Add(new PickPocketUser
			{
				user = self,
				timeStamp = DateTime.Now
			});
		}

		public static async Task PrintCoinsLeaderboard(SocketCommandContext context)
		{
			List<int> list = new List<int>();
			for (int i = 0; i < context.Guild.Users.Count; i++)
				list.Add(UserAccounts.GetAccount(context.Guild.Users.ElementAt(i)).coins);

			int[] MostCoinsArray = new int[5];
			int indexMin = 0;
			var IntArray = list.ToArray();
			MostCoinsArray[indexMin] = IntArray[0];
			int min = MostCoinsArray[indexMin];

			for (int i = 1; i < IntArray.Length; i++)
			{
				if (i < 5)
				{
					MostCoinsArray[i] = IntArray[i];
					if (MostCoinsArray[i] < min)
					{
						min = MostCoinsArray[i];
						indexMin = i;
					}
				}
				else if (IntArray[i] > min)
				{
					min = IntArray[i];
					MostCoinsArray[indexMin] = min;
					for (int r = 0; r < 5; r++)
					{
						if (MostCoinsArray[r] < min)
						{
							min = MostCoinsArray[r];
							indexMin = r;
						}
					}
				}
			}

			var embed = new EmbedBuilder().WithTitle("Coins Leaderboard").WithColor(color);

			Array.Sort(MostCoinsArray);
			Array.Reverse(MostCoinsArray);
			List<SocketGuildUser> PeopleOnLB = new List<SocketGuildUser>();
			for (int i = 0; i < 5; i++)
			{
				for (int n = 0; n < context.Guild.Users.Count; n++)
				{
					if (UserAccounts.GetAccount(context.Guild.Users.ElementAt(n)).coins == MostCoinsArray[i] && !PeopleOnLB.Contains(context.Guild.Users.ElementAt(n)))
					{
						string name = context.Guild.Users.ElementAt(n).Nickname != null ? context.Guild.Users.ElementAt(n).Nickname : context.Guild.Users.ElementAt(n).Username;
						embed.AddField($"{i + 1} - {name}", MostCoinsArray[i] + " Coins");
						PeopleOnLB.Add(context.Guild.Users.ElementAt(n));
						break;
					}
				}
			}

			await context.Channel.SendMessageAsync("", false, embed.Build());
		}

		private static bool isLotteryGoing = false;
		private static List<SocketGuildUser> PeopleEnteredInLottery;
		private static int LotteryFee = 0;
		private static int LotteryPrize = 0;

		public static async Task StartCoinsLottery(SocketCommandContext context, int amount, int cost)
		{
			if (!await Utilities.CheckForSuperadmin(context, context.User)) return;
			if (!await Utilities.CheckForChannel(context, 518846214603669537, context.User)) return;
			if (isLotteryGoing)
			{
				await Utilities.PrintError(context.Channel, $"A lottery is already active, {context.User.Mention}.");
				return;
			}

			isLotteryGoing = true;
			LotteryFee = cost;
			LotteryPrize = amount;
			PeopleEnteredInLottery = new List<SocketGuildUser>();
			await PrintEmbed(context, $"{context.User.Mention} has started a lottery for {amount} coins!\n\nType `!coins lottery join` to enter!\n\nIt cost `{cost}` Coins!");
		}

		public static async Task JoinCoinsLottery(SocketCommandContext context)
		{
			if (!isLotteryGoing)
			{
				await Utilities.PrintError(context.Channel, $"There is no active lottery, {context.User.Mention}.");
				return;
			}
			if (UserAccounts.GetAccount(context.User).coins < LotteryFee)
			{
				await Utilities.PrintError(context.Channel, $"You do not have enough Coins to enter the lottery, {context.User.Mention}.");
				return;
			}

			AdjustCoins((SocketGuildUser)context.User, -LotteryFee);
			PeopleEnteredInLottery.Add((SocketGuildUser)context.User);
			await PrintEmbed(context, $"{context.User.Mention} has joined the lottery!\n\n{PeopleEnteredInLottery.Count} currently entered.");
		}

		public static async Task DrawLottery(SocketCommandContext context)
		{
			if (!await Utilities.CheckForSuperadmin(context, context.User)) return;
			if (!await Utilities.CheckForChannel(context, 518846214603669537, context.User)) return;
			if (!isLotteryGoing)
			{
				await Utilities.PrintError(context.Channel, $"There is no active Lottery, {context.User.Mention}.");
				return;
			}

			SocketGuildUser winner;
			winner = PeopleEnteredInLottery.ElementAt(Utilities.GetRandomNumber(1, PeopleEnteredInLottery.Count));
			AdjustCoins(winner, LotteryPrize);
			await PrintEmbed(context, $"{winner.Mention} has won {LotteryPrize} coins!\n\nThanks for playing!");
			await ResetCoinsLottery(context, false);
		}

		public static async Task ResetCoinsLottery(SocketCommandContext context, bool isFromUser)
		{
			if (isFromUser)
			{
				if (!await Utilities.CheckForSuperadmin(context, context.User)) return;
				await PrintEmbed(context, $"{context.User.Mention} has reset the Lottery.");
			}
			isLotteryGoing = false;
			LotteryFee = 0;
			LotteryPrize = 0;
			PeopleEnteredInLottery = null;
		}
	}
}
