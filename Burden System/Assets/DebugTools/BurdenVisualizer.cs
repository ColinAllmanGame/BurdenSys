using UnityEngine;
using TMPro;
using UnityEngine.Events;

namespace NoStudios.Burdens
{
    public class BurdenVisualizer : MonoBehaviour
    {
        public CharacterBurdenManager subject;

        public TextMeshProUGUI Val1;
        public TextMeshProUGUI Val2;
        public TextMeshProUGUI Val3;
        public TextMeshProUGUI Val4;
        public TextMeshProUGUI Val5;

        UnityAction m_OnInventorySet;
        UnityAction m_SubjectBurdensChanged;

        BurdenVisualizer()
        {
            m_OnInventorySet = OnInventorySet;
            m_SubjectBurdensChanged = SubjectBurdensChanged;
        }
        
        void Awake()
        {
            if(subject == null)
                gameObject.SetActive(false);
            
            subject.OnInventorySet.AddListener(m_OnInventorySet);
        }

        void OnDestroy()
        {
            // ReSharper disable once DelegateSubtraction
            subject.OnInventorySet.RemoveListener(m_OnInventorySet);
        }

        void OnInventorySet()
        {
            //subscribe to target burden inventory events.
            subject.burdenInventory.OnBurdensChanged.RemoveListener(m_SubjectBurdensChanged);
            subject.burdenInventory.OnBurdensChanged.AddListener(m_SubjectBurdensChanged);
            SubjectBurdensChanged();
        }

        void SubjectBurdensChanged()
        {
            if (Val1 != null)
            {
                Val1.text = subject.burdenInventory.numBurdens.ToString() + " total burdens";
            }

            if (Val2 != null)
            {
                Val2.text = subject.burdenInventory.totalFearVisible.ToString() + " fear";
            }

            if (Val3 != null)
            {
                Val3.text = subject.burdenInventory.totalTraumaVisible.ToString() + " trauma";
            }

            if (Val4 != null)
            {
                Val4.text = subject.burdenInventory.totalHateVisible.ToString() + " Hate";
            }

            if (Val5 != null)
            {
                Val5.text = subject.burdenInventory.totalRegretVisible.ToString() + " Regret";
            }
        }
    }
}
