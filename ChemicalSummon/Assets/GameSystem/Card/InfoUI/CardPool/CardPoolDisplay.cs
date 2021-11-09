using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class CardPoolDisplay : MonoBehaviour
{
    [SerializeField]
    Text poolNameText, cardAmountText;
    [SerializeField]
    InputField searchInputField;
    [SerializeField]
    Transform cardListTransform;
    [SerializeField]
    float cardScale;
    [Min(-1)]
    public int capacity = int.MaxValue;

    public Text PoolNameText => poolNameText;
    int cardAmount;
    public int CardAmount
    {
        get => cardAmount;
        set
        {
            cardAmount = value;
            string text = General.LoadSentence("CardAmount") + " " + value;
            if(capacity != int.MaxValue)
                text += "/" + capacity;
            cardAmountText.text = text;
        }
    }
    public readonly List<Card> cards = new List<Card>();
    public void Init(TypeAndCountList<CardHeader> substanceStacks)
    {
        Clear();
        foreach(var substanceStack in substanceStacks)
            AddNewCard(substanceStack.type, substanceStack.count);
        CardAmount = substanceStacks.TotalCount();
    }
    private void AddNewCard(CardHeader cardHeader, int amount = 1)
    {
        //anchor protects card scale collapse from grid layout arranging
        GameObject cardAnchor = new GameObject(cardHeader.name + " anchor", typeof(RectTransform));
        cardAnchor.transform.SetParent(cardListTransform);
        Card card = cardHeader.GenerateCard(amount);
        card.transform.SetParent(cardAnchor.transform);
        card.transform.localScale = Vector3.one * cardScale;
        card.SetDraggable(false);
        cards.Add(card);
    }
    public int AddCard(CardHeader cardHeader, int amount = 1)
    {
        int addableAmount = (CardAmount + amount <= capacity) ? amount : capacity - CardAmount;
        if (addableAmount <= 0)
            return 0;
        Card card = cards.Find(card => card.Header.Equals(cardHeader));
        if (card == null)
            AddNewCard(cardHeader, addableAmount);
        else
            card.CardAmount += addableAmount;
        CardAmount += addableAmount;
        return addableAmount;
    }
    public void RemoveOneCard(Card card)
    {
        card.RemoveAmount(1);
        if (card.CardAmount == 0)
        {
            cards.Remove(card);
            Destroy(card.transform.parent.gameObject);
        }
        --CardAmount;
    }
    public void Clear()
    {
        cardListTransform.DestroyAllChildren();
        cards.Clear();
        CardAmount = 0;
    }
    public void OnSearchFieldChange()
    {
        if(General.CurrentSceneIsWorld)
            WorldManager.Player.LockMovement = true;
        if(searchInputField.text.Length == 0)
        {
            cards.ForEach(card => card.transform.parent.gameObject.SetActive(true));
        }
        else
        {
            cards.ForEach(card => card.transform.parent.gameObject.SetActive(
                card.name.IndexOf(searchInputField.text, System.StringComparison.OrdinalIgnoreCase) >= 0
                || card.name.IndexOf(searchInputField.text, System.StringComparison.OrdinalIgnoreCase) >= 0));
        }
    }
    public void OnEndSearchFieldEdit()
    {
        if (General.CurrentSceneIsWorld)
            WorldManager.Player.LockMovement = false;
    }
}
