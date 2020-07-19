using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;
using CharacterInventoryTemplate = System.Collections.Generic.KeyValuePair<NoStudios.Burdens.BurdenTools.BurdenSenderType, NoStudios.Burdens.BurdenInventoryTemplate>;
using CharacterInventory = System.Collections.Generic.KeyValuePair<NoStudios.Burdens.BurdenTools.BurdenSenderType, NoStudios.Burdens.BurdenInventory>;

namespace NoStudios.Burdens
{
    [CreateAssetMenu(fileName = "GameStateContainer", menuName = "Fault/Game State Container", order = 0)]
    public class GameStateContainer : ScriptableObject
    {
        GameState m_GameState = null;

        bool m_HasLoadedSuccessfully;

        Task m_IsReady = null;

        public Task IsReady => m_IsReady;

        void OnEnable()
        {
            m_IsReady = Task.Run(
                async () =>
                {
                    while (!m_HasLoadedSuccessfully)
                    {
                        await Task.Delay(100);
                    }
                });
        }

        void OnDisable()
        {
            m_IsReady.Dispose();
            m_IsReady = null;
        }

        public async Task Load(string filePath)
        {
            Assert.IsFalse(string.IsNullOrEmpty(filePath));
            
            GameState gameState = null;
            await Task.Run(
                () =>
                {
                    var json = File.ReadAllText(filePath, Encoding.UTF8);
                    gameState = JsonUtility.FromJson<GameState>(json);
                });

            m_GameState = gameState;
            m_HasLoadedSuccessfully = true;
        }
        public async Task Save(string filePath)
        {
            Assert.IsFalse(string.IsNullOrEmpty(filePath));
            Assert.IsNotNull(m_GameState);

            var gameState = m_GameState;

            await Task.Run(
                () =>
                {
                    var json = JsonUtility.ToJson(gameState);
                    File.WriteAllText(filePath, json, Encoding.UTF8);
                });
        }

        public async Task Create(GameStateTemplate template, string filePath)
        {
            Assert.IsNotNull(template);
            Assert.IsFalse(string.IsNullOrEmpty(filePath));

            GameState gameState = null;

            await Task.Run(
                () =>
                {
                    gameState = template.Clone();
                });

            m_GameState = gameState;
            
            await Save(filePath);
            m_HasLoadedSuccessfully = true;
        }

        public GameState GameState => m_GameState;

        public BurdenInventory GetInventory(BurdenTools.BurdenSenderType senderType) => m_GameState.GetInventory(senderType);
    }

    public class GameStateTemplate : DataTemplate, IDataTemplate<GameState>
    {
        [SerializeField] WorldStateTemplate m_WorldStateTemplate;
        [SerializeField] StoryStateTemplate m_StoryStateTemplate;
        [SerializeField] PlayerStateTemplate m_PlayerStateTemplate;
        [SerializeField] List<CharacterInventoryTemplate> m_Inventories;

        public GameState Clone()
        {
            var world = m_WorldStateTemplate.Clone();
            var story = m_StoryStateTemplate.Clone();
            var player = m_PlayerStateTemplate.Clone();

            var inventories = new CharacterInventory[m_Inventories.Count];
            for (var i = 0; i < m_Inventories.Count; i++)
            {
                var kvp = m_Inventories[i];
                var sender = kvp.Key;
                var inventory = kvp.Value;
                
                inventories[i] = new CharacterInventory(sender, inventory.Clone());
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

    [System.Serializable]
    public class GameState
    {
        public WorldState WorldData;
        public StoryState StoryData;
        public PlayerState PlayerData;
        
        [SerializeReference]
        public CharacterInventory[] Inventories;

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
