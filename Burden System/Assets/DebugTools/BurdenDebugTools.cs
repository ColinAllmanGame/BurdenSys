using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NoStudios.Burdens;

public class BurdenDebugTools : MonoBehaviour
{
    public CharacterBurdenManager ravi;
    public CharacterBurdenManager slaughter;

    public Burden burden;
    public void SendBurdenToRavi()
    {
        ravi.AddBurdenWorldSource(burden.GenerateClone(),null,ravi);
    }
    public void SendBurdenToSlaughter()
    {
        slaughter.AddBurdenWorldSource(burden.GenerateClone(), null, slaughter);
    }

    public void DissolveRaviBurden()
    {
        var targetBurden = ravi.burdenInventory.GetTopBurdenByCategory(categoryToTransfer);
        if (targetBurden != null)
        {
            ravi.burdenInventory.DissolveBurden(targetBurden,false);
        }
    }

    public void UpdateRavi()
    {
        ravi.burdenInventory.UpdateContainerValues();
    }
    public void UpdateSlaughter()
    {
        slaughter.burdenInventory.UpdateContainerValues();
    }

    public BurdenCategory categoryToTransfer;
    public void TransferToSlaughter()
    {
        //burdens will need to be sent via their unique ID, or one from a category at random, or a whole category, etc.
        var targetBurden = ravi.burdenInventory.GetTopBurdenByCategory(categoryToTransfer);
        if (targetBurden != null)
        {
            ravi.TrySendBurden(targetBurden, slaughter);
        }
    }

    public void TransferToRavi()
    {
        //burdens will need to be sent via their unique ID, or one from a category at random, or a whole category, etc.
        var targetBurden = slaughter.burdenInventory.GetTopBurdenByCategory(categoryToTransfer);
        if (targetBurden != null)
        {
            slaughter.TrySendBurden(targetBurden, ravi);
        }
    }

}
