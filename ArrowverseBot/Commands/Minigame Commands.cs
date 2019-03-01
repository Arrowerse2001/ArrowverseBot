using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace ArrowverseBot.Commands
{
    class Minigame_Commands
    {
        [RequireContext(ContextType.Guild)]
        public class Misc : ModuleBase<SocketCommandContext>
        {
            // Trivia Menu and/or Start Trivia
            [Command("trivia")]
            public async Task TryToStartTrivia(string input = null) => await Handlers.MinigameHandler.Trivia.TryToStartTrivia((SocketGuildUser)Context.User, Context, input ?? "trivia");
        }
    }
}
