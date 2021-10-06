using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Condition_Not : SubstanceCondition
{
    public override bool Accept(Substance substance)
    {
        return false;
    }

    protected override string InitDescription()
    {
        return "";
    }
}
