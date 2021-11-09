using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class CardSelectButton : MonoBehaviour
{
    [SerializeField]
    Text locationText;
    [SerializeField]
    Text substanceText;
    [SerializeField]
    Text capacityText;

    public Card Card { get; protected set; }
    int selectedAmount;
    public int SelectedAmount {
        get => selectedAmount;
        protected set {
            selectedAmount = value;
            capacityText.text = SelectedAmount + "/" + Card.CardAmount;
        }
    }

    public void Set(Card card)
    {
        locationText.text = CardTransport.LocationName(card.location);
        Card = card;
        substanceText.text = card.CurrentLanguageName;
        SelectedAmount = 0;
    }
    public bool OnButtonClick()
    {
        if (SelectedAmount == Card.CardAmount)
            return false;
        ++SelectedAmount;
        return true;
    }
    public void Undo()
    {
        if (SelectedAmount > 0)
            --SelectedAmount;
    }
}
