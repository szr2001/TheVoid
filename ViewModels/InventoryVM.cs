using TheVoid.Enums;

namespace TheVoid.ViewModels
{
    public class InventoryVM
    {
        public List<ItemVM> Items { get; set; }
        public ItemDataVM? SelectedItem { get; set; }

        public InventoryVM(List<ItemVM> items)
        {
            Items = items;
        }
    }
}
