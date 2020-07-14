using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NoStudios.Burdens;

namespace NoStudios.Burdens
{

    // [System.Serializable]
    // public class BurdenClone
    // {
    //     //a burden clone is an object that is created which references an existing burden scriptable object.
    //     //this clone uses the methods on that object as tools to author it's behaviors.
    //     //to make a clone, call the parent SO, it will provide delegates and return the constructed instance.
    //
    //     public Burden parentBurden; /// <summary>
    //
    //     public System.Guid parentGuid
    //     {
    //         get { return parentBurden.burdenGuid(); }
    //     }
    //     /// add getter setter, if this is null, get from guid. Or at deserialize
    //     /// </summary>
    //
    //     public string sourceNote = "";
    //
    //     //it looks atypical
    //     //however, each burdenclone points to the parent scriptable that it was made from to determine behaviors.
    //     //this is peculiar, because the burdenclone itself contains the values being adjusted.
    //     //in the case of valueburdens, this simply returns the value of the clone. In other behaviors, this may return more esoteric values (like the parentburdens, for faction-wide values)
    //     public int Trauma(){ return parentBurden.Trauma(this); }
    //     public int traumaValue =0;
    //     public int Hate() { return parentBurden.Hate(this); }
    //     public int hateValue = 0;
    //     public int Fear() { return parentBurden.Fear(this); }
    //     public int fearValue = 0;
    //     public int Regret() { return parentBurden.Regret(this); }
    //     public int regretValue = 0;
    //
    //     public System.Guid uniqueID;
    //
    //     public BurdenCategory category
    //     {
    //         get { return parentBurden.category; }
    //     }
    //
    //     public int numShared
    //     {
    //         get { return parentBurden.numShared; }
    //     }//how many entities are currently holding this burden. NOT how many individual instances are held.
    //
    //     public bool isShared
    //     {
    //         get { return parentBurden.isShared; }
    //     }
    //
    //     public bool hiddenBurden
    //     {
    //         get { return parentBurden.hiddenBurden; }
    //     }
    //     //a hidden burden does not adjust scores presented to the user. such as total burden count.
    //
    //     public bool Permanent
    //     {
    //         get { return parentBurden.Permanent; }
    //     }//this burden cannot be removed, ever (except by specific managers)
    //
    //     public Burden.CloneDuplicateRule duplicateRule
    //     {
    //         get { return parentBurden.preventDuplicates; }
    //     }//can the target accrue multiple instances of this burden?
    //
    //     public bool canBeShared
    //     {
    //         get { return parentBurden.canBeShared; }
    //     }
    //
    // }
}
