using System;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ArrowverseBot.Handlers.AudioServ;

namespace ArrowverseBot
{
    class Program
    {
        public static DiscordSocketClient _client;
        private IServiceProvider _services;

        static void Main(string[] args) => new Program().StartAsync().GetAwaiter().GetResult();

        public async Task StartAsync()
        {
            if (Config.bot.DisordBotToken == "" || Config.bot.DisordBotToken == null) return;
            _client = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = LogSeverity.Verbose });
            _client.Log += Log;
            await _client.LoginAsync(TokenType.Bot, Config.bot.DisordBotToken);
            await _client.StartAsync();
            // displays a activity
            await _client.SetGameAsync("Lucifer", null, ActivityType.Watching);

            _services = new ServiceCollection().AddSingleton(new AudioService()).BuildServiceProvider();

            var _handler = new Handlers.EventHandler(_services);
            await _handler.InitializeAsync(_client);
            await Task.Delay(-1);
        }

        private static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
            return Task.CompletedTask;
        }
    }
}