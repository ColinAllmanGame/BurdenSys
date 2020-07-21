using System;
using System.Collections.Generic;
using NoStudios.Burdens;
using UnityEngine;
using UnityEngine.Assertions;

[Serializable]
public class BurdenDictionary
{
    [Serializable]
    public struct Pair
    {
        public BurdenCategory Category;
        public List<Burden> Burdens;

        public Pair(BurdenCategory category, List<Burden> burdens)
        {
            Category = category;
            Burdens = burdens;
        }
    }
    
    [SerializeReference] List<Pair> m_Data = new List<Pair>();

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
            var pair = m_Data[i];
            if (key == pair.Category)
                return pair.Burdens;
        }

        return null;
    }

    public bool ContainsKey(BurdenCategory key) => Find(key) != null;

    public void Add(BurdenCategory key, List<Burden> clones)
    {
        Assert.IsFalse(Find(key) != null);
        
        m_Data.Add(new Pair(key, clones));
    }

    public bool TryGetValue(BurdenCategory key, out List<Burden> value)
    {
        value = Find(key);
        return value != null;
    }

    public IReadOnlyCollection<Pair> Pairs => m_Data.AsReadOnly();
}




