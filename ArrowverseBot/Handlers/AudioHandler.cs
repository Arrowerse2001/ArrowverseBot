/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.Net;
using ArrowverseBot.Handlers;
using ArrowverseBot.Commands;
//using System.Speech.Synthesiser;
using Discord.Audio;
using System.Speech.Synthesis;


namespace ArrowverseBot.Handlers
{
    class AudioHandler
    {
        public async Task Join(SocketCommandContext Context)
        {
            context = Context;
            var channel = Context.Guild.GetVoiceChannel(294699220743618562);
            c = Context.Guild.GetTextChannel(518186074162331648);
            Guild = Context.Guild;

            (await channel.ConnectAsync()).Dispose();

            audioClient = await channel.ConnectAsync();

            synthesizer.Volume = 100;
            synthesizer.Rate = 1;
           
            synthesizer.SetOutputToWaveFile("_voice.wav");

            await Speak("Hello");

            

            StartRecognition();
        }
    }
}


System.Speech.dll is not compatible with this version of .Net Framework

*/

