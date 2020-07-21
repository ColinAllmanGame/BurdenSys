using System;
using System.Collections.Generic;
using NoStudios.Burdens;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public class BurdenDictionary
{
    static readonly List<BurdenCategory> k_KeysList;
    [SerializeReference] List<KeyValuePair<BurdenCategory, List<Burden>>> m_Data = new List<KeyValuePair<BurdenCategory, List<Burden>>>();

    // public void OnBeforeSerialize()
    // {
    //     m_Data.Clear();
    //     m_Data.AddRange(m_Dictionary);
    // }
    //
    // public void OnAfterDeserialize()
    // {
    //     m_Dictionary.Clear();
    //     for (var i = 0; i < m_Data.Count; i++)
    //         m_Data.Add(m_Data[i]);
    // }

    public List<Burden> this[BurdenCategory key] => Find(key);

    List<Burden> Find(BurdenCategory key)
    {
        for (var i = 0; i < m_Data.Count; i++)
        {
            if (key == m_Data[i].Key)
                return m_Data[i].Value;
        }

        return null;
    }

    public bool ContainsKey(BurdenCategory key) => Find(key) != null;

    public void Add(BurdenCategory key, List<Burden> clones)
    {
        Assert.IsFalse(Find(key) != null);
        
        m_Data.Add(new KeyValuePair<BurdenCategory, List<Burden>>(key, clones));
    }

    public bool TryGetValue(BurdenCategory key, out List<Burden> value)
    {
        value = Find(key);
        return value != null;
    }

    public IReadOnlyCollection<KeyValuePair<BurdenCategory, List<Burden>>> Pairs => m_Data.AsReadOnly();
}




