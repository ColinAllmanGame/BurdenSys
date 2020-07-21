using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace NoStudios.Burdens
{
    [CreateAssetMenu(fileName = "GameStateContext", menuName = "Fault/Game State Context", order = 0)]
    public class GameStateContext : ScriptableObject
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
            Assert.IsTrue(File.Exists(filePath));
            
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
            
            var directory = Path.GetDirectoryName(filePath);
            Assert.IsFalse(string.IsNullOrEmpty(directory));
            Directory.CreateDirectory(directory);

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

        public async Task Delete(string filePath)
        {
            Assert.IsFalse(string.IsNullOrEmpty(filePath));

            await Task.Run(() => File.Delete(filePath));
        }

        public GameState GameState => m_GameState;

        public BurdenInventory GetInventory(BurdenTools.BurdenSenderType senderType) => m_GameState.GetInventory(senderType);
    }
}
