using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace ArrowverseBot.Handlers
{
    [RequireContext(ContextType.Guild)]
    public class CommandDefinitions : ModuleBase<SocketCommandContext>
    {
        #region TV Show Commands
        [Command("supergirl")]
        public async Task DisplaySupergirlStuff()
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle("Supergirl")
                .WithDescription("At 12 years old, Kara Zor-El escapes doom on planet Krypton to find protection on Earth with the Danver family, where she grows up in the shadow of her foster sister, Alex, and learns to hide the extraordinary powers she shares with her cousin, Superman. Now an adult living in National City and working for media mogul Cat Grant, Kara finds her days of keeping her abilities a secret are over when super-secret agency head Hank Henshaw enlists her to help protect the city's citizens from threats. Finally coming into her own, Kara must juggle her new responsibilities with her very human relationships.")
                .WithColor(Color.Blue)
                .WithThumbnailUrl("https://cdn.discordapp.com/attachments/524633291756797952/526178091668733977/9k.png");

            await ReplyAsync("", false, builder.Build());
        }

        [Command("arrow")]
        public async Task DisplayArrowStuff()
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle("Arrow")
                .WithDescription("When presumed-dead billionaire playboy Oliver Queen returns home to Starling City after five years stranded on a remote island in the Pacific, he hides the changes the experience had on him, while secretly seeking reconciliation with his ex, Laurel. By day he picks up where he left off, playing the carefree philanderer he used to be, but at night he dons the alter ego of Arrow and works to right the wrongs of his family and restore the city to its former glory. Complicating his mission is Laurel's father, Detective Quentin Lance, who is determined to put the vigilante behind bars.")
                .WithColor(Color.Blue)
                .WithThumbnailUrl("https://cdn.discordapp.com/attachments/524633291756797952/526113431149871115/Z.png");

            await ReplyAsync("", false, builder.Build());
        }

        [Command("flash")]
        public async Task DisplayFlashStuff()
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle("The Flash")
                .WithDescription("At 11, Barry Allen's life changed completely when his mother died in a freak accident and his innocent father was convicted of her murder. Orphaned Barry later becomes Detective Joe West. Now a crime-scene investigator, his dedication to learn the truth about his mother's death drives him to follow up on every new scientific advancement and urban legend. When his latest obsession - a particle accelerator heralded as a world-changing invention - causes an explosion, it creates a freak storm and Barry is struck by lightning. He awakes from a coma nine months later with the power of superspeed. When he learns that others who have gained powers use them for evil, he dedicates himself to protecting the innocent, while still trying to solve the older mystery.")
                .WithColor(Color.Blue)
                .WithThumbnailUrl("https://cdn.discordapp.com/attachments/524633291756797952/526154633614196736/MV5BMjI1MDAwNDM4OV5BMl5BanBnXkFtZTgwNjUwNDIxNjM.png")
                .WithFooter("TV Show: The Flash (5 Seasons)");

            await ReplyAsync("", false, builder.Build());
        }

        [Command("legends")]
        public async Task DisplayLegendsStuff()
        {
            EmbedBuilder builder = new EmbedBuilder();

            builder.WithTitle("Legends Of Tomorrow")
                .WithDescription("DC's Legends of Tomorrow, or simply Legends of Tomorrow, is an American superhero television series developed by Greg Berlanti, Marc Guggenheim, Andrew Kreisberg, and Phil Klemmer, who are also executive producers along with Sarah Schechter and Chris Fedak; Klemmer and Fedak serve as showrunners.")
                .WithColor(Color.Blue)
                .WithThumbnailUrl("https://cdn.discordapp.com/attachments/524633291756797952/526457636334469123/2Q.png")
                .WithFooter("Source: Google.co.uk");

            await ReplyAsync("", false, builder.Build());
        }
        #endregion

        #region Trivia Commands
        // Trivia Menu and/or Start Trivia
        [Command("trivia")]
        public async Task TryToStartTrivia(string input = null) => await Config.MinigameHandler.Trivia.TryToStartTrivia((SocketGuildUser)Context.User, Context, input ?? "trivia");
        #endregion
    }
}