using UnityEngine;

[DisallowMultipleComponent]
public class CheckTurn_EventNode : EventNode
{
    [SerializeField]
    bool useTurnType = true;
    [SerializeField]
    TurnType turnType;
    [SerializeField]
    int turn;

    public override string PreferredGameObjectName => "Wait for " + (useTurnType ? MatchManager.TurnTypeToString(turnType) : "turn " + turn);
    public override void Reach()
    {
        ConversationWindow.Close();
        MatchManager.OnTurnStart.AddListener(CheckTurnStart);
    }
    private void CheckTurnStart()
    {
        if(useTurnType)
        {
            if (MatchManager.CurrentTurnType.Equals(turnType))
            {
                MatchManager.OnTurnStart.RemoveListener(CheckTurnStart);
                ProgressEvent();
            }
        }
        else if (MatchManager.Turn == turn) {
            MatchManager.OnTurnStart.RemoveListener(CheckTurnStart);
            ProgressEvent();
        }
    }
}
