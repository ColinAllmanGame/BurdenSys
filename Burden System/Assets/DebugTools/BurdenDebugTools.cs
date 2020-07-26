using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NoStudios.Burdens;
using UnityEngine.PlayerLoop;

public class BurdenDebugTools : MonoBehaviour
{
    public CharacterBurdenManager ravi;
    public CharacterBurdenManager slaughter;

    public BurdenTemplate burdenTemplate;
    public void SendBurdenToRavi()
    {
        ravi.AddBurdenWorldSource(burdenTemplate.Clone(),null,ravi);
        UpdateRavi();
    }
    public void SendBurdenToSlaughter()
    {
        slaughter.AddBurdenWorldSource(burdenTemplate.Clone(), null, slaughter);
        UpdateSlaughter();
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
