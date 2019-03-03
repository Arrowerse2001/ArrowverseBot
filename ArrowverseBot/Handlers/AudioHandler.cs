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
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using NAudio.Wave;
using System.Media;
using System.Speech.Synthesis;

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
        public async Task Join()
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();


            var channel = Context.Guild.GetVoiceChannel(548271687452327937);
            c = Context.Guild.GetTextChannel(551081838748237836);
            Guild = Context.Guild;

            (await channel.ConnectAsync()).Dispose();

            audioClient = await channel.ConnectAsync();

            synth.Volume = 100;
            synth.Rate = 1;
            synth.GetInstalledVoices();
            synth.SetOutputToWaveFile("_voice.wav");

            synth.Speak("Hello");

          
        }
        
        [Command("speak", RunMode = RunMode.Async)]
        public async Task Speakcmd()
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();


            synth.Volume = 100;
            synth.Rate = 1;
            synth.GetInstalledVoices();
            synth.SetOutputToWaveFile("_voice.wav");

            synth.Speak("test test test test test test test test");

            



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