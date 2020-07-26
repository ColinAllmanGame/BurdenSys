using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace NoStudios.Burdens
{
    [CreateAssetMenu(fileName = "GameStateContext", menuName = "Fault/Game State Context", order = 0)]
    public class GameStateContext : ScriptableObject
    {
        GameState m_GameState;
        Task m_IsReady;
        bool m_Loaded;

        public Task IsReady
        {
            get
            {
                if (!Application.isPlaying)
                    return null;
                
                InitIsReady();

                return m_IsReady;
            }
        }

        void InitIsReady()
        {
            if (m_IsReady != null)
                return;

            m_IsReady = Task.Run(WaitOnLoaded);
        }

        async Task WaitOnLoaded()
        {
            while (!m_Loaded)
            {
                await Task.Delay(100);
            }
        }

        void OnEnable()
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
                return;
#endif
            if(m_IsReady == null)
                InitIsReady();
        }

        void OnDisable()
        {
            m_IsReady?.Dispose();
            m_GameState = null;
            m_Loaded = false;
            m_IsReady = null;
        }

        public async Task Load(string filePath)
        {
            Assert.IsFalse(string.IsNullOrEmpty(filePath));
            Assert.IsTrue(File.Exists(filePath));
            
            await Task.Run(
                () =>
                {
                    var json = File.ReadAllText(filePath, Encoding.UTF8);
                    m_GameState = JsonUtility.FromJson<GameState>(json);
                });

            m_Loaded = true;
        }

        async Task Save(string filePath, GameState gameState)
        {
            Assert.IsFalse(string.IsNullOrEmpty(filePath));
            Assert.IsNotNull(m_GameState);
            
            var directory = Path.GetDirectoryName(filePath);
            Assert.IsFalse(string.IsNullOrEmpty(directory));
            Directory.CreateDirectory(directory);

            await Task.Run(
                () =>
                {
                    var json = JsonUtility.ToJson(gameState);
                    File.WriteAllText(filePath, json, Encoding.UTF8);
                });
        }
        
        public async Task Save(string filePath)
        {
            Assert.IsNotNull(m_GameState);
            await Save(filePath, m_GameState);
        }

        public async Task Create(GameStateTemplate template, string filePath)
        {
            Assert.IsNotNull(template);
            Assert.IsFalse(string.IsNullOrEmpty(filePath));
            await Task.Run(
                () =>
                {
                    m_GameState = template.Clone();
                });
            
            await Save(filePath, m_GameState);

            m_Loaded = true;
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
