using System;
using System.Collections.Generic;
using NoStudios.Burdens;
using UnityEngine;

[Serializable]
public class BurdenDictionary
{
    [SerializeReference] List<KeyValuePair<BurdenCategory, List<BurdenClone>>> m_Data = new List<KeyValuePair<BurdenCategory, List<BurdenClone>>>();
    [NonSerialized] Dictionary<BurdenCategory, List<BurdenClone>> m_Dictionary = new Dictionary<BurdenCategory, List<BurdenClone>>();
    
    public void OnBeforeSerialize()
    {
        m_Data.Clear();
        m_Data.AddRange(m_Dictionary);
    }

    public void OnAfterDeserialize()
    {
        m_Dictionary.Clear();
        for (var i = 0; i < m_Data.Count; i++)
            m_Data.Add(m_Data[i]);
    }

    public List<BurdenClone> this[BurdenCategory key]
    {
        get => m_Dictionary[key];
        set => m_Dictionary[key] = value;
    }

    public bool ContainsKey(BurdenCategory key) => m_Dictionary.ContainsKey(key);

    public void Add(BurdenCategory key, List<BurdenClone> clones) => m_Dictionary.Add(key, clones);

    public Dictionary<BurdenCategory, List<BurdenClone>>.KeyCollection Keys => m_Dictionary.Keys;
}




