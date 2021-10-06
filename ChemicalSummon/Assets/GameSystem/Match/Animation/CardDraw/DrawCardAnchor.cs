using UnityEngine;

[DisallowMultipleComponent]
public class DrawCardAnchor : MonoBehaviour
{
    [SerializeField]
    Transform cardCarrier;

    SubstanceCard substanceCard;

    public SubstanceCard Card => substanceCard;

    public void SetCard(SubstanceCard substanceCard)
    {
        this.substanceCard = substanceCard;
        substanceCard.transform.SetParent(cardCarrier);
        substanceCard.SkipMovingAnimation();
        if(substanceCard.location.Equals(CardTransport.Location.MyDeck))
        {
            substanceCard.TracePosition(transform);
            substanceCard.transform.localRotation = Quaternion.identity;
        }
        else
        {
            substanceCard.transform.localPosition = Vector3.zero;
            substanceCard.transform.localRotation = Quaternion.identity;
        }
    }
    public void OnAnimationEnd()
    {
        substanceCard.transform.SetParent(MatchManager.Instance.transform); //prevent destroy children before animation play
        substanceCard.location = CardTransport.Location.MyHandCard;
        MatchManager.Player.AddHandCard(substanceCard);
        Destroy(gameObject);
    }
}
