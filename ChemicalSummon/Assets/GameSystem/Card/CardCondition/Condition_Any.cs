using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition_Any : SubstanceCondition
{
    protected override string InitDescription()
    {
        return "";
    }
    public override bool Accept(Substance substance)
    {
        return true;
    }
}