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
    public List<SubstanceCard> MainCards {
        get
        {
            List<SubstanceCard> cards = new List<SubstanceCard>();
            foreach (ShieldCardSlot slot in Slots)
            {
                SubstanceCard card = slot.MainCard;
                if (card != null)
                {
                    cards.Add(card);
                }
            }
            return cards;
        }
    }
    public SubstanceCard TopATKCard => MainCards.FindMostValuable(card => card.ATK).Key;
    public int TopATK => (int)(MainCards.FindMostValuable(card => card.ATK).Value);
    public bool CardsDraggable => IsMySide && MatchManager.CurrentTurnType.Equals(TurnType.MyFusionTurn);
    public void UpdateCardsDraggable()
    {
        bool cond = CardsDraggable;
        foreach (ShieldCardSlot slot in Slots)
        {
            if (!slot.IsEmpty && !slot.MainCard.DenideMove)
                slot.MainCard.SetDraggable(cond);
        }
    }
}
