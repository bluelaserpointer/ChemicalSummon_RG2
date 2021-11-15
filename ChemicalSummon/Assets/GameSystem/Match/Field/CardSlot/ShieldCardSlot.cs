using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 格挡区卡槽
/// </summary>
[DisallowMultipleComponent]
public class ShieldCardSlot : MonoBehaviour
{
    [SerializeField]
    CardSlot mainCardSlot, lapCardSlot;
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
    /// <summary>
    /// 主卡 卡槽
    /// </summary>
    public CardSlot MainCardSlot => mainCardSlot;
    /// <summary>
    /// 覆盖卡 卡槽
    /// </summary>
    public CardSlot LapCardSlot => lapCardSlot;
    /// <summary>
    /// 主卡
    /// </summary>
    public SubstanceCard MainCard => MainCardSlot.TopCard as SubstanceCard;
    /// <summary>
    /// 覆盖卡
    /// </summary>
    public Card LapCard => LapCardSlot.TopCard;
    /// <summary>
    /// 是否为空(只要主卡为空覆盖卡也空)
    /// </summary>
    public bool IsEmpty => MainCardSlot.IsEmpty;
    /// <summary>
    /// 温度
    /// </summary>
    public float Tempreture => MatchManager.DefaultTempreture;
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
        MainCardSlot.onClear.AddListener(field.onCardsChanged.Invoke);
        LapCardSlot.onClear.AddListener(field.onCardsChanged.Invoke);
    }
    /// <summary>
    /// 放置物质卡
    /// 不检查放置条件
    /// </summary>
    /// <param name="substanceCard"></param>
    public void SetMainCard(SubstanceCard substanceCard)
    {
        if (Equals(substanceCard.Slot)) //when card drag distance too short
        {
            MainCardSlot.DoAlignment(MainCard.transform);
            return;
        }
        //check if need update fusion list
        bool needUpdateFusionList = true;
        if(IsMySide)
        {
            switch (substanceCard.location)
            {
                case CardTransport.Location.MyField:
                case CardTransport.Location.MyHandCard:
                    needUpdateFusionList = false;
                    break;
            }
        }
        //place card
        if (MainCardSlot.IsEmpty)
        {
            substanceCard.Disband();
            MainCardSlot.SlotSet(substanceCard, () =>
            {
                placeAnimation.StartAnimation();
                MatchManager.PlaySE(placeSE);
            });
            MainCard.location = IsMySide ? CardTransport.Location.MyField : CardTransport.Location.EnemyField;
            MainCard.SetDraggable(CardDraggable);
        }
        else
            MainCard.TryUnion(substanceCard);
        if (needUpdateFusionList)
            MatchManager.OpenReactionListButton.UpdateFusionMethod();
        field.onCardsChanged.Invoke();
    }
    /// <summary>
    /// 添加覆盖卡
    /// </summary>
    /// <param name="card"></param>
    public void AddLapCard(Card card)
    {

    }
    /// <summary>
    /// 检查主卡放置条件,AI不会调用
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    public bool AllowSetAsMainCard(SubstanceCard card, bool enableWarnings = true)
    {
        if (card != null)
        {
            if (!card.GetStateInTempreture(Tempreture).Equals(ThreeState.Solid))
            {
                if(enableWarnings)
                    MatchManager.MessagePanel.WarnNotPlaceNonSolid();
                return false;
            }
            if (!Gamer.Equals(card.Gamer))
            {
                //TODO: warn not place to opponent field
                return false;
            }
            if (!Gamer.InFusionTurn)
            {
                if (enableWarnings)
                    MatchManager.MessagePanel.WarnNotPlaceBeforeFusionTurn();
                return false;
            }
            SubstanceCard placedCard = MainCard;
            if (placedCard != null && !placedCard.Unionable(card))
            {
                //TODO: warn different type of cards cannot be unioned
                return false;
            }
            return true;
        }
        return false;
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
    public void Damage(int dmg)
    {
        if (!IsEmpty)
        {
            MainCard.Damage(dmg);
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
    /// 去除主卡（与覆盖卡）
    /// </summary>
    public void DisbandMainCard()
    {
        MainCardSlot.Disband(MainCard.transform);
    }
    /// <summary>
    /// 去除所有覆盖卡
    /// </summary>
    public void DisbandLapCards()
    {
        LapCardSlot.Disband(LapCard.transform);
    }
    /// <summary>
    /// 拿回卡牌至手牌
    /// </summary>
    /// <returns></returns>
    public bool TakeBackCard()
    {
        if (IsEmpty)
            return false;
        SubstanceCard mainCard = MainCard;
        DisbandMainCard();
        Gamer.AddHandCard(mainCard);
        return true;
    }

    public void OnAlignmentEnd(Transform childTransform)
    {
        placeAnimation.StartAnimation();
        MatchManager.PlaySE(placeSE);
    }
}
