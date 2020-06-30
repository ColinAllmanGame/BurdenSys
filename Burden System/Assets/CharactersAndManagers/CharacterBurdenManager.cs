using NoStudios.Burdens;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NoStudios.Burdens.BurdenTools;

public class CharacterBurdenManager : MonoBehaviour
{
    public bool canReceiveBurdens = true;
    public bool canSendBurdens = true;

    public BurdenInventory burdenInventory;

    private void Awake()
    {
        burdenInventory.CharacterReady();
        Debug.Log(burdenInventory.ContainerName +" burden inventory active, starting Vals :" + "T"+burdenInventory.Trauma(true).ToString()
            + "F" + burdenInventory.Fear(true).ToString()
            + "R" + burdenInventory.Regret(true).ToString()
            + "H" + burdenInventory.Hate(true).ToString());
    }




    //the character was involved in a burden transaction that was rejected, add reaction flavor, followup actions, etc, here.
    //the clone ref is made available to determine reaction to failure.
    //Perhaps this could generate a new burden, if it was rejected by a character not willing to listen.
    //reactions are configured on a per-burden basis.
    //this may also need to observe some character psychology state, such as Busy, Talking, Queued Actions, etc.
    public void BurdenSendRejected(BurdenTools.TransactionRejectionReasons reason, CharacterBurdenManager sender, CharacterBurdenManager receiver, BurdenClone burden)
    {
        //this character tried to send a burden, but it failed for reason
        Debug.Log("burden transaction rejected with reason : " +reason.ToString());
    }
    public void BurdenReceiveRejected(BurdenTools.TransactionRejectionReasons reason, CharacterBurdenManager sender, CharacterBurdenManager receiver, BurdenClone burden)
    {
        //this character was to be sent a burden, but it failed with reason.
        Debug.Log("burden transaction rejected with reason : " + reason.ToString());
    }



    void BurdenReceiveFail()
    {
        Debug.LogError(burdenInventory.ContainerName + " tried to add a burden but was unable. This is post validate or a non-character sender, something has gone wrong.");
    }

    void BurdenSendFail(BurdenClone burden)
    {
        Debug.LogError(burdenInventory.ContainerName + " was requested to send a " + burden.parentBurden.category.ToString() + ", but is flagged unable to do so. This is post validate, something has gone wrong.");
    }

    public bool TrySendBurden(BurdenClone burden, CharacterBurdenManager target)
    {
        if(burden == null)
        {
            Debug.LogError("Tried to send a null burden. aborting");
            return false;
        }
        //can the target receive burdens? etc.
        if(!canSendBurdens)
        {
            Debug.LogWarning(burdenInventory.ContainerName + " had a send burden request, but is not in a state they can.");
            return false;
        }
        else
        {
            //return true if send was a success.
            return BurdenTools.TransferBurden(burden,this,target);
        }
    }

    public bool AddBurden(BurdenClone burden, CharacterBurdenManager sender, CharacterBurdenManager receiver)
    {
        //pre-validated add burden method, used on dynamic transactions

        if (sender == null)
        {
            //this is a scripted send from a world source, use a more direct override. do not apply sender effects.
            //apply receiver tint, and receive effects.
            receiver.burdenInventory.IngestBurden(burden, receiver);
        }
        else
        {
            //include the sender in the logic to apply various effects.
            receiver.burdenInventory.IngestBurden(burden, sender, receiver);
        }
        //the sender may not be another container.
        return true;
    }

    public bool AddBurdenWorldSource(BurdenClone burden,CharacterBurdenManager sender,CharacterBurdenManager receiver,bool overrideValidation=false)
    {
        //accessed by world sources to add a burden to the character.
        //May or may not validate prior (some systems may necessitate overrides)
        //this will commonly have a null sender, but in some cases, the sender may be an interactable object that can receive burdens and ideas.
        if (!overrideValidation)
        {
            if (BurdenTools.PrevalidateTransaction(burden, sender, receiver))
            {
                receiver.burdenInventory.IngestBurden(burden, receiver);
                return true;
            }
            else
            {
                //prevalidation failed! validation will handle the debug logs.
                return false;
            }
        }
        else
        {
            receiver.burdenInventory.IngestBurden(burden, receiver);
            return true;
        }
    }


}
