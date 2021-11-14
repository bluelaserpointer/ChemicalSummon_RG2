using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFusionDone : MatchCondition
{
    [SerializeField]
    Reaction reaction;
    protected override string InitDescription()
    {
        return "Player did fusion " + (reaction != null ? reaction.formula : "...");
    }
    public override void StartCheck()
    {
        MatchManager.Player.onFusionExecute.AddListener(Check);
    }
    private void Check(Reaction.ReactionMethod method)
    {
        if(method.fusion.Reaction.Equals(reaction))
        {
            MatchManager.Player.onFusionExecute.RemoveListener(Check);
            onConditionMet.Invoke();
        }
    }
}
