using UnityEngine;

public abstract class ConvertedCardAbility<TCard> : CardAbility where TCard : Card
{
    public override sealed bool IsAvaliable(Card card)
    {
        TCard tcard = card as TCard;
        return tcard == null ? false : IsAvaliable(tcard);
    }
    public override sealed void DoAbility(Card card)
    {
        TCard tcard = card as TCard;
        if (tcard != null)
            DoAbility(tcard);
        else
        {
            Debug.LogError(GetType().Name + " does not work on card " + card.CurrentLanguageName + ". Please check the ability prefab of the same name.");
        }
    }
    protected abstract bool IsAvaliable(TCard card);
    protected abstract void DoAbility(TCard card);
}
