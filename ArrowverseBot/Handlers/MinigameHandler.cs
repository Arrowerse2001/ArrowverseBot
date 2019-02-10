﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using ArrowverseBot.Minigames;

namespace ArrowverseBot.Handlers
{
    class MinigameHandler
    {
        private static readonly Color color = new Color(31, 139, 76);




        public Trivia Trivia = new Trivia();
        public static WhoSaidIt WSI = new WhoSaidIt();


        public static async Task DisplayGames(SocketCommandContext context)
        {
            if (!await Utilities.CheckForChannel(context, 543877449767845898, context.User)) return;
            await context.Channel.SendMessageAsync("", false, Utilities.Embed("MiniGames", "Trivia\n`!trivia`\n\nTic-Tac-Toe\n`!ttt`\n\nNumber Guess\n`!play ng`\n\nRussian Roulette\n`!rr`\n\n8-Ball\n`!8ball`", new Color(31, 139, 76), "", ""));
        }

        // Reset a game
        public static async Task ResetGame(SocketCommandContext context, string game)
        {
            if (!await Utilities.CheckForSuperadmin(context, context.User)) return;
            else if (game == "trivia")
            {
                await context.Channel.SendMessageAsync("", false, Utilities.Embed("MiniGames", $"{context.User.Mention} has reset Trivia.", color, "", ""));
                Config.MinigameHandler.Trivia.ResetTrivia();
            }
            else if (game == "wsi")
            {
                await context.Channel.SendMessageAsync("", false, Utilities.Embed("MiniGames", $"{context.User.Mention} has reset the Who Said It.", color, "", ""));
                WSI.Reset();
            }
            else if (game == "")
                await Utilities.PrintError(context.Channel, "Please specify a game to reset.");
            else
                await Utilities.PrintError(context.Channel, $"I was unable to find the `{game}` game.\n\nAvailable games to reset:\nTrivia\n`!trivia`\n\nTic-Tac-Toe\n`!ttt`\n\nNumber Guess\n`!play ng`\n\nRussian Roulette\n`!rr`");

        }
    }
    }