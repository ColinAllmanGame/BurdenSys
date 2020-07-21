using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace NoStudios.Burdens
{
    public class GameStateTester : MonoBehaviour
    {
        [SerializeField] string m_FilePath = "Saves/TestFile";
        
        [SerializeField] GameStateContext m_Context;
        [SerializeField] GameStateTemplate m_Template;
        [SerializeField] bool m_SaveFile;
        [SerializeField] bool m_DeleteFile;

        Task m_FileOperation;
        Func<string, Task> m_Delete;
        Func<string, Task> m_Save;

        string FullPath => Path.Combine(Application.persistentDataPath, m_FilePath);

        void Awake()
        {
            m_Save = m_Context.Save;
            m_Delete = m_Context.Delete;
            
            var fullPath = FullPath;
            PerformFileOperation(!File.Exists(fullPath) ? m_Context.Create(m_Template, fullPath) : m_Context.Load(fullPath));
        }
        
        void OnDestroy()
        {
            m_FileOperation?.Dispose();
        }

        void Update()
        {
            TryPerformOperation(m_SaveFile, m_Save);
            TryPerformOperation(m_DeleteFile, m_Delete);
        }

        void TryPerformOperation(bool flag, Func<string, Task> op)
        {
            if(ShouldPerformOperation(flag))
                PerformFileOperation(op(FullPath));
        }

        bool ShouldPerformOperation(bool flag) => flag && m_FileOperation == null;

        async Task FileOperationTask(Task task)
        {
            m_FileOperation = task;
            await m_FileOperation;
            m_FileOperation = null;
        }

        async void PerformFileOperation(Task task)
        {
            await FileOperationTask(task);
            m_SaveFile = false;
            m_DeleteFile = false;
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            if(EditorApplication.isPlayingOrWillChangePlaymode)
                return;
            
            m_SaveFile = false;
            m_DeleteFile = false;
        }
#endif
    }
}
