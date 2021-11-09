using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelectReaction : GamerAction
{
    //[SerializeField]
    //ReactionCondition

    public override bool CanAction(Gamer gamer)
    {
        return false;
    }

    public override void DoAction(Gamer gamer, Action afterAction = null, Action cancelAction = null)
    {
        throw new NotImplementedException();
    }

    protected override string GetDescription()
    {
        return "";
    }
}
