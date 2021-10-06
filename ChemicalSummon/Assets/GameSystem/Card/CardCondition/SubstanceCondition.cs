public abstract class SubstanceCondition : CardCondition
{
    public override bool Accept(SubstanceCard card)
    {
        return Accept(card.Substance);
    }
    public abstract bool Accept(Substance substance);
}
