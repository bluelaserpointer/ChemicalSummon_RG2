using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTurn : MatchCondition
{
    [SerializeField]
    bool useTurnType = true;
    [SerializeField]
    TurnType turnType;
    [SerializeField]
    int turn;

    protected override string InitDescription()
    {
        return "Wait for " + (useTurnType ? MatchManager.TurnTypeToString(turnType) : "turn " + turn);
    }
    public override void StartCheck()
    {
        MatchManager.OnTurnStart.AddListener(Check);
        Check();
    }
    private void Check()
    {
        if (useTurnType)
        {
            if (MatchManager.CurrentTurnType.Equals(turnType))
            {
                MatchManager.OnTurnStart.RemoveListener(Check);
                onConditionMet.Invoke();
            }
        }
        else if (MatchManager.Turn == turn)
        {
            MatchManager.OnTurnStart.RemoveListener(Check);
            onConditionMet.Invoke();
        }
    }
}
