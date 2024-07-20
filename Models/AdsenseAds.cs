using Microsoft.Extensions.Configuration;
using TheVoid.Interfaces;

namespace TheVoid.Models
{
    public class AdsenseAds : IAdsHandler
    {
        public int AdChance { get; set; }

        public bool CanReceieveRandomAd
        {
            get
            {
                int randomNumber = _rnd.Next(0, 100);
                return randomNumber <= AdChance;
            }
        }

        private readonly Random _rnd = new();

        public AdsenseAds(IConfiguration configuration)
        {
            AdChance = configuration.GetValue<int>("AddChance");
        }
    }
}
