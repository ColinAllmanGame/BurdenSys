using System;
using System.Collections.Generic;
using UnityEngine;

namespace NoStudios.Burdens
{

    public interface IDataTemplate
    {
        Guid TemplateId { get; }
    }
    
    public interface IDataTemplate<out T> : IDataTemplate
    {
        T Clone();
    }

    public interface IBurdenTemplate : IDataTemplate<Burden>
    {
        Burden Clone(string sourceNote);
    }

    public abstract class DataTemplate : ScriptableObject
    {
        [SerializeField, HideInInspector] Guid m_TemplateId;
        
        public Guid TemplateId => m_TemplateId;
        
#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (m_TemplateId == Guid.Empty)
                m_TemplateId = Guid.NewGuid();
        }
#endif
    }
    
    public abstract class DataTemplate<T> : DataTemplate, IDataTemplate<T>
        where T : class, new()
    {
        [SerializeField] T m_Value = new T();

        public T Clone() => JsonUtility.FromJson<T>(JsonUtility.ToJson(m_Value));
    }
}
