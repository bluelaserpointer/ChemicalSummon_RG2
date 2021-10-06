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
    SubstanceCard substanceCard;
    public ShieldCardSlot CurrentSlot => transform.parent == null ? null : transform.GetComponentInParent<ShieldCardSlot>();
    
    private void Awake()
    {
        substanceCard = GetComponent<SubstanceCard>();
    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        substanceCard.EnableShadow(true);
        if (ChemicalSummonManager.CurrentSceneIsMatch)
        {
            if (MatchManager.Player.HandCardsDisplay.cards.Contains(gameObject))
            {
                MatchManager.Player.HandCardsDisplay.Remove(gameObject);
            }
        }
        substanceCard.transform.SetParent(ChemicalSummonManager.MainCanvas.transform);
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        substanceCard.EnableShadow(false);
        if (ChemicalSummonManager.CurrentSceneIsMatch)
        {
            bool disbandable = CurrentSlot == null || CurrentSlot.AllowSlotClear(); //check dibandbility
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raycastResults);
            foreach (RaycastResult eachResult in raycastResults)
            {
                GameObject hitUI = eachResult.gameObject;
                ShieldCardSlot cardSlot = hitUI.GetComponent<ShieldCardSlot>();
                if (cardSlot != null)
                {
                    if (cardSlot.Equals(CurrentSlot))
                    {
                        CurrentSlot.DoAlignment();
                        return;
                    }
                    if (cardSlot.IsMySide)
                    {
                        if (!cardSlot.AllowSlotSet(substanceCard.gameObject))
                            continue;
                        if (cardSlot.IsEmpty)
                        {
                            //set slot
                            if (!disbandable)
                                continue;
                            if (CurrentSlot != null) //move from another slot
                            {
                                CurrentSlot.SlotTopClear();
                            }
                            else //move from handcard
                            {
                                MatchManager.Player.RemoveHandCard(substanceCard);
                            }
                            cardSlot.SlotSet(substanceCard.gameObject);
                            return;
                        }
                        else
                        {
                            SubstanceCard existedCard = cardSlot.Card;
                            if (existedCard.Substance.Equals(substanceCard.Substance))
                            {
                                //union same cards
                                existedCard.UnionSameCard(substanceCard);
                                return;
                            }
                        }
                    }
                    continue;
                }
            }
            //TODO: acctually CurrentSlot always null because when drag start transform parent must be changed. redesign this.
            if (CurrentSlot != null)
            {
                if (disbandable)
                {
                    CurrentSlot.SlotTopClear();
                    MatchManager.Player.AddHandCard(substanceCard);
                }
            }
            else if (MatchManager.Player.HandCards.Contains(substanceCard))
            {
                MatchManager.Player.HandCardsDisplay.Add(gameObject);
            }
            else
            {
                MatchManager.Player.AddHandCard(substanceCard);
            }
        }
    }
}
