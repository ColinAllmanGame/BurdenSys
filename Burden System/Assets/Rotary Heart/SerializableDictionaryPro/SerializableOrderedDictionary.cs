using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

namespace RotaryHeart.Lib.SerializableDictionaryPro
{
    /// <summary>
    /// Used to create a fully serializable ordered dictionary
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    [System.Serializable]
    public class SerializableOrderedDictionary<TKey, TValue> : DrawableDictionary, ISerializationCallbackReceiver
    {
        public SerializableOrderedDictionary(bool canAdd = true, bool canRemove = true, bool canReorder = true, bool readOnly = false)
            : base(canAdd, canRemove, canReorder, readOnly) { }

        static readonly List<KeyValuePair<TKey, TValue>> M_StaticEmptyList = new List<KeyValuePair<TKey, TValue>>(0);

        Dictionary<TKey, TValue> m_dict;
        List<KeyValuePair<TKey, TValue>> m_list;

        /// <summary>
        /// Copies the data from a dictionary. If an entry with the same key is found it replaces the value
        /// </summary>
        /// <param name="src">Dictionary to copy the data from</param>
        public void CopyFrom(Dictionary<TKey, TValue> src)
        {
            foreach (KeyValuePair<TKey, TValue> data in src)
            {
                if (ContainsKey(data.Key))
                {
                    this[data.Key] = data.Value;
                }
                else
                {
                    Add(data.Key, data.Value);
                }
            }
        }

        /// <summary>
        /// Copies the data from a dictionary. If an entry with the same key is found it replaces the value.
        /// Note that it will only copy the values that are
        /// of the same <typeparamref name="TKey"/> and <typeparamref name="TValue"/> type
        /// </summary>
        /// <param name="src">OrderedDictionary to copy the data from</param>
        public void CopyFrom(OrderedDictionary src)
        {
            IDictionaryEnumerator data = src.GetEnumerator();
            while (data.MoveNext())
            {
                if (data.Key.GetType() != typeof(TKey) || data.Value.GetType() != typeof(TValue))
                    continue;

                if (ContainsKey((TKey)data.Key))
                {
                    this[(TKey)data.Key] = (TValue)data.Value;
                }
                else
                {
                    Add((TKey)data.Key, (TValue)data.Value);
                }
            }
        }

        /// <summary>
        /// Copies the data from a dictionary or ordered dictionary. If an entry with the same key is found it replaces the value.
        /// Note that if <paramref name="src"/> is not a dictionary or ordered dictionary of the same type it will not be copied
        /// </summary>
        /// <param name="src">Dictionary or ordered dictionary to copy the data from</param>
        public void CopyFrom(object src)
        {
            switch (src)
            {
                case Dictionary<TKey, TValue> dictionary:
                    CopyFrom(dictionary);
                    break;
                case OrderedDictionary dict:
                    CopyFrom(dict);
                    break;
            }
        }

        /// <summary>
        /// Copies the data to a dictionary. If an entry with the same key is found it replaces the value
        /// </summary>
        /// <param name="dest">Dictionary to copy the data to</param>
        public void CopyTo(Dictionary<TKey, TValue> dest)
        {
            foreach (KeyValuePair<TKey, TValue> data in this)
            {
                if (dest.ContainsKey(data.Key))
                {
                    dest[data.Key] = data.Value;
                }
                else
                {
                    dest.Add(data.Key, data.Value);
                }
            }
        }

        /// <summary>
        /// Copies the data to a ordered dictionary. If an entry with the same key is found it replaces the value.
        /// </summary>
        /// <param name="dest">OrderedDictionary to copy the data to</param>
        public void CopyTo(OrderedDictionary dest)
        {
            foreach (KeyValuePair<TKey, TValue> data in this)
            {
                if (dest.Contains(data.Key))
                {
                    dest[data.Key] = data.Value;
                }
                else
                {
                    dest.Add(data.Key, data.Value);
                }
            }
        }

        /// <summary>
        /// Copies the data to a dictionary or ordered dictionary. If an entry with the same key is found it replaces the value.
        /// Note that if <paramref name="dest"/> is not a dictionary or ordered dictionary of the same type it will not be copied
        /// </summary>
        /// <param name="dest">Dictionary or ordered dictionary to copy the data to</param>
        public void CopyTo(object dest)
        {
            switch (dest)
            {
                case Dictionary<TKey, TValue> dictionary:
                    CopyTo(dictionary);
                    break;
                case OrderedDictionary dict:
                    CopyTo(dict);
                    break;
            }
        }

        /// <summary>
        /// Returns a copy of the ordered dictionary.
        /// </summary>
        public OrderedDictionary CloneOrdered()
        {
            OrderedDictionary dest = new OrderedDictionary(Count);

            foreach (KeyValuePair<TKey, TValue> data in this)
            {
                dest.Add(data.Key, data.Value);
            }

            return dest;
        }

        /// <summary>
        /// Returns a copy of the dictionary.
        /// </summary>
        public Dictionary<TKey, TValue> CloneDictionary()
        {
            Dictionary<TKey, TValue> dest = new Dictionary<TKey, TValue>(Count);

            foreach (KeyValuePair<TKey, TValue> data in this)
            {
                dest.Add(data.Key, data.Value);
            }

            return dest;
        }

        /// <summary>
        /// Returns true if the value exists; otherwise, false
        /// </summary>
        /// <param name="value">Value to check</param>
        public bool ContainsValue(TValue value)
        {
            if (m_dict == null)
                return false;

            return m_dict.ContainsValue(value);
        }

        #region IDictionary Interface

        public TValue this[int index]
        {
            get
            {
                if (m_dict == null) throw new KeyNotFoundException();
                return m_dict[m_list[index].Key];
            }
            set
            {
                if (m_dict == null) m_dict = new Dictionary<TKey, TValue>();
                m_list[index] = new KeyValuePair<TKey, TValue>(m_list[index].Key, value);
                m_dict[m_list[index].Key] = value;
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                if (m_dict == null) throw new KeyNotFoundException();
                return m_dict[key];
            }
            set
            {
                if (m_dict == null) m_dict = new Dictionary<TKey, TValue>();

                int index = m_list.IndexOf(new KeyValuePair<TKey, TValue>(key, value));

                m_list[index] = new KeyValuePair<TKey, TValue>(key, value);
                m_dict[key] = value;
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                if (m_dict == null)
                    m_dict = new Dictionary<TKey, TValue>();

                return m_dict.Keys;
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                if (m_dict == null)
                    m_dict = new Dictionary<TKey, TValue>();

                return m_dict.Values;
            }
        }

        public int Count
        {
            get
            {
                return (m_dict != null) ? m_dict.Count : 0;
            }
        }

        public bool ContainsKey(TKey key)
        {
            if (m_dict == null)
                return false;

            return m_dict.ContainsKey(key);
        }

#if UNITY_EDITOR
        public void Add(TKey key, TValue value)
        {
            if (m_dict == null)
                m_dict = new Dictionary<TKey, TValue>();

            m_dict.Add(key, value);

            if (m_list == null)
                m_list = new List<KeyValuePair<TKey, TValue>>();

            m_list.Add(new KeyValuePair<TKey, TValue>(key, value));

            if (m_keys == null)
                m_keys = new List<TKey>();
            if (m_values == null)
                m_values = new List<TValue>();

            m_keys.Add(key);
            m_values.Add(value);
        }

        public void Clear()
        {
            if (m_dict != null)
                m_dict.Clear();
            if (m_list != null)
                m_list.Clear();

            if (m_keys != null)
                m_keys.Clear();
            if (m_values != null)
                m_values.Clear();
        }

        public bool Remove(TKey key)
        {
            if (m_dict == null)
                return false;

            TValue value = m_dict[key];

            int index = m_list.IndexOf(new KeyValuePair<TKey, TValue>(key, value));

            if (index != -1)
            {
                if (m_keys != null)
                {
                    m_keys.RemoveAt(index);

                    if (m_values != null)
                        m_values.RemoveAt(index);
                }

                m_list.RemoveAt(index);
            }
            return m_dict.Remove(key);
        }

        public bool Remove(int index)
        {
            if (m_dict == null)
                return false;

            if (index < 0 || index >= Count)
                return false;

            if (m_keys != null)
            {
                m_keys.RemoveAt(index);

                if (m_values != null)
                    m_values.RemoveAt(index);
            }

            TKey key = m_list[index].Key;
            m_list.RemoveAt(index);
            return m_dict.Remove(key);
        }
#else
        public void Add(TKey key, TValue value)
        {
            if (m_dict == null)
                m_dict = new Dictionary<TKey, TValue>();

            m_dict.Add(key, value);

            if (m_list == null)
                m_list = new List<KeyValuePair<TKey, TValue>>();

            m_list.Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        public void Clear()
        {
            if (m_dict != null)
                m_dict.Clear();
            if (m_list != null)
                m_list.Clear();
        }

        public bool Remove(TKey key)
        {
            if (m_dict == null)
                return false;

            TValue value = m_dict[key];

            int index = m_list.IndexOf(new KeyValuePair<TKey, TValue>(key, value));

            if (index != -1)
            {
                m_list.RemoveAt(index);
            }

            return m_dict.Remove(key);
        }

        public bool Remove(int index)
        {
            if (m_dict == null)
                return false;

            if (index < 0 || index >= Count)
                return false;

            TKey key = m_list[index].Key;
            m_list.RemoveAt(index);
            return m_dict.Remove(key);
        }
#endif

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (m_dict == null)
            {
                value = default(TValue);
                return false;
            }

            return m_dict.TryGetValue(key, out value);
        }

        public List<KeyValuePair<TKey, TValue>>.Enumerator GetEnumerator()
        {
            if (m_dict == null) return M_StaticEmptyList.GetEnumerator();
            return m_list.GetEnumerator();
        }

        #endregion IDictionary Interface

        #region ISerializationCallbackReceiver

        [SerializeField]
        List<TKey> m_keys;
        [SerializeField]
        List<TValue> m_values;

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (m_keys != null && m_values != null)
            {
                //Need to clear the dictionary
                if (m_dict == null)
                    m_dict = new Dictionary<TKey, TValue>(m_keys.Count);
                else
                    m_dict.Clear();

                if (m_list == null)
                    m_list = new List<KeyValuePair<TKey, TValue>>(m_keys.Count);
                else
                    m_list.Clear();

                for (int i = 0; i < m_keys.Count; i++)
                {
                    //Key cannot be null, skipping entry
                    if (m_keys[i] == null)
                    {
                        continue;
                    }

                    //Add the data to the dictionary. Value can be null so no special step is required
                    if (i < m_values.Count)
                    {
                        m_dict[m_keys[i]] = m_values[i];
                        m_list.Add(new KeyValuePair<TKey, TValue>(m_keys[i], m_values[i]));
                    }
                    else
                    {
                        m_dict[m_keys[i]] = default(TValue);
                        m_list.Add(new KeyValuePair<TKey, TValue>(m_keys[i], default(TValue)));
                    }
                }
            }
            
            //Outside of editor we clear the arrays to free up memory
#if !UNITY_EDITOR
            m_keys = null;
            m_values = null;
#endif
        }  

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            if (!Application.isPlaying)
                return;

            if (m_list == null || m_list.Count == 0)
            {
                //Dictionary is empty, erase data
                m_keys = null;
                m_values = null;
            }
            else
            {
                //Initialize arrays
                int cnt = m_list.Count;
                m_keys = new List<TKey>(cnt);
                m_values = new List<TValue>(cnt);

                using (List<KeyValuePair<TKey, TValue>>.Enumerator e = m_list.GetEnumerator())
                {
                    while (e.MoveNext())
                    {
                        //Set the respective data from the dictionary
                        m_keys.Add(e.Current.Key);
                        m_values.Add(e.Current.Value);
                    }
                }
            }
        }
        #endregion ISerializationCallbackReceiver
    }
}
