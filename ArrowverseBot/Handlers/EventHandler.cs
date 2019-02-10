using System;
using Discord;
using System.Linq;
using Discord.Commands;
using System.Reflection;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace ArrowverseBot.Handlers
{
    class EventHandler
    {
        DiscordSocketClient _client;
        CommandService _service;


        public async Task InitializeAsync(DiscordSocketClient client)
        {
            _client = client;
            _service = new CommandService();
            AudioService AudioService = new AudioService();

        await _service.AddModulesAsync(Assembly.GetEntryAssembly(), null);

            _client.MessageReceived += HandleCommandAsync;

            _client.UserBanned += HandleUserBanned;
            _client.UserUnbanned += HandleUserUnbanned;

            _client.UserJoined += HandleUserJoining;
            _client.UserLeft += HandleUserLeaving;

            _service.Log += Log;


            _client.MessageDeleted += HandleMessageDeleted;
        }

        private Task Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        private async Task HandleMessageDeleted(Cacheable<IMessage, ulong> arg1, ISocketMessageChannel arg2)
        {
            var msg = arg1.HasValue ? arg1.Value : null;
            await arg2.SendMessageAsync("", false, new EmbedBuilder()
               .WithTitle("Deleted Message")
               .AddField("Message", msg.Content)
               .AddField("User", msg.Author)
               .AddField("User ID", msg.Author.Id)
               .AddField("Time", msg.Timestamp.ToString("h:mm tt, dddd, MMMM d."))
               .WithColor(new Color(31, 139, 76))
               .WithThumbnailUrl(msg.Author.GetAvatarUrl())
               .Build());
        }

        private async Task HandleUserUnbanned(SocketUser arg1, SocketGuild arg2) => await arg2.GetTextChannel(509099311661842433).SendMessageAsync("", false, Utilities.Embed("Pardon", $"{arg1} has been unbanned.", new Color(31, 139, 76), "", arg1.GetAvatarUrl()));

        private async Task HandleUserBanned(SocketUser arg1, SocketGuild arg2)
        {
            var bans = arg2.GetBansAsync().Result.ToList();
            string reason = "";
            foreach (var ban in bans)
                if (ban.User.Id == arg1.Id)
                    reason = ban.Reason;
            if (reason == "")
                await arg2.GetTextChannel(509099311661842433).SendMessageAsync("", false, Utilities.Embed("Ban", $"{arg1} has been banned.", new Color(231, 76, 60), "", arg1.GetAvatarUrl()));
            else
                await arg2.GetTextChannel(509099311661842433).SendMessageAsync("", false, Utilities.Embed("Ban", $"{arg1} has been banned for {reason}.", new Color(231, 76, 60), "", arg1.GetAvatarUrl()));
        }

        private async Task HandleUserJoining(SocketGuildUser arg)
        {
            if (arg.IsBot)
            {
                await (arg as IGuildUser).AddRoleAsync(arg.Guild.Roles.FirstOrDefault(x => x.Name == "Bot"));
                await arg.Guild.GetTextChannel(509099311661842433).SendMessageAsync("", false, Utilities.Embed("New Bot", $"The {arg.Username} bot has been added to the server.", new Color(31, 139, 76), "", arg.GetAvatarUrl()));
                return;
            }
            string desc = $"{arg} has joined the server.";
            if (UserAccounts.GetAccount(arg).level != 0)
            {
            }
            await arg.Guild.GetTextChannel(509099311661842433).SendMessageAsync("", false, Utilities.Embed("New User", desc, new Color(31, 139, 76), "", arg.GetAvatarUrl()));
        }

        private async Task HandleUserLeaving(SocketGuildUser arg)
        {
            if (arg.IsBot)
                await arg.Guild.GetTextChannel(509099311661842433).SendMessageAsync("", false, Utilities.Embed("Bot Removed", $"The {arg.Username} bot has been removed from the server.", new Color(231, 76, 60), "", arg.GetAvatarUrl()));
            else
                await arg.Guild.GetTextChannel(509099311661842433).SendMessageAsync("", false, Utilities.Embed("User Left", $"{arg} has left the server. I wonder what he fucked up in the timeline.", new Color(231, 76, 60), "", arg.GetAvatarUrl()));
        }

        string[] spellingMistakes = { "should of", "would of", "wouldnt of", "wouldn't of", "would not of", "couldnt of", "couldn't of", "could not of", "better of", "shouldnt of", "shouldn't of", "should not of", "alot", "could of" };
        string[] spellingFix = { "should have", "would have", "wouldn't have", "wouldn't have", "would not have", "couldn't have", "couldn't have", "could not have", "better have", "shouldn't have", "shouldn't have", "should not have", "a lot", "could have" };

        private async Task HandleCommandAsync(SocketMessage s)
        {
            SocketUserMessage msg = s as SocketUserMessage;
            if (msg == null || msg.Author.IsBot) return;

            var context = new SocketCommandContext(_client, msg);

            int argPos = 0;
            if (msg.HasStringPrefix("!", ref argPos))
                await _service.ExecuteAsync(context, argPos, null, MultiMatchHandling.Exception);

            string m = msg.Content.ToLower();


            // Answer minigames
            if (context.Channel.Id == 509099311661842433)
            {
                {
                    // Answer Trivia
                    if (m == "a" || m == "b" || m == "c" || m == "d")
                        await MinigameHandler.Trivia.AnswerTrivia((SocketGuildUser)msg.Author, context, m);

                    
                }



                // Print a lennyface
                if (m.Contains("lennyface"))
                    await context.Channel.SendMessageAsync("( ͡° ͜ʖ ͡°)");

                // Fix some spelling mistakes
                for (int i = 0; i < spellingMistakes.Length; i++)
                    if (m.Contains(spellingMistakes[i]))
                        await msg.Channel.SendMessageAsync(spellingFix[i] + "*");

                // Print a DM message to console
                if (s.Channel.Name.StartsWith("@"))
                    Console.WriteLine($" ----------\n DIRECT MESSAGE\n From: {s.Channel}\n {s}\n ----------");
            }
        }
    }
}
