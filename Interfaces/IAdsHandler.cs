using TheVoid.ViewModels;

namespace TheVoid.Interfaces
{
    public interface IAdsHandler
    {
        public int AdChance { get; set; }

        public bool CanReceieveRandomAd { get;}

        //request adds, load adds, delay after one add was shown
        //is add loaded, add so you can't just leave and come back but need to watch the add
    }
}
