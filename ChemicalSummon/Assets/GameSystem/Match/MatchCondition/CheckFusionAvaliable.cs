using UnityEngine;

public class CheckFusionAvaliable : MatchCondition
{
    [SerializeField]
    Reaction reaction;
    protected override string InitDescription()
    {
        return "Player can fusion " + reaction.description;
    }
    public override void StartCheck()
    {
        MatchManager.FusionPanel.onFusionListUpdate.AddListener(Check);
        Check();
    }
    private void Check()
    {
        foreach(var method in MatchManager.FusionPanel.lastAvaliableReactionMethods)
        {
            if (method.reaction.Equals(reaction))
            {
                MatchManager.FusionPanel.onFusionListUpdate.RemoveListener(Check);
                onConditionMet.Invoke();
                break;
            }
        }
    }
}

