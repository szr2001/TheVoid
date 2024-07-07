using System.ComponentModel.DataAnnotations;

namespace TheVoid.Models
{
    public class StorageItemData : ItemData
    {
        [MaxLength(1000)]
        public byte[]? Data { get; set; }
    }
}
