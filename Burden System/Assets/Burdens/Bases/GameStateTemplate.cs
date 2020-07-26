using System;
using System.Collections.Generic;
using UnityEngine;

namespace NoStudios.Burdens
{
    [CreateAssetMenu(menuName = "Fault/Game State Template", fileName = "GameStateTemplate", order = 0)]
    public class GameStateTemplate : DataTemplate, IDataTemplate<GameState>
    {
        [Serializable]
        struct CharacterInventoryEntry
        {
            public string Name;
            public BurdenTools.BurdenSenderType CharacterSenderType;
            public BurdenInventoryTemplate InventoryTemplate;
        }
        
        [SerializeField] WorldStateTemplate m_WorldStateTemplate;
        [SerializeField] StoryStateTemplate m_StoryStateTemplate;
        [SerializeField] PlayerStateTemplate m_PlayerStateTemplate;
        [SerializeField] List<CharacterInventoryEntry> m_Inventories;

        public GameState Clone()
        {
            var world = m_WorldStateTemplate.Clone();
            var story = m_StoryStateTemplate.Clone();
            var player = m_PlayerStateTemplate.Clone();

            var inventories = new GameState.InventoryEntry[m_Inventories.Count];
            for (var i = 0; i < m_Inventories.Count; i++)
            {
                var kvp = m_Inventories[i];
                var sender = kvp.CharacterSenderType;
                var inventoryTemplate = kvp.InventoryTemplate;
                
                inventories[i] = new GameState.InventoryEntry(sender, inventoryTemplate.Clone());
            }

            var gameState = new GameState
            {
                WorldData = world,
                StoryData = story,
                PlayerData = player,
                Inventories = inventories
            };

            return gameState;
        }
    }
}
