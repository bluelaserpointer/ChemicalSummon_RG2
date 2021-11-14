using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FusionEnhancer : CardAbility
{
    public override void DoAbility(Card card)
    {
        MatchManager.OpenReactionListButton.TrySetCard(card);
    }

    public override bool IsAvaliable(Card card)
    {
        return true;
    }
    public abstract bool Appliable(Fusion fusion);
    public abstract void Apply(Fusion fusion);
    public abstract void OnFusion(Gamer gamer, Reaction reaction);
}
