using UnityEngine;

public class CheckHandCardAmount : MatchCondition
{
    public enum Relational { Less, More, LessEq, MoreEq, Eq}
    [SerializeField]
    Relational relational;
    [SerializeField]
    int amount;
    [SerializeField]
    Substance specificSubstance;
    protected override string InitDescription()
    {
        return "Player HandCard has " + (specificSubstance != null ? specificSubstance.chemicalSymbol + " " : "")
            + relational.ToString() + " " + amount;
    }
    public override void StartCheck()
    {
        MatchManager.Player.OnHandCardsChanged.AddListener(Check);
        Check();
    }
    private void Check()
    {
        int foundAmount;
        if(specificSubstance != null)
        {
            foundAmount = 0;
            foreach (var card in MatchManager.Player.FindAllHandCard(specificSubstance))
                foundAmount += card.CardAmount;
        }
        else
        {
            foundAmount = MatchManager.Player.HandCardCount;
        }
        bool cond = false;
        switch(relational)
        {
            case Relational.Eq:
                cond = foundAmount == amount;
                break;
            case Relational.LessEq:
                cond = foundAmount <= amount;
                break;
            case Relational.MoreEq:
                cond = foundAmount >= amount;
                break;
            case Relational.Less:
                cond = foundAmount < amount;
                break;
            case Relational.More:
                cond = foundAmount > amount;
                break;
        }
        if(cond)
        {
            MatchManager.Player.OnHandCardsChanged.RemoveListener(Check);
            onConditionMet.Invoke();
        }
    }
}
