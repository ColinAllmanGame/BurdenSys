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

    public bool TryAddBurden(BurdenClone burden, CharacterBurdenManager sender, CharacterBurdenManager receiver)
    {
        if (sender == null)
        {
            //this is a scripted send, use a direct route. do not apply sender effects.
            //apply receiver tint, and receive effects.
            if (canReceiveBurdens)
            {
                return receiver.burdenInventory.IngestBurden(burden, receiver);
            }
            else
            {
                BurdenReceiveFail();
                return false;
            }
        }
        else
        {
            if (canReceiveBurdens)
            {
                return receiver.burdenInventory.IngestBurden(burden, sender, receiver);
            }
            else
            {
                BurdenReceiveFail();
                return false;
            }
        }
        //the sender may not be another container.

        //can the character receive this burden?
        //does the burden need to be unique?
        //is the character able to receive ANY burdens?
    }

    void BurdenReceiveFail()
    {
        Debug.LogWarning(burdenInventory.ContainerName + " tried to add a burden but was unable.");
    }

    void BurdenSendFail(BurdenClone burden)
    {
        Debug.LogError(burdenInventory.ContainerName + " was requested to send a " + burden.parentBurden.category.ToString() + ", but is flagged unable to do so.");
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
            BurdenSendFail(burden);
            return false;
        }
        else
        {
            //return true if send was a success.
            return BurdenTools.TransferBurden(burden,this,target);
        }
    }


}
