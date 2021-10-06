using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class DeckListDisplay : MonoBehaviour
{
    [SerializeField]
    Transform buttonListTransform;
    [SerializeField]
    DeckButton deckButtonPrefab;

    List<Deck> decks = new List<Deck>();
    public List<Deck> Decks => decks;
    public void InitList(List<Deck> decks)
    {
        this.decks = decks;
        buttonListTransform.DestroyAllChildren();
        foreach(Deck deck in decks)
        {
            DeckButton deckButton = Instantiate(deckButtonPrefab, buttonListTransform);
            deckButton.deck = deck;
            deckButton.DeckNameText.text = deck.name;
        }
    }
    public void AddButtonAction(UnityAction<DeckButton> buttonAction)
    {
        foreach(Transform buttonTf in buttonListTransform)
        {
            DeckButton deckButton = buttonTf.GetComponent<DeckButton>();
            deckButton.Button.onClick.AddListener(() => buttonAction.Invoke(deckButton));
        }
    }
}
