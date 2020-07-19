using System.Collections.Generic;
using UnityEngine;

namespace NoStudios.Burdens
{
    [CreateAssetMenu(menuName = "Fault/Burden Inventory Template", fileName = "BurdenInventoryTemplate", order = 0)]
    public class BurdenInventoryTemplate : DataTemplate, IDataTemplate<BurdenInventory>
    {
        [SerializeField] List<BurdenTemplate> m_BurdensTemplate;

        [SerializeField] string m_ContainerName;
        [SerializeField] BurdenTools.BurdenReceiverType m_ReceiverType;
        [SerializeField] BurdenTools.BurdenSenderType m_SenderType;
        [SerializeField] int m_NumBurdens;
        
        public BurdenInventory Clone()
        {
            var inventory = new BurdenInventory();
            var heldBurdens = inventory.d_heldBurdens;
            for (var i = 0; i < m_BurdensTemplate.Count; i++)
            {
                var burdenTemplate = m_BurdensTemplate[i];
                var clone = burdenTemplate.Clone();
                var category = clone.category;

                if (!heldBurdens.TryGetValue(category, out var burdens))
                {
                    burdens = new List<Burden>(4);
                    heldBurdens.Add(category, burdens);
                }
                
                burdens.Add(clone);
            }

            inventory.ContainerName = m_ContainerName;
            inventory.receiverType = m_ReceiverType;
            inventory.senderType = m_SenderType;
            inventory.numBurdens = m_NumBurdens;
            
            inventory.CharacterReady();

            return inventory;
        }
    }
}
