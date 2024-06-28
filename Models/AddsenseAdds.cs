using TheVoid.Interfaces;
using TheVoid.ViewModels;

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

        private Random _rnd = new();
        public AddsenseAdds(int addChance)
        {
            AddChance = addChance;
        }
    }
}
