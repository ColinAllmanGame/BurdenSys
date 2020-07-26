using System;
using UnityEngine;

namespace NoStudios.Burdens
{
    [Serializable]
    public class GameState
    {
        [Serializable]
        internal class InventoryEntry
        {
            public BurdenTools.BurdenSenderType SenderType;
            public BurdenInventory Inventory;

            public InventoryEntry(BurdenTools.BurdenSenderType senderType, BurdenInventory inventory)
            {
                SenderType = senderType;
                Inventory = inventory;
            }
        }
        
        public WorldState WorldData;
        public StoryState StoryData;
        public PlayerState PlayerData;
        
        [SerializeReference]
        internal InventoryEntry[] Inventories;

        public BurdenInventory GetInventory(BurdenTools.BurdenSenderType key)
        {
            for (var i = 0; i < Inventories.Length; i++)
            {
                if (Inventories[i].SenderType == key)
                {
                    return Inventories[i].Inventory;
                }
            }

            return null;
        }
    }
}
