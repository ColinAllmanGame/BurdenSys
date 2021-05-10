using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Junkboi : SavableClass
{
    public override void SaveClass()
    {
        if(shouldSave)
        {
            //call save thing?
        }
        return;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

public class SavableClass : MonoBehaviour
{
    public bool shouldSave;
    public bool shouldSaveHealth;

    public virtual void SaveClass()
    {

    }
}
