using Microsoft.Extensions.Configuration;
using TheVoid.Interfaces;

namespace TheVoid.Models
{
    public class AddsenseAdds : IAddsHandler
    {
        public int AddChance { get; set; }

        public bool CanReceieveRandomAdd
        {
            get
            {
                int randomNumber = _rnd.Next(0, 100);
                return randomNumber <= AddChance;
            }
        }

        private readonly Random _rnd = new();

        public AddsenseAdds(IConfiguration configuration)
        {
            AddChance = configuration.GetValue<int>("AddChance");
        }
    }
}
