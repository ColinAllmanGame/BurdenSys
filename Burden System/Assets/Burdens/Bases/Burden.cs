using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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

    public abstract class BurdenTemplate : DataTemplate, IBurdenTemplate
    {
        public abstract Burden Clone();

        public abstract Burden Clone(string sourceNote);
    }

    public abstract class BurdenTemplate<T> : BurdenTemplate
        where T : Burden, new()
    {
        [SerializeField] string m_SourceNote;
        [SerializeField] T m_Burden = new T();
        
        public virtual T Create(string sourceNote = null) => Burden.CloneFrom(m_Burden, sourceNote);

        public sealed override Burden Clone() => Create(m_SourceNote);

        public sealed override Burden Clone(string sourceNote) => Create(sourceNote);
    }

    // [CreateAssetMenu(fileName = "DefaultBurden", menuName = "Burdens/MakeDefaultBurden", order = 1)]
    [System.Serializable]
    public abstract class Burden
    {
        public static T CloneFrom<T>(T parent, string sourceNote = null)
            where T : Burden, new()
        {
            Assert.IsNotNull(parent);
            
            var json = JsonUtility.ToJson(parent);
            var clone = JsonUtility.FromJson<T>(json);
            clone.parentBurden = parent;
            clone.SourceNote = sourceNote;

            return clone;
        }

        public BurdenCategory category;

        public Guid TemplateId = Guid.Empty;
        public Burden parentBurden = null;
        public string SourceNote = null;
        public CloneDuplicateRule duplicateRule = CloneDuplicateRule.none;

        public virtual int Trauma { get; }
        public virtual int Hate { get; }
        public virtual int Fear { get; }
        public virtual int Regret { get; }


        public virtual void AdjustTrauma(int adjust) { return; }
        public virtual void AdjustHate(int adjust) { return; }
        public virtual void AdjustFear(int adjust) { return; }    
        public virtual void AdjustRegret(int adjust) { return; }
        //
        // [SerializeField]
        // [HideInInspector]
        // System.Guid _guid;

        //runtime calls that check this should use TRUE for initializeIfEmpty, to prevent runtime errors, at the cost of stability.
        // public System.Guid burdenGuid(bool initializeIfEmpty = false)
        // {
        //         if (_guid == System.Guid.Empty)
        //         {
        //             if (initializeIfEmpty)
        //             {
        //             Debug.LogWarning("An object requested the GUID of a burden. It was not made yet. This has been updated. Burden category : " + category.ToString());
        //             MakeGuid();
        //             }
        //             else
        //             {
        //             Debug.LogWarning("An object requested the GUID of a burden and it was Empty. Call this method with a true arg, or MakeGuid " + category.ToString());
        //         }
        //
        //         }
        //         return _guid;
        // }
        // public void MakeGuid()
        // {
        //     if (_guid == System.Guid.Empty)
        //     {
        //         _guid = System.Guid.NewGuid();
        //     }
        //     else
        //     {
        //         Debug.LogError("bruh you don't need a new GUID, this burden already has one, the category is " + category.ToString()); ;
        //     }
        // }


        //sourcenote is any extra string data that you may want referenceable in the burden later (such as the source's name, or scene, etc)
        //it is a debugging tool that should not be used in logic
        public abstract Burden GenerateClone(string sourceNote = "");

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

        public enum CloneDuplicateRule
        {
            none, //HURT ME PLENTY, ADD ME WEIGHT, TIS NOT THIS 'OL FRAME CANNOT BEAR
            rejectAllDuplicateRequests, //single instance of a category, all duplicates rejected prior to transaction.
            singleInstanceAcceptDuplicates, //can ingest a duplicate, but will only modify the already held clone, instead of adding a new clone to the list.
        }
        public CloneDuplicateRule preventDuplicates = CloneDuplicateRule.none; //can the target accrue multiple instances of this burden?
        //(this should adjust an instancesHeld value in the burden, isntead of actually adding more instances)

        public bool canBeShared = true;
        //if a burden cannot be shared, it can only be owned by a single entity.
        //during a transfer, the burden must arrive at the new owner, or be returned to the sender.
        //the sender MUST remove the burden, or not send it.

            //there should be a check on propagate that verifies this instance of a burden is not shared by any other active container.


        public virtual void BurdenWorldTick() //when the world state advances, the burden receives this call.
        {
            Debug.LogError("section not implemented");
        }

        
        public virtual void BurdenRedundantAdd() //a burden of this category is already here, what happens when another is added?
        {
            Debug.LogError("section not implemented");
        }


        public virtual void BurdenSendAction(Burden burden, BurdenInventory origin,BurdenInventory target)
        { }//burden is dispatched, the sender tints the burden here.
        public virtual void BurdenPostSend(Burden burden, BurdenInventory origin)
        { }//when burden is sent to another entity, after the sender has modified it.
        //the post action also removes the burden from it's origin container

        List<BurdenInfo> currentOwners = new List<BurdenInfo>();
        public virtual void OwnerChangeNotice(Burden burden, BurdenInventory owner,bool acquiring )
        {
            //what happens when a burden changes hands?
        }


        public virtual void BurdenIngestAction(BurdenInventory origin,BurdenInventory target)
        { }//behaviors to apply, adjust, or add during ingest. The receiver has tinted the burden prior to this call.
        public virtual void BurdenPostIngest(BurdenInventory target)
        { }//burden has been ingested, after being tinted by receiver. closing actions.
        //the post action also adds the burden to its' destination container's list.

        public virtual void BurdenPreDissolve(BurdenInventory currentHolder)
        {
            Debug.Log("burden pre dissolve");
            //burden is being removed by something nice.
        }
        public virtual void BurdenPostDissolve(BurdenInventory oldHolder)
        {
            Debug.Log("burden dissolved");
            //burden is being removed by something nice.
        }

    }

    public static class BurdenUtility<T>
        where T : Burden, new()
    {
        
    }
}

