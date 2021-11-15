using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class EnhancementLabel : MonoBehaviour
{
    [SerializeField]
    Text nameText;
    [SerializeField]
    Image icon;

    //data
    public Card Card { get; protected set; }
    public CardAbility EnhancementAbility { get; protected set; }
    public void Set(Card card, CardAbility enhancementAbility)
    {
        Card = card;
        EnhancementAbility = enhancementAbility;
        if(card.abilities.Count == 1)
            nameText.text = enhancementAbility.Card.CurrentLanguageName;
        else
        {
            nameText.text = enhancementAbility.Card.CurrentLanguageName + "-" + card.abilities.IndexOf(enhancementAbility);
        }
        icon.sprite = enhancementAbility.Icon;
    }
    public void OnRemoveButtonPressed()
    {
        MatchManager.EnhancementList.RemoveEnhancement(this);
    }
}
