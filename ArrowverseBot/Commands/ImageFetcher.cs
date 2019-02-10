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
         "https://i.imgur.com/e1OjqO1g.png",
         "https://i.imgur.com/zdEjTDO.png",
         "https://i.imgur.com/nCUaAPs.jpg",
         "https://i.imgur.com/E5SGrpj.png"
        };
    }
}
