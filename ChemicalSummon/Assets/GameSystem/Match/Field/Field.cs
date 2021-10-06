using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 场地，我方与敌方各一个
/// </summary>
[DisallowMultipleComponent]
public class Field : MonoBehaviour
{
    /// <summary>
    /// 卡牌发生变化
    /// </summary>
    public UnityEvent onCardsChanged;
    /// <summary>
    /// 是我方场地
    /// </summary>
    public bool IsMySide => MatchManager.MyField.Equals(this);
    /// <summary>
    /// 是敌方场地
    /// </summary>
    public bool IsEnemySide => MatchManager.EnemyField.Equals(this);
    /// <summary>
    /// 拥有该场地的游戏者
    /// </summary>
    public Gamer Gamer {
        get
        {
            if (IsMySide)
                return MatchManager.Player;
            if (IsEnemySide)
                return MatchManager.Enemy;
            return null;
        }
    }
    public ShieldCardSlot[] Slots => GetComponentsInChildren<ShieldCardSlot>();
    /// <summary>
    /// 所有卡牌
    /// </summary>
    public List<SubstanceCard> Cards {
        get
        {
            List<SubstanceCard> cards = new List<SubstanceCard>();
            foreach (ShieldCardSlot slot in Slots)
            {
                SubstanceCard card = slot.Card;
                if (card != null)
                {
                    cards.Add(card);
                }
            }
            return cards;
        }
    }
    public SubstanceCard TopATKCard => Cards.FindMostValuable(card => card.ATK).Key;
    public int TopATK => (int)(Cards.FindMostValuable(card => card.ATK).Value);
    /// <summary>
    /// 查看物质卡
    /// </summary>
    /// <param name="substance"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public List<SubstanceCard> FindSubstances(Substance substance, ref int amount)
    {
        List<SubstanceCard> results = new List<SubstanceCard>();
        if (amount > 0)
        {
            foreach (SubstanceCard card in Cards)
            {
                if (card.Substance.Equals(substance))
                {
                    results.Add(card);
                    amount -= card.CardAmount;
                    if (amount <= 0)
                    {
                        break;
                    }
                }
            }
        }
        return results;
    }
    /// <summary>
    /// 对手查看物质卡(只能查看暴露(格挡区)的卡牌)
    /// </summary>
    /// <param name="substance"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public List<SubstanceCard> FindSubstancesFromEnemy(Substance substance, int amount)
    {
        List<SubstanceCard> results = new List<SubstanceCard>();
        if (amount > 0)
        {
            foreach (SubstanceCard card in Cards)
            {
                if (card.Substance.Equals(substance))
                {
                    results.Add(card);
                    if (--amount == 0)
                    {
                        break;
                    }
                }
            }
        }
        return results;
    }
    public bool CardsDraggable => IsMySide && MatchManager.CurrentTurnType.Equals(TurnType.MyFusionTurn);
    public void UpdateCardsDraggable()
    {
        bool cond = CardsDraggable;
        foreach (ShieldCardSlot slot in Slots)
        {
            if (!slot.IsEmpty && !slot.Card.DenideMove)
                slot.Card.SetDraggable(cond);
        }
    }
}
