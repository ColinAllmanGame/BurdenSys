using UnityEngine;

namespace RotaryHeart.Lib.SerializableDictionaryPro
{
    [CreateAssetMenu(fileName = "DataBaseSorted.asset", menuName = "Data Base (Sorted)")]
    public class SortedExample : ScriptableObject
    {
        [SerializeField]
        private Int_String _intString;

        [System.Serializable]
        public class Int_String : SerializableSortedDictionary<int, string> { };

        [ContextMenu("Print")]
        public void Print()
        {
            Debug.Log(JsonUtility.ToJson(_intString));
        }
    }
}