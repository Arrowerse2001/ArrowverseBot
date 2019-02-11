using System.Threading.Tasks;
using Discord.Commands;
using ArrowverseBot.Handlers;
using Discord.Audio;
using Discord;
using Discord.WebSocket;

namespace ArrowverseBot.Handlers
{
    public class AudioModule : ModuleBase<ICommandContext>
    {
        
        private readonly AudioService _service;

       
        public AudioModule(AudioService service)
        {
            _service = service;
        }
        static ISocketMessageChannel c;
        static SocketGuild Guild;
        static SocketCommandContext context;
        static IAudioClient audioClient;


        [Command("join", RunMode = RunMode.Async)]
        public async Task Join(SocketCommandContext Context)
        {
            context = Context;
            var channel = Context.Guild.GetVoiceChannel(294699220743618562);
            c = Context.Guild.GetTextChannel(518186074162331648);
            Guild = Context.Guild;

            (await channel.ConnectAsync()).Dispose();

            audioClient = await channel.ConnectAsync();

        }


        [Command("leave", RunMode = RunMode.Async)]
        public async Task LeaveCmd()
        {
            await _service.LeaveAudio(Context.Guild);
        }

        
        [Command("play", RunMode = RunMode.Async)]
        public async Task PlayCmd([Remainder] string song)
        {
            await _service.SendAudioAsync(Context.Guild, Context.Channel, song);
        }


       
    }
}