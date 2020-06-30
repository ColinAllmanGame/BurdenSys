using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace RotaryHeart.Lib.SerializableDictionaryPro
{
    /// <summary>
    /// Base class that most be used for any dictionary that wants to be implemented
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    [System.Serializable]
    public class SerializableDictionary<TKey, TValue> : DrawableDictionary, IDictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        public SerializableDictionary(bool canAdd = true, bool canRemove = true, bool canReorder = true, bool readOnly = false)
            : base(canAdd, canRemove, canReorder, readOnly) { }

        static readonly Dictionary<TKey, TValue> M_StaticEmptyDict = new Dictionary<TKey, TValue>(0);

        Dictionary<TKey, TValue> m_dict;

        /// <summary>
        /// Copies the data from a dictionary. If an entry with the same key is found it replaces the value
        /// </summary>
        /// <param name="src">Dictionary to copy the data from</param>
        public void CopyFrom(IDictionary<TKey, TValue> src)
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
        /// Note that if the <paramref name="src"/> is not a dictionary of the same type it will not be copied
        /// </summary>
        /// <param name="src">Dictionary to copy the data from</param>
        public void CopyFrom(object src)
        {
            if (src is IDictionary<TKey, TValue> dictionary)
            {
                CopyFrom(dictionary);
            }
        }

        /// <summary>
        /// Copies the data to a dictionary. If an entry with the same key is found it replaces the value
        /// </summary>
        /// <param name="dest">Dictionary to copy the data to</param>
        public void CopyTo(IDictionary<TKey, TValue> dest)
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
        /// Copies the data to a dictionary. If an entry with the same key is found it replaces the value.
        /// Note that if <paramref name="dest"/> is not a dictionary of the same type it will not be copied
        /// </summary>
        /// <param name="dest">Dictionary to copy the data to</param>
        public void CopyTo(object dest)
        {
            if (dest is IDictionary<TKey, TValue> dictionary)
            {
                CopyTo(dictionary);
            }
        }

        /// <summary>
        /// Returns a copy of the dictionary.
        /// </summary>
        public Dictionary<TKey, TValue> Clone()
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
            return m_dict != null && m_dict.ContainsValue(value);
        }

        #region IDictionary Interface

        #region Properties

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

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return false; }
        }

        #endregion Properties

#if UNITY_EDITOR
        public void Add(TKey key, TValue value)
        {
            if (m_dict == null)
                m_dict = new Dictionary<TKey, TValue>();

            m_dict.Add(key, value);

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

            if (m_keys != null)
                m_keys.Clear();
            if (m_values != null)
                m_values.Clear();
        }

        public bool Remove(TKey key)
        {
            if (m_dict == null)
                return false;

            if (m_keys != null)
            {
                int index = m_keys.IndexOf(key);

                if (index != -1)
                {
                    m_keys.RemoveAt(index);

                    if (m_values != null)
                        m_values.RemoveAt(index);
                }
            }

            return m_dict.Remove(key);
        }
#else
        public void Add(TKey key, TValue value)
        {
            if (m_dict == null)
                m_dict = new Dictionary<TKey, TValue>();

            m_dict.Add(key, value);
        }

        public void Clear()
        {
            if (m_dict != null)
                m_dict.Clear();
        }

        public bool Remove(TKey key)
        {
            if (m_dict == null)
                return false;

            return m_dict.Remove(key);
        }
#endif

        public bool ContainsKey(TKey key)
        {
            if (m_dict == null)
                return false;

            return m_dict.ContainsKey(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (m_dict == null)
            {
                value = default(TValue);
                return false;
            }

            return m_dict.TryGetValue(key, out value);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            if (m_dict == null) m_dict = new Dictionary<TKey, TValue>();
            (m_dict as ICollection<KeyValuePair<TKey, TValue>>).Add(item);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            if (m_dict == null) return false;
            return (m_dict as ICollection<KeyValuePair<TKey, TValue>>).Contains(item);
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (m_dict == null) return;
            (m_dict as ICollection<KeyValuePair<TKey, TValue>>).CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            if (m_dict == null) return false;
            return (m_dict as ICollection<KeyValuePair<TKey, TValue>>).Remove(item);
        }

        public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
        {
            if (m_dict == null) return M_StaticEmptyDict.GetEnumerator();
            return m_dict.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region ISerializationCallbackReceiver

        [SerializeField, FormerlySerializedAs("_keys")]
        List<TKey> m_keys;
        [SerializeField, FormerlySerializedAs("_values")]
        List<TValue> m_values;

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (m_keys == null || m_values == null)
                return;
            
            //Need to clear the dictionary
            if (m_dict == null)
                m_dict = new Dictionary<TKey, TValue>(m_keys.Count);
            else
                m_dict.Clear();

            for (int i = 0; i < m_keys.Count; i++)
            {
                //Key cannot be null, skipping entry
                if (m_keys[i] == null)
                {
                    continue;
                }

                //Add the data to the dictionary. Value can be null so no special step is required
                if (i < m_values.Count)
                    m_dict[m_keys[i]] = m_values[i];
                else
                    m_dict[m_keys[i]] = default(TValue);
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

            if (m_dict == null || m_dict.Count == 0)
            {
                //Dictionary is empty, erase data
                m_keys = null;
                m_values = null;
            }
            else
            {
                //Initialize arrays
                int cnt = m_dict.Count;
                m_keys = new List<TKey>(cnt);
                m_values = new List<TValue>(cnt);

                using (Dictionary<TKey, TValue>.Enumerator e = m_dict.GetEnumerator())
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

        #endregion

    }
}
