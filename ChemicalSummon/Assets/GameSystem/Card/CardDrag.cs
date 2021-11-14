using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 卡牌挪动
/// </summary>
public class CardDrag : Draggable
{
    Card card;
    public ShieldCardSlot CurrentSlot => transform.parent == null ? null : transform.GetComponentInParent<ShieldCardSlot>();
    
    private void Awake()
    {
        card = GetComponent<Card>();
    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        card.EnableShadow(true);
        if (General.CurrentSceneIsMatch)
        {
            if (MatchManager.Player.HandCardsDisplay.cards.Contains(gameObject))
            {
                MatchManager.Player.HandCardsDisplay.Remove(gameObject);
            }
        }
        card.transform.SetParent(AbstractManager.MainCanvas.transform);
        MatchManager.CardDragGuidance.StartCardDrag(card);
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        MatchManager.CardDragGuidance.ClearAllGuidances();
        card.EnableShadow(false);
        if (General.CurrentSceneIsMatch)
        {
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResults);
            foreach (RaycastResult eachResult in raycastResults)
            {
                GameObject hitUI = eachResult.gameObject;
                //Field (slot)
                ShieldCardSlot cardSlot = hitUI.GetComponent<ShieldCardSlot>();
                if (cardSlot != null)
                {
                    SubstanceCard substanceCard = card as SubstanceCard;
                    if(cardSlot.AllowSetAsMainCard(substanceCard))
                    {
                        cardSlot.SetMainCard(substanceCard);
                        return;
                    }
                    else
                    {
                        break;
                    }
                }
                //Fusion panel set card
                if(hitUI.Equals(MatchManager.CardDragAreaForOpenReactionList))
                {
                    if (MatchManager.OpenReactionListButton.TrySetCard(card))
                        break;
                }
            }
            //return to original place
            //TODO: acctually CurrentSlot always null because transform parent change after drag start. May should redesign this.
            if (CurrentSlot != null)
            {
                CurrentSlot.DisbandMainCard();
                MatchManager.Player.AddHandCard(card);
            }
            else if (MatchManager.Player.HandCards.Contains(card))
            {
                MatchManager.Player.HandCardsDisplay.Add(gameObject);
            }
            else
            {
                MatchManager.Player.AddHandCard(card);
            }
        }
    }
}
