using System.Threading.Tasks;
using Discord.Commands;
using ArrowverseBot.Commands;
using Discord;

public class AudioModule : ModuleBase<ICommandContext>
{
    // Scroll down further for the AudioService.
    // Like, way down
    private readonly AudioService _service;

    // Remember to add an instance of the AudioService
    // to your IServiceCollection when you initialize your bot
    public AudioModule(AudioService service)
    {
        _service = service;
    }

    [Command("join", RunMode = RunMode.Async)]
    public async Task JoinCmd()
    {
        await _service.JoinAudio(Context.Guild, (Context.User as IVoiceState).VoiceChannel);
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