using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            "https://cdn.discordapp.com/attachments/524633291756797952/543820908565889024/49913202_783979151955586_9006569575642018506_n.png",
            "https://cdn.discordapp.com/attachments/524633291756797952/543821142784475146/49376198_127543714944463_5652016525521471429_n.png",
            "https://cdn.discordapp.com/attachments/524633291756797952/543821312917897217/45532247.png"
        };
    }
}
