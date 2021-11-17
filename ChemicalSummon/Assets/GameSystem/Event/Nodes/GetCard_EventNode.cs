using UnityEngine;

public class GetCard_EventNode : EventNode
{
    [SerializeField]
    TypeAndCountList<CardHeader> cards;

    public override string PreferredGameObjectName => "get " + cards.TotalCount() + " cards";

    public override void Reach()
    {
        PlayerSave.AddCard(cards);
        ProgressEvent();
    }
}
