using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NoStudios.Burdens;
using UnityEngine.Events;
using static NoStudios.Burdens.BurdenTools;
using System;
using System.Reflection;

namespace NoStudios.Burdens
{

    [CreateAssetMenu(fileName = "DefaultBurdenContainer", menuName = "ScriptableObjects/MakeBurdenContainer", order = 1)]
    public class BurdenInventory : ScriptableObject
    {
        public string ContainerName = "defaultName";      
        public BurdenReceiverType receiverType;
        public BurdenSenderType senderType;

        public UnityEvent OnBurdensChanged;

        public int numBurdens = 0; //some burdens should be applied to a non-counted list. for hotfixable adjustments.

        public void CharacterReady()
        {
            InitializeBurdenDictionary();
        }

        public BurdenClone GetTopBurdenByCategory(BurdenCategory cat)
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
                d_heldBurdens.Add(cat, new List<BurdenClone>());
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
            foreach(BurdenCategory burdenCat in d_heldBurdens.Keys)
            {
                //for each burden within that category.
                foreach (BurdenClone burden in d_heldBurdens[burdenCat])
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
                        tempTraumaVisible += burden.Trauma();
                        tempRegretVisible += burden.Regret();
                        tempFearVisible += burden.Fear();
                        tempHateVisible += burden.Hate();
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



        public bool IngestBurden(BurdenClone burden,CharacterBurdenManager sender, CharacterBurdenManager receiver)
        {
            //check if able to receive burden
            bool ableToReceive = true;
            if (!ableToReceive)
            {
                Debug.LogWarning("receiver unable to receive burden.");
                return false;
            }

            if (d_heldBurdens.ContainsKey(burden.category))
            {
                //this category is already held
                if(burden.uniquePreventMultiple)
                {
                    Debug.Log("burden duplicate disallowed by configuration of parent burden. (category type was already held)");
                    return false;
                }
                //an instance of this type is already held, Process it and add it to the list.
                BurdenClone ReceiverTintedBurden = ReceiverTintBurden(burden, sender, receiver);
                BurdenClone IngestTintedBurden = IngestTintBurden(burden, sender, receiver);
                d_heldBurdens[burden.category].Add(IngestTintedBurden);
            }
            else
            {
                d_heldBurdens.Add(burden.category, new List<BurdenClone>()); //make the new category
                BurdenClone ReceiverTintedBurden = ReceiverTintBurden(burden, sender, receiver); //tint by receiver (character ingest action)
                BurdenClone IngestTintedBurden = IngestTintBurden(burden, sender, receiver); //tint by ingest (burden ingest action)
                d_heldBurdens[burden.category].Add(burden); //add to receiver inventory
                //this is a new burden we have not received yet. make a new info, new list, and add to it.
            }

            //a character should only ever have one copy of any type of burden. adding to that type increases the count in it.
            
            //modifiedBurden.OwnerChangeNotice(this, true);

            BurdensCollectionChanged();
            //return true if the addition was compatible with the container, after adding.
            return true;
        }

        BurdenClone ReceiverTintBurden(BurdenClone burden, CharacterBurdenManager sender, CharacterBurdenManager receiver)
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
        BurdenClone IngestTintBurden(BurdenClone burden, CharacterBurdenManager sender, CharacterBurdenManager receiver)
        {
            //Lets the burden's ingest behavior act, with reference to the sender and receiver so the burden can do things to them.
            if (sender != null)
            {
                burden.parentBurden.BurdenIngestAction(sender.burdenInventory, this);
            }
            else
            {
                burden.parentBurden.BurdenIngestAction(null, this);
            }
            return burden;
        }

        //this overload is used when a sender does not exist. it will dispatch null to the various methods instead.
        public bool IngestBurden(BurdenClone burden, CharacterBurdenManager receiver)
        {
            return IngestBurden(burden, null, receiver);
        }

        BurdenClone cachedCloneBackup;
        public bool DispatchBurden(BurdenClone burden,BurdenInventory sender,BurdenInventory receiver)
        {
            //this method is for the sender to operate on. the burden goes through several steps
            //pre-remove actions and tints by sender and burden itself. (in sender's inventory)
            //removal
            //post-removal actions and tints, by sender and burden itself. (not in any inventory)


            //burdens will need to have contingincies for failures. such as saving the original state, and reverting it.
            //alternatively, each tint should have an "undo" contingency, and then undo them in reverse order
            //IE, post removal undo, then add back to inventory with no events, then pre-removal undo.

            //as another alternative, Burden dispatches may not be capable of being undone, and no potential failure situation should EVER be attempted.
            //perhaps check against the receiver prior to tinting.

            //consider doing this with a callback to confirm receipt.
            bool ableToDispatch = true;
            if(!ableToDispatch)
            {
                return false;
            }

                //tint by sender
                BurdenProcess senderOperation = BurdenTools.GetBurdenProcessSender(senderType);
                BurdenClone modifiedBurden = senderOperation(burden, this, receiver);

                //tint by parentburden's pre-send action while still in sender inventory
                modifiedBurden.parentBurden.BurdenSendAction(burden,sender, receiver);

            //check remove request on burden. if burden confirms removal, do so here.
                d_heldBurdens[modifiedBurden.category].Remove(modifiedBurden);
            
                modifiedBurden.parentBurden.OwnerChangeNotice(burden, sender, false);


                BurdensCollectionChanged();
                burden.parentBurden.BurdenPostSend(burden, this);

                return true;
        }

        void AddBurdenToInventory(Burden newBurden)
        {
            Debug.LogWarning("method iterated on, please check");
            //heldBurdens.Add(newBurden);
        }

        void RemoveBurdenFromInventory(Burden burden)
        {
            Debug.LogWarning("method iterated on, please check");
            //heldBurdens.Remove(burden);
        }
    }
}

