using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NoStudios.Burdens;
using System;

[CreateAssetMenu(fileName = "ValueBurden", menuName = "Burdens/MakeValueBurden", order = 1)]
    public class ValueBurden : Burden
    {

        //a value burden clone can be created with inherent burden values.
        //these values can be referenced and returned.
        public int traumaValue = 1;
        public int fearValue = 1;
        public int regretValue = 1;
        public int hateValue = 1;


    public override BurdenClone GenerateClone(string sourceNote = "")
    {
        BurdenClone clone = new BurdenClone();
        clone.parentBurden = this;
        clone.sourceNote = sourceNote;
        clone.traumaValue = traumaValue;
        clone.regretValue = regretValue;
        clone.hateValue = hateValue;
        clone.fearValue = fearValue;
        clone.uniqueID = System.Guid.NewGuid();
        return clone;
    }

    public override int Trauma(BurdenClone clone)
        {
            return clone.traumaValue;
        }
        public override int Fear(BurdenClone clone)
        {
            return clone.fearValue;
        }
        public override int Regret(BurdenClone clone)
        {
            return clone.regretValue;
        }
        public override int Hate(BurdenClone clone)
        {
            return clone.hateValue;
        }

    public override void AdjustTrauma(BurdenClone clone, int adjust)
    {
        clone.traumaValue = Mathf.Clamp(clone.traumaValue + adjust, 0, 1000000);
    }
    public override void AdjustFear(BurdenClone clone, int adjust)
    {
        clone.fearValue = Mathf.Clamp(clone.fearValue + adjust, 0, 1000000);
    }
    public override void AdjustRegret(BurdenClone clone, int adjust)
    {
        clone.regretValue = Mathf.Clamp(clone.regretValue + adjust, 0, 1000000);
    }
    public override void AdjustHate(BurdenClone clone,int adjust)
    {
        clone.hateValue = Mathf.Clamp(clone.hateValue + adjust, 0, 1000000);
    }
}

