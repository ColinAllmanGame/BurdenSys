using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace NoStudios.Burdens
{

    public class BurdenVisualizer : MonoBehaviour
    {

        public BurdenInventory subject;

        public TextMeshProUGUI Val1;
        public TextMeshProUGUI Val2;
        public TextMeshProUGUI Val3;
        public TextMeshProUGUI Val4;
        public TextMeshProUGUI Val5;

        private void Awake()
        {
            //subscribe to target burden inventory events.
            subject.OnBurdensChanged.AddListener(SubjectBurdensChanged);
            SubjectBurdensChanged();
        }

        void SubjectBurdensChanged()
        {
            if (Val1 != null)
            {
                Val1.text = subject.numBurdens.ToString() + " total burdens";
            }

            if (Val2 != null)
            {
                Val2.text = subject.totalFearVisible.ToString() + " fear";
            }

            if (Val3 != null)
            {
                Val3.text = subject.totalTraumaVisible.ToString() + " trauma";
            }

            if (Val4 != null)
            {
                Val4.text = subject.totalHateVisible.ToString() + " Hate";
            }

            if (Val5 != null)
            {
                Val5.text = subject.totalRegretVisible.ToString() + " Regret";
            }
        }
    }
}
