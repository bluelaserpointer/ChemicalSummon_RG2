using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class DeckScreen : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    Transform deckListTransform;
    [SerializeField]
    DeckButton deckButtonPrefab;

    [SerializeField]
    InputField deckRenameInputField;
    [SerializeField]
    Button deckDeleteButton;

    [SerializeField]
    CardPreview cardPreview;
    [SerializeField]
    List<EchelonDisplay> echelons;
    [SerializeField]
    CardPoolDisplay storageCardPool;
    //data
    DeckButton selectingDeckButton;
    Deck SelectingDeck => selectingDeckButton?.deck;
    EchelonDisplay echelonOnEdit;
    TypeAndCountList<CardHeader> leftCardsInStorage;
    public static CardPreview CardPreview => WorldManager.DeckScreen.cardPreview;
    private void Start()
    {
        CardPreview.gameObject.SetActive(false);
    }
    public void Init()
    {
        foreach (Transform eachTf in deckListTransform)
        {
            DeckButton deckButton = eachTf.GetComponent<DeckButton>();
            if (deckButton == null)
                continue;
            Destroy(eachTf.gameObject);
        }
        foreach (Deck deck in PlayerSave.SavedDecks)
        {
            DeckButton deckButton = InstantiateDeckButton(deck);
            if (deck.Equals(PlayerSave.ActiveDeck))
                selectingDeckButton = deckButton;
        }
        SelectDeckButton(selectingDeckButton);
    }
    public DeckButton InstantiateDeckButton(Deck deck)
    {
        DeckButton deckButton = Instantiate(deckButtonPrefab, deckListTransform);
        deckButton.deck = deck;
        deckButton.DeckNameText.text = deck.name;
        deckButton.transform.SetSiblingIndex(deckListTransform.childCount - 2);
        deckButton.Button.onClick.AddListener(() =>
        {
            SelectDeckButton(deckButton);
        });
        return deckButton;
    }
    public void CreateNewDeck()
    {
        Deck deck = new Deck();
        deck.name = General.LoadSentence("NewDeck");
        PlayerSave.SavedDecks.Add(deck);
        SelectDeckButton(InstantiateDeckButton(deck));
    }
    public void SelectDeckButton(DeckButton deckButton)
    {
        EndEditEchelon();
        selectingDeckButton = deckButton;
        PlayerSave.ActiveDeck = deckButton.deck;
        deckRenameInputField.text = SelectingDeck.name;
        foreach (Transform eachTf in deckListTransform)
        {
            DeckButton eachDeckButton = eachTf.GetComponent<DeckButton>();
            if (eachDeckButton == null)
                continue;
            eachDeckButton.Lit(eachDeckButton.Equals(deckButton));
        }
        int echelonIndex = 0;
        leftCardsInStorage = new TypeAndCountList<CardHeader>(PlayerSave.CardStorage);
        foreach (var echelonCards in deckButton.deck.Echelons)
        {
            echelons[echelonIndex++].CardPool.Init(echelonCards);
            leftCardsInStorage.RemoveAll(echelonCards);
        }
        //TODO: display if player has enough cards to build this echelon
        storageCardPool.gameObject.SetActive(false);
    }
    public void DeleteDeck()
    {
        if(deckDeleteButton.image.color == Color.white)
        {
            deckDeleteButton.image.color = Color.red;
            deckDeleteButton.GetComponentInChildren<Text>().text += " ?";
        }
        else
        {
            if(echelonOnEdit == null)
            {
                PlayerSave.SavedDecks.Remove(SelectingDeck);
                int siblingIndex = selectingDeckButton.transform.GetSiblingIndex();
                Destroy(selectingDeckButton.gameObject);
                if (PlayerSave.SavedDecks.Count > 0)
                {
                    if (siblingIndex == deckListTransform.childCount - 2)
                        --siblingIndex;
                    else
                        ++siblingIndex;
                    SelectDeckButton(deckListTransform.GetChild(siblingIndex).GetComponent<DeckButton>());
                }
                else
                {
                    CreateNewDeck();
                }
            }
            else
            {
                foreach(var substanceStack in SelectingDeck.Echelons[echelonOnEdit.ArrayIndex])
                {
                    leftCardsInStorage.Add(substanceStack.type, substanceStack.count);
                    storageCardPool.AddCard(substanceStack.type, substanceStack.count);
                }
                echelonOnEdit.CardPool.Clear();
                SelectingDeck.Echelons[echelonOnEdit.ArrayIndex].Clear();
            }
            EndDeleteDeckRecheck();
        }
    }
    public void EndDeleteDeckRecheck()
    {
        if (deckDeleteButton.image.color == Color.red)
        {
            deckDeleteButton.image.color = Color.white;
            Text text = deckDeleteButton.GetComponentInChildren<Text>();
            string str = text.text;
            text.text = str.Substring(0, str.Length - 2);
        }
    }
    public void OnRenameDeckLabelChange()
    {
        if(deckRenameInputField.isFocused)
            WorldManager.Player.LockMovement = true;
    }
    public void RenameDeck()
    {
        EndDeleteDeckRecheck();
        selectingDeckButton.GetComponentInChildren<Text>().text = SelectingDeck.name = deckRenameInputField.text;
        WorldManager.Player.LockMovement = false;
    }
    public void EditEchelon(EchelonDisplay echelon)
    {
        if (echelonOnEdit == null)
        {
            echelonOnEdit = echelon;
            EndDeleteDeckRecheck();
            deckDeleteButton.GetComponentInChildren<Text>().text = General.LoadSentence("Clear");
            echelon.StartEdit();
            foreach (var each in echelons)
            {
                if (!each.Equals(echelon))
                    each.gameObject.SetActive(false);
            }
            TypeAndCountList<CardHeader> addableCards = new TypeAndCountList<CardHeader>();
            foreach(var substanceStack in leftCardsInStorage)
            {
                if(substanceStack.type.rank <= echelon.NameIndex)
                {
                    addableCards.Add(substanceStack);
                }
            }
            storageCardPool.gameObject.SetActive(true);
            storageCardPool.capacity = PlayerSave.CardStorage.TotalCount();
            storageCardPool.Init(addableCards);
        }
        else
        {
            EndEditEchelon();
        }
    }
    public void EndEditEchelon()
    {
        if(echelonOnEdit != null)
        {
            echelonOnEdit.EndEdit();
            echelonOnEdit = null;
        }
        EndDeleteDeckRecheck();
        deckDeleteButton.GetComponentInChildren<Text>().text = General.LoadSentence("Delete");
        foreach (var each in echelons)
        {
            each.gameObject.SetActive(true);
        }
        storageCardPool.gameObject.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        foreach (RaycastResult rayResult in results)
        {
            GameObject obj = rayResult.gameObject;
            //if it is CardInfoDisplay
            if (obj.GetComponent<CardPreview>() != null)
                return; //keep info display shown
            //if it is card
            Card card = obj.GetComponent<Card>();
            if(card != null)
            {
                if (eventData.button == PointerEventData.InputButton.Left)
                    CardPreview.SetCardHeader(card.Header);
                else if (eventData.button == PointerEventData.InputButton.Right)
                {
                    if (echelonOnEdit != null)
                    {
                        CardPoolDisplay belongCardPool = card.transform.GetComponentInParent<CardPoolDisplay>();
                        if (belongCardPool.Equals(storageCardPool))
                        {
                            echelonOnEdit.CardPool.AddCard(card.Header);
                            SelectingDeck.Echelons[echelonOnEdit.ArrayIndex].Add(card.Header);
                            storageCardPool.RemoveOneCard(card);
                            leftCardsInStorage.Remove(card.Header);
                        }
                        else if (belongCardPool.Equals(echelonOnEdit.CardPool))
                        {
                            storageCardPool.AddCard(card.Header);
                            leftCardsInStorage.Add(card.Header);
                            SelectingDeck.Echelons[echelonOnEdit.ArrayIndex].Remove(card.Header);
                            echelonOnEdit.CardPool.RemoveOneCard(card);
                        }
                    }
                }
                return;
            }
            CardPreview.gameObject.SetActive(false);
        }
    }
}
