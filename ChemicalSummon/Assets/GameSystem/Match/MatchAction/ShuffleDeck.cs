using System;

public class ShuffleDeck : MatchAction
{
    public override bool CanAction(Gamer gamer)
    {
        return true;
    }

    public override void DoAction(Gamer gamer, Action afterAction = null, Action cancelAction = null)
    {
        gamer.ShuffleDrawPile();
        afterAction.Invoke();
    }

    protected override string GetDescription()
    {
        return ChemicalSummonManager.LoadSentence("ShuffleDeck");
    }
}
