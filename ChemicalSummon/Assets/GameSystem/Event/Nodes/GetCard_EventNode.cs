using UnityEngine;

public class GetCard_EventNode : EventNode
{
    [SerializeField]
    TypeAndCountList<Substance> cards;

    public override string PreferredGameObjectName => "get " + cards.TotalCount() + " cards";

    public override void Reach()
    {
        PlayerSave.SubstanceStorage.AddAll(cards);
        ProgressEvent();
    }
}
