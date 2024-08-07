﻿using TheVoid.Enums;

namespace TheVoid.ViewModels.Items
{
    public class ItemVM
    {
        public ItemType Type { get; set; }
        public string? ThumbPath { get; set; }

        public ItemVM(ItemType type, string? thumbPath)
        {
            Type = type;
            ThumbPath = thumbPath;
        }
    }
}
