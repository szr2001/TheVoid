namespace TheVoid.ViewModels
{
    public class InventoryVM
    {
        public List<ItemVM> ItemsVM { get; set; }

        public InventoryVM(List<ItemVM> itemsVM)
        {
            ItemsVM = itemsVM;
        }
    }
}
