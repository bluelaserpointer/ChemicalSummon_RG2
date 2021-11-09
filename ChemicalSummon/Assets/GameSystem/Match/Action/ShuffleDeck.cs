using System;

public class ShuffleDeck : GamerAction
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
        return General.LoadSentence("ShuffleDeck");
    }
}
