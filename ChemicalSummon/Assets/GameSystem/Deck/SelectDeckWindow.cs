using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class SelectDeckWindow : MonoBehaviour
{
    [SerializeField]
    DeckListDisplay deckListDisplay;
    [SerializeField]
    UnityEvent onSelect;

    DeckButton selectedDeckButton;
    public DeckButton SelectedDeckButton => selectedDeckButton;
    public Deck SelectedDeck => selectedDeckButton?.deck;
    private void Start()
    {
        deckListDisplay.InitList(PlayerSave.SavedDecks);
        deckListDisplay.AddButtonAction((button) =>
        {
            selectedDeckButton = button;
            onSelect.Invoke();
            Destroy(gameObject);
        });
    }
}
