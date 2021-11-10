using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// ħ�����������Ƹ���
/// </summary>
[DisallowMultipleComponent]
public abstract class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //inspector
    [SerializeField]
    CardDrag cardDrag;
    [SerializeField]
    SBA_TracePosition tracer;
    [SerializeField]
    SBA_TraceRotation rotater;
    [SerializeField]
    CanvasGroup canvasGroup;
    [SerializeField]
    protected Image cardImage;
    [SerializeField]
    Text amountText;

    //data
    [HideInInspector]
    public CardAbility[] abilities;
    /// <summary>
    /// ��������(���ݵ�ǰ���Ա仯)
    /// </summary>
    public abstract string CurrentLanguageName { get; }

    Gamer gamer;
    /// <summary>
    /// ������Ϸ��
    /// </summary>
    public Gamer Gamer
    {
        get => gamer;
        set
        {
            gamer = value;
            if (gamer == null)
                SetDraggable(false);
            else if (IsEnemySide)
                transform.eulerAngles = new Vector3(0, 180, 180);
        }
    }
    /// <summary>
    /// �ҷ�����
    /// </summary>
    public bool IsMySide => MatchManager.Player.Equals(Gamer);
    /// <summary>
    /// �з�����
    /// </summary>
    public bool IsEnemySide => MatchManager.Enemy.Equals(Gamer);
    /// <summary>
    /// ���ҷ�����
    /// </summary>
    public bool InGamerHandCards => Gamer != null && Gamer.HandCards.Contains(this);
    /// <summary>
    /// ���ҷ�����
    /// </summary>
    public bool InGamerDrawPile => Gamer != null && Gamer.DrawPile.Contains(this);
    /// <summary>
    /// ���ڿ���
    /// </summary>
    public ShieldCardSlot Slot => cardDrag.CurrentSlot;
    /// <summary>
    /// �ڳ���(�����ǵ���)
    /// </summary>
    public bool InField => Slot != null;
    /// <summary>
    /// ���ҷ�����
    /// </summary>
    public bool InGamerField => IsMySide && InField;
    /// <summary>
    /// ����λ��
    /// </summary>
    public CardTransport.Location location;
    /// <summary>
    /// ���Ʊ�ͷ
    /// </summary>
    public abstract CardHeader Header { get; }
    int cardAmount;
    /// <summary>
    /// ������
    /// </summary>
    public virtual int CardAmount
    {
        get => cardAmount;
        set
        {
            cardAmount = value;
            amountText.text = cardAmount.ToString();
            amountText.transform.parent.gameObject.SetActive(value != 1);
            if (Slot != null)
                Slot.Field.onCardsChanged.Invoke();
        }
    }
    /// <summary>
    /// �Ƿ����ڴݻ�(RemoveAmount���ú�ʹ��)
    /// </summary>
    public bool IsDisposing => CardAmount <= 0;
    /// <summary>
    /// �Ƿ�ɺϲ�Ŀ�꿨��(������ͬ����)
    /// </summary>
    /// <param name="substanceCard"></param>
    public bool Unionable(Card card)
    {
        return Header.Equals(card.Header);
    }
    /// <summary>
    /// ���Ժϲ�ͬ���࿨�Ʋ��ɹ�ʱ�ݻ�
    /// </summary>
    /// <param name="card"></param>
    /// <returns>�Ƿ�ɹ�</returns>
    public bool TryUnion(Card card)
    {
        if (Unionable(card))
        {
            CardAmount += card.CardAmount;
            card.Dispose();
            return true;
        }
        return false;
    }
    /// <summary>
    /// ������ȥ����(���ܻ��Ϊ���ɼ������������¸��任)
    /// </summary>
    public void Disband()
    {
        if (Slot != null)
            Slot.DisbandMainCard();
        else if (InGamerHandCards)
            Gamer.RemoveHandCard(this);
        else if (InGamerDrawPile)
            Gamer.RemoveDrawPile(this);
    }
    /// <summary>
    /// ��ȫ�����ÿ�
    /// </summary>
    public void Dispose()
    {
        Disband();
        Destroy(gameObject);
    }
    /// <summary>
    /// ����ͼƬ
    /// </summary>
    public abstract Sprite Image { get; }
    /// <summary>
    /// �����Ƿ��Ų��
    /// </summary>
    /// <param name="cond"></param>
    public void SetDraggable(bool cond)
    {
        cardDrag.enabled = cond;
    }
    public static AudioClip CardMoveSE => Resources.Load<AudioClip>("Sound/SE/dealing_cards1");
    /// <summary>
    /// ����ԭ��
    /// </summary>
    public enum DecreaseReason { Damage, FusionMaterial, SkillCost, SkillEffect, Other }
    /// <summary>
    /// ���ٵ�����(�����Զ�Dispose)
    /// </summary>
    /// <param name="amount"></param>
    /// <returns>�Ƿ�����</returns>
    public void RemoveAmount(int amount, DecreaseReason decreaseReason = DecreaseReason.Other)
    {
        int decreasedAmount;
        CardAmount -= (decreasedAmount = (CardAmount < amount ? CardAmount : amount));
        if (General.CurrentSceneIsMatch)
        {
            switch (decreaseReason)
            {
                case DecreaseReason.Damage:
                case DecreaseReason.FusionMaterial:
                    MatchManager.MatchLogDisplay.AddCardReturnDeckLog(this, decreasedAmount);
                    for (int i = 0; i < decreasedAmount; ++i)
                    {
                        Card card = Header.GenerateCard();
                        card.transform.position = transform.position;
                        card.location = location;
                        card.Gamer = Gamer;
                        Gamer.AddDrawPile(card);
                    }
                    break;
            }
            if (IsMySide && (MatchManager.CurrentTurnType.Equals(MatchManager.Player.FusionTurn) || MatchManager.CurrentTurnType.Equals(MatchManager.Player.AttackTurn)))
            {
                switch (decreaseReason)
                {
                    //FusionMaterial case is done by FusionPanelButton
                    case DecreaseReason.Damage:
                    case DecreaseReason.SkillCost:
                    case DecreaseReason.SkillEffect:
                        MatchManager.OpenReactionListButton.UpdateList();
                        break;
                }
            }
        }
        if (CardAmount == 0)
            Dispose();
    }
    public void SetAlpha(float alpha)
    {
        canvasGroup.alpha = alpha;
    }
    public void TracePosition(Vector3 position, UnityAction reachAction = null)
    {
        tracer.SetTarget(position);
        tracer.AddReachAction(reachAction);
        tracer.StartAnimation();
    }
    public void TracePosition(Transform target, UnityAction reachAction = null)
    {
        tracer.SetTarget(target);
        tracer.AddReachAction(reachAction);
        tracer.StartAnimation();
    }
    public void TraceRotation(Quaternion rotation, UnityAction reachAction = null)
    {
        rotater.SetTarget(rotation);
        rotater.AddReachAction(reachAction);
        rotater.StartAnimation();
    }
    public void TraceRotation(Transform target, UnityAction reachAction = null)
    {
        rotater.SetTarget(target);
        rotater.AddReachAction(reachAction);
        rotater.StartAnimation();
    }
    public Vector3 TracePositionTarget => tracer.Target;
    public Quaternion TraceRotationTarget => rotater.Target;
    public void SkipMovingAnimation()
    {
        tracer.SkipAnimation();
        rotater.SkipAnimation();
    }
    public bool IsCursorPointing { get; protected set; }
    public void OnPointerEnter(PointerEventData eventData)
    {
        IsCursorPointing = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        IsCursorPointing = false;
    }
    public int Rank => Header.rank;
    public void EnableShadow(bool enable)
    {
        //TODO: shadow
    }
    public static List<Card> GenerateCards<T>(List<T> cardHeaders) where T : CardHeader
    {
        return cardHeaders == null ? null : cardHeaders.ConvertAll(header => header.GenerateCard());
    }
    public static List<Card> GenerateCards<T>(TypeAndCountList<T> cardHeaders) where T : CardHeader
    {
        List<Card> list = new List<Card>();
        if (cardHeaders == null)
            return list;
        foreach (var substanceStack in cardHeaders)
        {
            for (int i = 0; i < substanceStack.count; ++i)
                list.Add(substanceStack.type.GenerateCard());
        }
        return list;
    }
}