using System;
using Discord;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord.Commands;
using Discord.Audio;
using System.Threading;
using System.IO;
using Discord.Audio;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using NAudio.Wave;
using System.Media;

namespace ArrowverseBot.Handlers
{
    public class AudioModule : ModuleBase<SocketCommandContext>
    {
        
        private readonly AudioService _service;

       
        public AudioModule(AudioService service)
        {
            _service = service;
        }
        static ISocketMessageChannel c;
        static SocketGuild Guild;
        static IAudioClient audioClient;


        [Command("join", RunMode = RunMode.Async)]
        public async Task Join(SocketCommandContext Context)
        {
            var channel = Context.Guild.GetVoiceChannel(543828840380760085);
            c = Context.Guild.GetTextChannel(543877449767845898);
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