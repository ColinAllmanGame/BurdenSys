using System.Collections.Generic;
using UnityEngine;

namespace NoStudios.Burdens
{
    [System.Serializable]
    public class GameState
    {
        public WorldState WorldData;
        public StoryState StoryData;
        public PlayerState PlayerData;
        
        [SerializeReference]
        public KeyValuePair<BurdenTools.BurdenSenderType, BurdenInventory>[] Inventories;

        public BurdenInventory GetInventory(BurdenTools.BurdenSenderType key)
        {
            for (var i = 0; i < Inventories.Length; i++)
            {
                if (Inventories[i].Key == key)
                {
                    return Inventories[i].Value;
                }
            }

            return null;
        }
    }
}
