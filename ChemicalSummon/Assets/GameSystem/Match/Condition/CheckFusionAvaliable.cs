using UnityEngine;

public class CheckFusionAvaliable : MatchCondition
{
    [SerializeField]
    Reaction reaction;
    protected override string InitDescription()
    {
        return "Player can fusion " + reaction.formula;
    }
    public override void StartCheck()
    {
        MatchManager.OpenReactionListButton.onFusionListUpdate.AddListener(Check);
        Check();
    }
    private void Check()
    {
        foreach(var method in MatchManager.OpenReactionListButton.lastAvaliableReactionMethods)
        {
            if (method.reaction.Equals(reaction))
            {
                MatchManager.OpenReactionListButton.onFusionListUpdate.RemoveListener(Check);
                onConditionMet.Invoke();
                break;
            }
        }
    }
}

