using System;

public abstract class PassiveMatchAction : MatchAction
{
    public override bool CanAction(Gamer gamer)
    {
        return true;
    }

    public override void DoAction(Gamer gamer, Action afterAction = null, Action cancelAction = null)
    {
    }
}
