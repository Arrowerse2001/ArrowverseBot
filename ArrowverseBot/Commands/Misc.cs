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
using System.Net.Http;

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


        [Command("delete")]
        public async Task DeleteMessage(int amount)
        {
            if (!await Utilities.CheckForSuperadmin(Context, Context.User)) return;
            var channel = (ITextChannel)Context.Channel;
            var messages = await channel.GetMessagesAsync(++amount).Flatten().ToList();
            await channel.DeleteMessagesAsync(messages);
        }

        [Command("say")]
        public async Task Say([Remainder]string message)
        {
            if (!await Utilities.CheckForSuperadmin(Context, Context.User)) return;
            await Context.Channel.DeleteMessageAsync(Context.Message.Id);
            await Context.Channel.SendMessageAsync(message);
        }

        [Command("dm")]
        public async Task DM(SocketGuildUser target, [Remainder]string message)
        {
            if (!await Utilities.CheckForSuperadmin(Context, Context.User)) return;
            await Context.Channel.DeleteMessageAsync(Context.Message.Id);
            await target.SendMessageAsync(message);
        }

        [Command("color")]
        public async Task GetDomColor(string url)
        {
            var color = Utilities.DomColorFromURL(url);
            await Utilities.SendEmbed(Context.Channel, "Dominant Color", $"The dominant color for the image is:\n\nHexadecimal:\n`#{color.R:X2}{color.G:X2}{color.B:X2}`\n\nRGB:\n`Red: {color.R}`\n`Green: {color.G}`\n`Blue: {color.B}`", color, "", url);
        }

        [Command("onlinecount")]
        public async Task CountUsersOnline() => await Context.Channel.SendMessageAsync($"There are currently {Context.Guild.Users.ToArray().Length} members online.");

        [Command("joined")]
        public async Task JoinedAt(SocketGuildUser user = null) => await Context.Channel.SendMessageAsync(StatsHandler.GetJoinedDate((user ?? (SocketGuildUser)Context.User)));

        [Command("created")]
        public async Task Created(SocketGuildUser user = null) => await Context.Channel.SendMessageAsync(StatsHandler.GetCreatedDate(user ?? Context.User));

        [Command("avatar")]
        public async Task GetAvatar(SocketGuildUser user = null) => await Context.Channel.SendMessageAsync("", false, Utilities.ImageEmbed("", "", Utilities.DomColorFromURL((user ?? Context.User).GetAvatarUrl()), "", (user ?? Context.User).GetAvatarUrl().Replace("?size=128", "?size=512")));


        // See stats for a certain user
        [Command("stats")]
        public async Task DisplayUserStats(SocketGuildUser user = null) => await StatsHandler.DisplayUserStats(Context, user ?? (SocketGuildUser)Context.User);


        [Command("harry")]
        public async Task RandomAlaniPic()
        {
            string pic = Config.bot.harryPics[Utilities.GetRandomNumber(0, Config.bot.harryPics.Count)];
            await Context.Channel.SendMessageAsync("", false, Utilities.ImageEmbed("", "", Utilities.DomColorFromURL(pic), "", pic));
        }

        // see custom emojis
        [Command("emotes")]
        [Alias("emojis")]
        public async Task ViewServerEmotes() => await Utilities.SendEmbed(Context.Channel, $"Server Emotes ({Context.Guild.Emotes.Count})", string.Join("", Context.Guild.Emotes), Colours.Blue, "", "");


        [Command("jumbo")]
        public async Task PrintEmojis([Remainder]string input)
        {
            var emojis = input.Split('>');
            foreach (var s in emojis)
                await PrintEmoji(s + ">").ConfigureAwait(false);
        }

        private async Task PrintEmoji(string emoji)
        {
            string url = $"https://cdn.discordapp.com/emojis/{Regex.Replace(emoji, "[^0-9.]", "")}.{(emoji.StartsWith("<a:") ? "gif" : "png")}";
            await Context.Channel.SendMessageAsync("", false, Utilities.ImageEmbed("", "", Utilities.DomColorFromURL(url), "", url));
        }

        [Command("create emote")]
        public async Task MakEmote(string url, string name)
        {


            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(new Uri(url));
                var emote = await Context.Guild.CreateEmoteAsync(name, new Image(await response.Content.ReadAsStreamAsync()));
                await Utilities.PrintSuccess(Context.Channel, $"Created: {emote}");
            }

        }


    }
}

