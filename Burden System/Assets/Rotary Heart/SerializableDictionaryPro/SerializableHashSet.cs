using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// ReSharper disable RedundantAssignment

namespace RotaryHeart.Lib.SerializableDictionaryPro
{
    /// <summary>
    /// Used to create a fully serializable hash set
    /// </summary>
    /// <typeparam name="TValue">Value type</typeparam>
    [System.Serializable]
    public class SerializableHashSet<TValue> : DrawableDictionary, ISerializationCallbackReceiver
    {
        public SerializableHashSet(bool canAdd = true, bool canRemove = true, bool canReorder = true, bool readOnly = false)
            : base(canAdd, canRemove, canReorder, readOnly) { }

        static readonly Dictionary<TValue, byte> M_StaticEmptyDict = new Dictionary<TValue, byte>(0);
        
        Dictionary<TValue, byte> m_dict;

        /// <summary>
        /// Copies the data from <paramref name="array"/>. If an entry with the same key is found it won't be added
        /// </summary>
        /// <param name="array">Array to copy the data from</param>
        public void CopyFrom(IEnumerable<TValue> array)
        {
            foreach (TValue data in array)
            {
                if (m_dict.ContainsKey(data))
                    continue;
                
                Add(data);
            }
        }

        /// <summary>
        /// Copies the data from <paramref name="array"/>, starting at the specified array index. If an entry with the same key is found it won't be added
        /// </summary>
        /// <param name="array">Array to copy the data from</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyFrom(TValue[] array, int arrayIndex)
        {
            for (int i = arrayIndex; i < array.Length; i++)
            {
                if (m_dict.ContainsKey(array[i]))
                    continue;
                
                Add(array[i]);
            }
        }

        /// <summary>
        /// Copies the specified number of elements from <paramref name="array"/>, starting at the specified array index.  If an entry with the same key is found it won't be added
        /// </summary>
        /// <param name="array">Array to copy the data from</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <param name="count">The number of elements to copy.</param>
        public void CopyFrom(TValue[] array, int arrayIndex, int count)
        {
            int tmpCount = 0;
            
            for (int i = arrayIndex; i < array.Length; i++)
            {
                if (m_dict.ContainsKey(array[i]))
                    continue;

                if (tmpCount > count)
                    break;
                
                tmpCount++;
                Add(array[i]);
            }
        }

        /// <summary>
        /// Copies the data from <paramref name="list"/>, starting at the specified array index.  If an entry with the same key is found it won't be added
        /// </summary>
        /// <param name="list">List to copy the data from</param>
        /// <param name="listIndex">The zero-based index in the list at which copying begins.</param>
        public void CopyFrom(List<TValue> list, int listIndex)
        {
            for (int i = listIndex; i < list.Count; i++)
            {
                if (m_dict.ContainsKey(list[i]))
                    continue;
                
                Add(list[i]);
            }
        }
        
        /// <summary>
        /// Copies the specified number of elements from <paramref name="list"/>, starting at the specified array index.  If an entry with the same key is found it won't be added
        /// </summary>
        /// <param name="list">List to copy the data from</param>
        /// <param name="listIndex">The zero-based index in the list at which copying begins.</param>
        /// <param name="count">The number of elements to copy.</param>
        public void CopyFrom(List<TValue> list, int listIndex, int count)
        {
            int tmpCount = 0;
            
            for (int i = listIndex; i < list.Count; i++)
            {
                if (m_dict.ContainsKey(list[i]))
                    continue;

                if (tmpCount > count)
                    break;
                
                tmpCount++;
                Add(list[i]);
            }
        }
        
        /// <summary>
        /// Copies the data from an array. If an entry with the same key is found it won't be added.
        /// Note that if the <paramref name="src"/> is not an array of the same type it will not be copied
        /// </summary>
        /// <param name="src">Array to copy the data from</param>
        public void CopyFrom(object src)
        {
            switch (src)
            {
                case TValue[] array:
                    CopyFrom(array);
                    break;
                case List<TValue> list:
                    CopyFrom(list);
                    break;
            }
        }
        
        /// <summary>
        /// Copies the elements to an array.
        /// </summary>
        /// <param name="array">The one-dimensional array that is the destination of the elements copied from the HashSet object.
        /// The array must have zero-based indexing.</param>
        public void CopyTo(TValue[] array)
        {
            array = new TValue[Count];

            int index = 0;
            foreach (TValue value in this)
            {
                array[index++] = value;
            }
        }

        /// <summary>
        /// Copies the elements to an array, starting at the specified array index.
        /// </summary>
        /// <param name="array">The one-dimensional array that is the destination of the elements copied from the HashSet object.
        /// The array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        public void CopyTo(TValue[] array, int arrayIndex)
        {
            if (Count < arrayIndex)
                return;

            array = new TValue[Count - arrayIndex];

            int index = 0;
            int correctIndex = 0;
            foreach (TValue value in this)
            {
                if (index < arrayIndex)
                {
                    index++;
                    continue;
                }

                array[correctIndex++] = value;
            }
        }

        /// <summary>
        /// Copies the specified number of elements to an array, starting at the specified array index.
        /// </summary>
        /// <param name="array">The one-dimensional array that is the destination of the elements copied from the HashSet object.
        /// The array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <param name="count">The number of elements to copy to the array.</param>
        public void CopyTo(TValue[] array, int arrayIndex, int count)
        {
            if (Count < arrayIndex)
                return;

            array = new TValue[Count - arrayIndex];

            int tmpCount = 0;
            int index = 0;
            int correctIndex = 0;
            foreach (TValue value in this)
            {
                if (index < arrayIndex)
                {
                    index++;
                    continue;
                }

                if (tmpCount > count)
                    break;

                tmpCount++;
                array[correctIndex++] = value;
            }
        }

        /// <summary>
        /// Copies the elements to a list.
        /// </summary>
        /// <param name="list">The one-dimensional list that is the destination of the elements copied from the HashSet object.
        /// The list must have zero-based indexing.</param>
        public void CopyTo(List<TValue> list)
        {
            list = new List<TValue>(Count);

            foreach (TValue value in this)
            {
                list.Add(value);
            }
        }

        /// <summary>
        /// Copies the elements to a list, starting at the specified list index.
        /// </summary>
        /// <param name="list">The one-dimensional list that is the destination of the elements copied from the HashSet object.
        /// The list must have zero-based indexing.</param>
        /// <param name="listIndex">The zero-based index in the list at which copying begins.</param>
        public void CopyTo(List<TValue> list, int listIndex)
        {
            if (Count < listIndex)
                return;

            list = new List<TValue>(Count - listIndex);

            int index = 0;
            foreach (TValue value in this)
            {
                if (index < listIndex)
                {
                    index++;
                    continue;
                }

                list.Add(value);
            }
        }

        /// <summary>
        /// Copies the specified number of elements to a list, starting at the specified list index.
        /// </summary>
        /// <param name="list">The one-dimensional list that is the destination of the elements copied from the HashSet object.
        /// The list must have zero-based indexing.</param>
        /// <param name="listIndex">The zero-based index in the list at which copying begins.</param>
        /// <param name="count">The number of elements to copy to the list.</param>
        public void CopyTo(List<TValue> list, int listIndex, int count)
        {
            if (Count < listIndex)
                return;

            list = new List<TValue>(Count - listIndex);

            int tmpCount = 0;
            int index = 0;
            foreach (TValue value in this)
            {
                if (index < listIndex)
                {
                    index++;
                    continue;
                }

                if (tmpCount > count)
                    break;

                tmpCount++;
                list.Add(value);
            }
        }
        
        /// <summary>
        /// Copies the data to an array or list. If an entry with the same key is found it replaces the value.
        /// Note that if <paramref name="dest"/> is not an array or list of the same type it will not be copied
        /// </summary>
        /// <param name="dest">Array or list to copy the data to</param>
        public void CopyTo(object dest)
        {
            switch (dest)
            {
                case TValue[] array:
                    CopyTo(array);
                    break;
                case List<TValue> list:
                    CopyTo(list);
                    break;
            }
        }

        /// <summary>
        /// Returns a copy of the hashset
        /// </summary>
        public TValue[] Clone()
        {
            TValue[] dest = new TValue[Count];

            for (int i = 0; i < Count; i++)
            {
                dest[i] = m_keys[i];
            }

            return dest;
        }
        
        #region IDictionary Interface

        public ICollection<TValue> Values
        {
            get
            {
                if (m_dict == null)
                    m_dict = new Dictionary<TValue, byte>();

                return m_dict.Keys;
            }
        }

        public int Count
        {
            get
            {
                return (m_dict != null) ? m_dict.Count : 0;
            }
        }

        public bool Add(TValue value)
        {
            if (m_dict.ContainsKey(value))
                return false;

            m_dict.Add(value, 0);
            return true;
        }

#if UNITY_EDITOR
        public void Clear()
        {
            if (m_dict != null)
                m_dict.Clear();

            if (m_keys != null)
                m_keys.Clear();
        }

        public bool Remove(TValue value)
        {
            if (m_dict == null)
                return false;

            if (m_keys != null)
            {
                m_keys.Remove(value);
            }

            return m_dict.Remove(value);
        }
#else
        public void Clear()
        {
            if (m_dict != null)
                m_dict.Clear();
        }

        public bool Remove(TValue value)
        {
            if (m_dict == null)
                return false;

            return m_dict.Remove(value);
        }
#endif

        public bool Contains(TValue value)
        {
            if (m_dict == null)
                return false;

            return m_dict.ContainsKey(value);
        }

        public List<TValue>.Enumerator GetEnumerator()
        {
            if (m_dict == null) return M_StaticEmptyDict.Keys.ToList().GetEnumerator();
            return m_dict.Keys.ToList().GetEnumerator();
        }

        #endregion IDictionary Interface

        #region ISerializationCallbackReceiver

        [SerializeField]
        List<TValue> m_keys;

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            if (m_keys != null)
            {
                //Need to clear the dictionary
                if (m_dict == null)
                    m_dict = new Dictionary<TValue, byte>(m_keys.Count);
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

                    m_dict[m_keys[i]] = 0;
                }
            }
            
            //Outside of editor we clear the arrays to free up memory
#if !UNITY_EDITOR
            m_keys = null;
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
            }
            else
            {
                //Initialize arrays
                int cnt = m_dict.Count;
                m_keys = new List<TValue>(cnt);

                using (Dictionary<TValue, byte>.Enumerator e = m_dict.GetEnumerator())
                {
                    while (e.MoveNext())
                    {
                        //Set the respective data from the dictionary
                        m_keys.Add(e.Current.Key);
                    }
                }
            }
        }

        #endregion ISerializationCallbackReceiver
    }
}