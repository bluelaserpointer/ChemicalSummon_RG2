using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FusionEnhancer : CardAbility
{
    public override void DoAbility(Card card)
    {
        //TODO: call reaction window and take this card for enhancement
        throw new System.NotImplementedException();
    }

    public override bool IsAvaliable(Card card)
    {
        return true;
    }
    public abstract Reaction.ReactionMethod Apply(Reaction.ReactionMethod method);
}
