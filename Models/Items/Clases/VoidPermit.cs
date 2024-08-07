﻿using TheVoid.Enums;
using TheVoid.Models.Items.Functionalities;

namespace TheVoid.Models.Items.Clases
{
    public class VoidPermit : ItemBase
    {
        public VoidPermit(string name, string description, string? iconPath, ItemType type, ItemRarity rarity, Dictionary<ItemOptionType, ItemOptionBase> options) : base(name, description, iconPath, type, options, rarity)
        {
        }
    }
}
