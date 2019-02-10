using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Net;
using Discord.Commands;
using ArrowverseBot.Handlers;
using Discord.WebSocket;

namespace ArrowverseBot.Commands
{
    class Minigame_Commands
    {
        [RequireContext(ContextType.Guild)]
        public class Misc : ModuleBase<SocketCommandContext>
        {



            #region Trivia Commands
            // Trivia Menu and/or Start Trivia
            [Command("trivia")]
            public async Task TryToStartTrivia(string input = null) => await Handlers.MinigameHandler.Trivia.TryToStartTrivia((SocketGuildUser)Context.User, Context, input ?? "trivia");
            #endregion
        }
    }
}
