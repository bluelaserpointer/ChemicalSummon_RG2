using UnityEngine;

[DisallowMultipleComponent]
public class DrawCardAnchor : MonoBehaviour
{
    [SerializeField]
    Transform cardCarrier;

    Card card;

    public void SetCard(Card card)
    {
        this.card = card;
        card.transform.SetParent(cardCarrier);
        card.SkipMovingAnimation();
        if(card.location.Equals(CardTransport.Location.MyDeck))
        {
            card.TracePosition(transform);
            card.transform.localRotation = Quaternion.identity;
        }
        else
        {
            card.transform.localPosition = Vector3.zero;
            card.transform.localRotation = Quaternion.identity;
        }
    }
    public void OnAnimationEnd()
    {
        card.transform.SetParent(MatchManager.Instance.transform); //prevent destroy children before animation play
        card.location = CardTransport.Location.MyHandCard;
        MatchManager.Player.AddHandCard(card);
        Destroy(gameObject);
    }
}
