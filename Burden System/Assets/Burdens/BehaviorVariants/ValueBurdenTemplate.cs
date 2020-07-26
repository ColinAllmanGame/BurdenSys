using UnityEngine;
using NoStudios.Burdens;
using System;

[CreateAssetMenu(fileName = "ValueBurdenTemplate", menuName = "Fault/Burdens/Value Burden", order = 1)]
public sealed class ValueBurdenTemplate : BurdenTemplate<ValueBurden>
{
}

[Serializable]
public class ValueBurden : Burden
{
    //a value burden clone can be created with inherent burden values.
    //these values can be referenced and returned.
    public int traumaValue = 1;
    public int fearValue = 1;
    public int regretValue = 1;
    public int hateValue = 1;


    public override Burden GenerateClone(string sourceNote = "")
    {
        var clone = CloneFrom(this, sourceNote);
        clone.traumaValue = traumaValue;
        clone.regretValue = regretValue;
        clone.hateValue = hateValue;
        clone.fearValue = fearValue;
        return clone;
    }

    public override int Trauma => traumaValue;

    public override int Fear => fearValue;

    public override int Regret => regretValue;

    public override int Hate => hateValue;

    public override void AdjustTrauma(int adjust)
    {
        traumaValue = Mathf.Clamp(traumaValue + adjust, 0, 1000000);
    }
    public override void AdjustFear(int adjust)
    {
        fearValue = Mathf.Clamp(fearValue + adjust, 0, 1000000);
    }
    public override void AdjustRegret(int adjust)
    {
        regretValue = Mathf.Clamp(regretValue + adjust, 0, 1000000);
    }
    public override void AdjustHate(int adjust)
    {
        hateValue = Mathf.Clamp(hateValue + adjust, 0, 1000000);
    }
}

