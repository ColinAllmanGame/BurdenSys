using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static NoStudios.Burdens.BurdenTools;
using System;

namespace NoStudios.Burdens
{
    // [CreateAssetMenu(fileName = "DefaultBurdenContainer", menuName = "ScriptableObjects/MakeBurdenContainer", order = 1)]
    [Serializable]
    public class BurdenInventory/* : ScriptableObject*/
    {
        public string ContainerName = "defaultName";      
        public BurdenReceiverType receiverType;
        public BurdenSenderType senderType;

        [NonSerialized] public UnityEvent OnBurdensChanged = new UnityEvent();

        public int numBurdens = 0; //some burdens should be applied to a non-counted list. for hotfixable adjustments.

        public void CharacterReady()
        {
            InitializeBurdenDictionary();
        }

        public Burden GetTopBurdenByCategory(BurdenCategory cat)
        {
            if(d_heldBurdens[cat].Count==0)
            {
                Debug.LogWarning("requested a random " + cat.ToString() + " from " + ContainerName + " but they had none");
                return null;
            }
            else
            {
                return d_heldBurdens[cat][0];
            }
        }


        List<BurdenCategory> missingCats = new List<BurdenCategory>();
        void InitializeBurdenDictionary()
        {
            List<BurdenCategory> missingCats = new List<BurdenCategory>();
            //add all categories to dictionary
            bool categoriesValid = true;
            foreach (BurdenCategory cat in (BurdenCategory[])Enum.GetValues(typeof(BurdenCategory)))
            {
                if(!d_heldBurdens.ContainsKey(cat))
                {
                    missingCats.Add(cat);
                    categoriesValid = false;
                }
            }
            foreach(BurdenCategory cat in missingCats)
            {
                d_heldBurdens.Add(cat, new List<Burden>());
            }
            if(!categoriesValid)
            {
                Debug.Log( missingCats.Count.ToString() + " Categories added to " + ContainerName + " consider saving to reduce loads");
            }
            BurdensCollectionChanged();
        }


        void BurdensCollectionChanged()
        {
            UpdateContainerValues();
            OnBurdensChanged.Invoke();
        }



        public int totalTraumaVisible = 0;
        int totalTrauma = 0;
        public int Trauma(bool includeInvisible)
        {
            if (includeInvisible)
            {
                return totalTrauma;
            }
            else
            {
                return totalTraumaVisible;
            }
        }


        public int totalFearVisible = 0;
        int totalFear = 0;
        public int Fear(bool includeInvisible)
        {
            if (includeInvisible)
            {
                return totalFear;
            }
            else
            {
                return totalFearVisible;
            }
        }


        public int totalRegretVisible = 0;
        int totalRegret = 0;
        public int Regret(bool includeInvisible)
        {
            if (includeInvisible)
            {
                return totalRegret;
            }
            else
            {
                return totalRegretVisible;
            }
        }


        public int totalHateVisible = 0;
        int totalHate = 0;
        public int Hate(bool includeInvisible)
        {
            if (includeInvisible)
            {
                return totalHate;
            }
            else
            {
                return totalHateVisible;
            }
        }


      
        //public List<Burden> heldBurdens = new List<Burden>();
        //[SerializeField]
        //public HeldBurdenHashtable D_heldBurdens = new HeldBurdenHashtable();
        [UnityEngine.SerializeField]
        public BurdenDictionary d_heldBurdens = new BurdenDictionary();

        //run all burden methods in sequence.
        public void UpdateContainerValues()
        {
            int tempNumBurdens = 0;
            int tempTraumaVisible = 0;
            int tempHateVisible = 0;
            int tempRegretVisible = 0;
            int tempFearVisible = 0;

            Debug.LogWarning("Updated container values on " + ContainerName);
            //for each burden category (holds a collection of burdens of that type)
            foreach(var burdenPair in d_heldBurdens.Pairs)
            {
                //for each burden within that category.
                foreach (var burden in burdenPair.Burdens)
                {
                    if (burden.hiddenBurden)
                    {
                        //YEET
                    }
                    else
                    {
                        tempNumBurdens += 1;
                        //each burden should be parsed for a delegate list to apply to every value prior to this.
                        //these delegates can then be applied to the whole process, or just a portion of it.
                        //much like a status effect.
                        tempTraumaVisible += burden.Trauma;
                        tempRegretVisible += burden.Regret;
                        tempFearVisible += burden.Fear;
                        tempHateVisible += burden.Hate;
                    }
                }
            }

            numBurdens = tempNumBurdens;
            totalFearVisible = tempFearVisible;
            totalRegretVisible = tempRegretVisible;
            totalHateVisible = tempHateVisible;
            totalTraumaVisible = tempTraumaVisible;
            //updates the container values whenever we make a change by parsing over all contained burdens,
            //gets modifiers as well, then applies them in sequence.
            //Ex.


        }



        public bool IngestBurden(Burden burden,CharacterBurdenManager sender, CharacterBurdenManager receiver)
        {

            if (d_heldBurdens.ContainsKey(burden.category))
            {
                //this category is already held
                //an instance of this type is already held, Process it and add it to the list.
                Burden ReceiverTintedBurden = ReceiverTintBurden(burden, sender, receiver);
                Burden IngestTintedBurden = IngestTintBurden(burden, sender, receiver);
                d_heldBurdens[burden.category].Add(IngestTintedBurden);
            }
            else
            {
                d_heldBurdens.Add(burden.category, new List<Burden>()); //make the new category
                Burden ReceiverTintedBurden = ReceiverTintBurden(burden, sender, receiver); //tint by receiver (character ingest action)
                Burden IngestTintedBurden = IngestTintBurden(burden, sender, receiver); //tint by ingest (burden ingest action)
                d_heldBurdens[burden.category].Add(burden); //add to receiver inventory
                //this is a new burden we have not received yet. make a new info, new list, and add to it.
            }

            //a character should only ever have one copy of any type of burden. adding to that type increases the count in it.
            
            //modifiedBurden.OwnerChangeNotice(this, true);

            BurdensCollectionChanged();
            //return true if the addition was compatible with the container, after adding.
            return true;
        }

        Burden ReceiverTintBurden(Burden burden, CharacterBurdenManager sender, CharacterBurdenManager receiver)
        {
            //tint based on receiving behaviors of the receiving inventory
            BurdenProcess receiverOperation = BurdenTools.GetBurdenProcessReceiver(receiverType);
            if (sender != null)
            {
                burden = receiverOperation(burden, sender.burdenInventory, receiver.burdenInventory);
            }
            else
            {
                burden = receiverOperation(burden, null, receiver.burdenInventory);
            }
            return burden;
        }
        Burden IngestTintBurden(Burden burden, CharacterBurdenManager sender, CharacterBurdenManager receiver)
        {
            //Lets the burden's ingest behavior act, with reference to the sender and receiver so the burden can do things to them.
            if (sender != null)
            {
                burden.BurdenIngestAction(sender.burdenInventory, this);
            }
            else
            {
                burden.BurdenIngestAction(null, this);
            }
            return burden;
        }

        //this overload is used when a sender does not exist. it will dispatch null to the various methods instead.
        public bool IngestBurden(Burden burden, CharacterBurdenManager receiver)
        {
            return IngestBurden(burden, null, receiver);
        }

        Burden cachedCloneBackup;
        public bool DispatchBurden(Burden burden,BurdenInventory sender, BurdenInventory receiver)
        {
                //tint by sender
                BurdenProcess senderOperation = BurdenTools.GetBurdenProcessSender(senderType);
                Burden modifiedBurden = senderOperation(burden, this, receiver);

                //tint by parentburden's pre-send action while still in sender inventory
                modifiedBurden.BurdenSendAction(burden,sender, receiver);

                d_heldBurdens[modifiedBurden.category].Remove(modifiedBurden);
            
                modifiedBurden.OwnerChangeNotice(burden, sender, false);


                BurdensCollectionChanged();
                burden.BurdenPostSend(burden, this);
                return true;
        }

        public void DissolveBurden(Burden burden,bool isSilent)
        {
            if (d_heldBurdens.ContainsKey(burden.category))
            {
                if(!isSilent)
                {
                    burden.BurdenPreDissolve(this);
                }
                d_heldBurdens[burden.category].Remove(burden);

                BurdensCollectionChanged();

                if (!isSilent)
                {
                    burden.BurdenPostDissolve(this);
                }
            }
            else
            {
                Debug.LogWarning("no burden in the requested category to dissolve : " + burden.category.ToString());
            }
        }
    }
}

