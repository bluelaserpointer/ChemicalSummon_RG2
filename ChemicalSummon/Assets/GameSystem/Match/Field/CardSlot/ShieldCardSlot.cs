using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 格挡区卡槽
/// </summary>
[DisallowMultipleComponent]
public class ShieldCardSlot : CardSlot, IAttackable
{
    [SerializeField]
    Field field;
    [SerializeField]
    AttackButton attackButton;
    [SerializeField]
    SBA_Bump sBA_Bump;
    [SerializeField]
    SBA_FadingExpand placeAnimation;
    [SerializeField]
    AudioClip placeSE;

    /// <summary>
    /// 所属场地
    /// </summary>
    public Field Field => field;
    /// <summary>
    /// 所属玩家
    /// </summary>
    public Gamer Gamer => Field.Gamer;
    public float Tempreture {
        get
        {
            if (!IsEmpty && Card.Substance.Equals(Substance.GetByName("FireFairy")))
                return Card.MeltingPoint;
            else
                return MatchManager.DefaultTempreture;
        }
    }
    /// <summary>
    /// 属于我方卡槽
    /// </summary>
    public bool IsMySide => IsBelongTo(MatchManager.MyField);
    /// <summary>
    /// 属于敌方卡槽
    /// </summary>
    public bool IsEnemySide => IsBelongTo(MatchManager.EnemyField);
    public virtual bool CardDraggable => Field.CardsDraggable;
    public SBA_Bump SBA_Bump => sBA_Bump;
    private void Awake()
    {
        onSet.AddListener(() => {
            if(ArrangeParent.childCount > 1)
            {
                SubstanceCard card = ArrangeParent.GetChild(0).GetComponent<SubstanceCard>();
                if(card.IsPhenomenon)
                {
                    Destroy(card.gameObject);
                }
                card.Gamer = Gamer;
            }
            bool needUpdateFusionList = true;
            switch(Card.location)
            {
                case CardTransport.Location.MyField:
                case CardTransport.Location.MyHandCard:
                    if (IsMySide)
                        needUpdateFusionList = false;
                    break;
                case CardTransport.Location.EnemyField:
                case CardTransport.Location.EnemyHandCard:
                    if (IsEnemySide)
                        needUpdateFusionList = false;
                    break;
            }
            if(needUpdateFusionList)
                MatchManager.FusionPanel.UpdateList();
            Card.location = IsMySide ? CardTransport.Location.MyField : CardTransport.Location.EnemyField;
            Card.SetDraggable(CardDraggable);
            field.onCardsChanged.Invoke();
        });
        onClear.AddListener(field.onCardsChanged.Invoke);
    }
    public void DoAlignment()
    {
        base.DoAlignment(GetTop());
    }
    /// <summary>
    /// 是否属于这个场地
    /// </summary>
    /// <param name="field"></param>
    /// <returns></returns>
    public bool IsBelongTo(Field field)
    {
        return this.field.Equals(field);
    }
    /// <summary>
    /// 是否属于同一个场地
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    public bool InSameField(ShieldCardSlot slot)
    {
        return field.Equals(slot.field);
    }
    public override bool AllowSlotSet(GameObject obj)
    {
        SubstanceCard substanceCard = obj.GetComponent<SubstanceCard>();
        if (substanceCard == null)
            return false;
        if (!substanceCard.GetStateInTempreture(Tempreture).Equals(ThreeState.Solid))
        {
            MatchManager.MessagePanel.WarnNotPlaceNonSolid();
            return false;
        }
        if (substanceCard.IsPhenomenon)
            return true;
        if (!Gamer.InFusionTurn)
        {
            MatchManager.MessagePanel.WarnNotPlaceBeforeFusionTurn();
            return false;
        }
        return true;
    }
    public bool AllowAttack(SubstanceCard card)
    {
        return false; //implement special attack
    }

    public void Attack(SubstanceCard card)
    {
    }
    public void Damage(int dmg)
    {
        if (!IsEmpty)
        {
            Card.Damage(dmg);
        }
        else
        {
            MatchManager.StartDamageAnimation(transform.position, dmg, Gamer);
        }
    }
    public void HideAttackButton()
    {
        attackButton.gameObject.SetActive(false);
    }
    public void ShowAttackButton(UnityAction buttonAction = null)
    {
        attackButton.gameObject.SetActive(true);
        attackButton.SetDirection(IsMySide);
        attackButton.SetButtonAction(buttonAction);
    }
    /// <summary>
    /// 拿回卡牌至手牌
    /// </summary>
    /// <returns></returns>
    public bool TakeBackCard()
    {
        if (IsEmpty || Card.IsPhenomenon)
            return false;
        SubstanceCard card = Card;
        SlotTopClear();
        Gamer.AddHandCard(card);
        return true;
    }

    public override void OnAlignmentEnd(Transform childTransform)
    {
        placeAnimation.StartAnimation();
        MatchManager.PlaySE(placeSE);
    }
}
