using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NoStudios.Burdens
{

    //we should use burden types to create from templates later.
    //rather than making a new default burden and configuring we should click "New unique regret burden"
    public enum BurdenCategory
    {
        //always add at end, and deprecate, never remove

        //these types are used to split logic for burdens.

        ////Test burden, custom values, used in inspector
        DebugBurden = -7, //does nothing but exist with a name
        CharacterTraumaBurden, //adjusts the character's trauma value. unique to this container.
        CharacterHateBurden, //adjusts the character's hate value. unique to this container.
        CharacterFearBurden, //adjusts the character's fear value. unique to this container.
        CharacterRegretBurden, //adjusts the character's regret value. unique to this container. (adjusting alky's will not affect edgar's)
        DebugSharedPartyBurden, //A burden that can be modified by anyone in the party, to affect all other party members (alky adjusting this will affect edgar)
        DebugStoryBurdenRecentInjury = -1, //Burden placed by a story variable. removed exclusively when conditions are met, and no other way.
        //the above may be removed by a character talking about their injury with another. in which case the scene controller would check the burden inventories by category of involved characters, get the dialogue, and adjust inventories.

    }

    [System.Serializable]
    public struct BurdenInfo
    {
        //contains information unique to this burden in regard to the containing actor.
        [SerializeField]
        public BurdenCategory burdenCategory;
    }

    [CreateAssetMenu(fileName = "DefaultBurden", menuName = "Burdens/MakeDefaultBurden", order = 1)]
    [System.Serializable]
    public class Burden : ScriptableObject
    {
        public BurdenCategory category;
        public virtual int Trauma(BurdenClone clone) { return 0; }
        public virtual int Hate(BurdenClone clone) { return 0; }
        public virtual int Fear(BurdenClone clone) { return 0; }
        public virtual int Regret(BurdenClone clone) { return 0; }


        public virtual void AdjustTrauma(BurdenClone clone, int adjust) { return; }
        public virtual void AdjustHate(BurdenClone clone, int adjust) { return; }
        public virtual void AdjustFear(BurdenClone clone, int adjust) { return; }    
        public virtual void AdjustRegret(BurdenClone clone,int adjust) { return; }

        System.Guid _guid;
        public System.Guid parentBurdenGuid
        {
            get
            {
                if (_guid == System.Guid.Empty)
                {
                    Debug.LogError("FUGGIN MAKE GUID");
                }
                return _guid;
            }
        }
        void MakeGuid()
        {

        }


        //sourcenote is any extra string data that you may want referenceable in the burden later (such as the source's name, or scene, etc)
        //it is a debugging tool that should not be used in logic
        public virtual BurdenClone GenerateClone(string sourceNote = "")
        {
            BurdenClone clone = new BurdenClone();
            clone.parentBurden = this;
            clone.sourceNote = sourceNote;
            clone.uniqueID = System.Guid.NewGuid();
            return clone;
        }

        public int numShared
        {
            get { return currentOwners.Count; }
        }//how many entities are currently holding this burden.
        public bool isShared
        {
            get
            {
                if(numShared>1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        //add in modifiers to ticks. 
        //for example, one burden modifier may cause world ticks to double effectiveness.

        public bool hiddenBurden = false; //a hidden burden does not adjust scores presented to the user. such as total burden count.

        public bool Permanent = false; //this burden cannot be removed, ever (except by specific managers)

        public bool preventDuplicates = false; //can the target accrue multiple instances of this burden?
        //(this should adjust an instancesHeld value in the burden, isntead of actually adding more instances)

        public bool canBeShared = true;
        //if a burden cannot be shared, it can only be owned by a single entity.
        //during a transfer, the burden must arrive at the new owner, or be returned to the sender.
        //the sender MUST remove the burden, or not send it.

            //there should be a check on propagate that verifies this instance of a burden is not shared by any other active container.


        public virtual void BurdenWorldTick(BurdenClone burden) //when the world state advances, the burden receives this call.
        {
            Debug.LogError("section not implemented");
        }

        public virtual void BurdenRedundantAdd(BurdenClone burden)
        {
            Debug.LogError("section not implemented");
        }


        public virtual void BurdenSendAction(BurdenClone burden, BurdenInventory origin,BurdenInventory target)
        { }//burden is dispatched, the sender tints the burden here.
        public virtual void BurdenPostSend(BurdenClone burden, BurdenInventory origin)
        { }//when burden is sent to another entity, after the sender has modified it.
        //the post action also removes the burden from it's origin container

        List<BurdenInfo> currentOwners = new List<BurdenInfo>();
        public virtual void OwnerChangeNotice(BurdenClone burden, BurdenInventory owner,bool acquiring )
        {
            //what happens when a burden changes hands?
        }


        public virtual void BurdenIngestAction(BurdenInventory origin,BurdenInventory target)
        { }//behaviors to apply, adjust, or add during ingest. The receiver has tinted the burden prior to this call.
        public virtual void BurdenPostIngest(BurdenInventory target)
        { }//burden has been ingested, after being tinted by receiver. closing actions.
        //the post action also adds the burden to its' destination container's list.
    }

    

}

