using System;

namespace ArrowverseBot.Commands
{
    class ImageFetcher
    {
        private static readonly Random getrandom = new Random();

        private static int GetRandomNumber(int min, int max)
        {
            lock (getrandom) { return getrandom.Next(min, max); }
        }

        public string[] Harry =
        {
            "https://cdn.discordapp.com/attachments/524633291756797952/543820714956947487/9k.png",
            "https://cdn.discordapp.com/attachments/524633291756797952/543820714956947487/9k.png" // add more
        };
    }
}
