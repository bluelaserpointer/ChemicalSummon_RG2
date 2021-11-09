public abstract class SubstanceCondition : CardCondition
{
    public override bool Accept(Card card)
    {
        SubstanceCard substanceCard = card as SubstanceCard;
        if (substanceCard == null)
            return false;
        return Accept(substanceCard.Substance);
    }
    public abstract bool Accept(Substance substance);
}
