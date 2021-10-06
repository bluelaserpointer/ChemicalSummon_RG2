using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// 物质卡(动态数据)
/// </summary>
[DisallowMultipleComponent]
public class SubstanceCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
    Text nameText;
    [SerializeField]
    Text amountText;
    [SerializeField]
    Text symbolText;
    [SerializeField]
    Image echelonLabel;
    [SerializeField]
    Text echelonText;
    [SerializeField]
    Image cardImage;
    [SerializeField]
    SBA_NumberApproachingTextMeshPro attackText;
    [SerializeField]
    Text meltingPointText, boilingPointText;
    [SerializeField]
    Text descriptionText;
    [SerializeField]
    AudioClip cardMoveSE;

    [SerializeField]
    List<Color> echelonColors = new List<Color>();

    //data
    Substance substance;
    /// <summary>
    /// 物质
    /// </summary>
    public Substance Substance
    {
        get => substance;
        set
        {
            substance = value;
            nameText.text = name;
            symbolText.text = Symbol;
            gameObject.name = "Card " + Symbol;
            MeltingPoint = substance.MeltingPoint;
            BoilingPoint = substance.BoilingPoint;
            mol = substance.GetMol();
            cardImage.sprite = Image;
            InitCardAmount(1);
            echelonText.text = Echelon.ToString();
            if(Echelon < 1 || Echelon > 3)
            {
                Debug.LogWarning(Symbol + " invalid echelon: " + Echelon);
            }
            else
            {
                echelonLabel.color = echelonColors[Echelon - 1];
            }
            descriptionText.text = Substance.description;
        }
    }
    int cardAmount;
    /// <summary>
    /// 卡牌数
    /// </summary>
    public int CardAmount
    {
        get => cardAmount;
        protected set {
            cardAmount = value;
            if (value != 1 && (IsCursorPointing || !ChemicalSummonManager.CurrentSceneIsMatch || Slot == null))
            {
                amountText.transform.parent.gameObject.SetActive(true);
                amountText.text = cardAmount.ToString();
            }
            else
            {
                amountText.transform.parent.gameObject.SetActive(false);
            }
            attackText.targetValue = ATK;
            if (Slot != null)
                Slot.Field.onCardsChanged.Invoke();
        }
    }
    /// <summary>
    /// 是否正在摧毁(RemoveAmount调用后使用)
    /// </summary>
    public bool IsDisposing => CardAmount <= 0;
    Gamer gamer;
    /// <summary>
    /// 所属游戏者
    /// </summary>
    public Gamer Gamer {
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
    /// 卡牌位置
    /// </summary>
    public CardTransport.Location location;
    /// <summary>
    /// 我方卡牌
    /// </summary>
    public bool IsMySide => MatchManager.Player.Equals(Gamer);
    /// <summary>
    /// 敌方卡牌
    /// </summary>
    public bool IsEnemySide => MatchManager.Enemy.Equals(Gamer);
    /// <summary>
    /// 所在卡槽
    /// </summary>
    public ShieldCardSlot Slot => cardDrag.CurrentSlot;
    /// <summary>
    /// 物质名
    /// </summary>
    public new string name => Substance.name;
    /// <summary>
    /// 化学表达式
    /// </summary>
    public string Symbol => Substance.chemicalSymbol;
    public int Echelon => Substance.echelon;
    /// <summary>
    /// 三态
    /// </summary>
    [HideInInspector]
    public ThreeState threeState = ThreeState.Gas;
    /// <summary>
    /// 卡牌图片
    /// </summary>
    public Sprite Image => Substance.Image;
    /// <summary>
    /// 是否为特殊卡: 现象
    /// </summary>
    public bool IsPhenomenon => Substance.isPhenomenon;
    int mol;
    /// <summary>
    /// 摩尔
    /// </summary>
    public int Mol => mol;
    /// <summary>
    /// 攻击力(最新值)
    /// </summary>
    public int ATK => ChemicalSummonManager.CurrentSceneIsMatch ? OriginalATK * CardAmount + ATKChange : OriginalATK;
    /// <summary>
    /// 攻击力变动
    /// </summary>
    public int ATKChange { get; set; }
    /// <summary>
    /// 是否禁止攻击
    /// </summary>
    public bool DenideAttack => IsPhenomenon;
    /// <summary>
    /// 是否禁止移动
    /// </summary>
    public bool DenideMove => IsPhenomenon;
    float meltingPoint;
    float boilingPoint;
    public float MeltingPoint
    {
        get => meltingPoint;
        set
        {
            meltingPoint = value;
            meltingPointText.text = value.ToString() + "℃";
        }
    }
    public float BoilingPoint {
        get => boilingPoint;
        set
        {
            boilingPoint = value;
            boilingPointText.text = value.ToString() + "℃";
        }
    }
    public AudioClip CardMoveSE => cardMoveSE;

    /// <summary>
    /// 与卡牌战斗
    /// </summary>
    /// <param name="opponentCard"></param>
    public void Battle(SubstanceCard opponentCard)
    {
        MatchManager.MatchLogDisplay.AddBattleLog(this, opponentCard);
        MatchManager.StartAttackAnimation(Slot, opponentCard.Slot, () => {
            MatchManager.PlaySE("Sound/SE/sword-kill-1");
            //int originalOpponentATK = opponentCard.ATK;
            opponentCard.Damage(ATK);
            //Damage(originalOpponentATK);
        });
    }
    /// <summary>
    /// 与玩家战斗(直接攻击)
    /// </summary>
    /// <param name="gamer"></param>
    public void Battle(Gamer gamer)
    {
        MatchManager.MatchLogDisplay.AddAttackPlayerLog(this);
        MatchManager.StartAttackAnimation(Slot, null, () => {
            MatchManager.PlaySE("Sound/SE/sword-kill-2");
            MatchManager.StartDamageAnimation(transform.position, ATK, gamer);
        });
    }
    public void Damage(int dmg)
    {
        int overDamage = dmg - ATK;
        if (overDamage >= 0)
        {
            if (!IsPhenomenon)
                RemoveAmount(1, DecreaseReason.Damage);
            if (overDamage > 0)
            {
                MatchManager.StartDamageAnimation(transform.position, overDamage, gamer);
            }
        }
    }

    /// <summary>
    /// 原本攻击力
    /// </summary>
    public int OriginalATK => Substance.ATK;
    public string Description => Substance.Description.ToString();
    /// <summary>
    /// 在我方手牌
    /// </summary>
    public bool InGamerHandCards => gamer != null && gamer.HandCards.Contains(this);
    /// <summary>
    /// 在我方卡组
    /// </summary>
    public bool InGamerDrawPile => gamer != null && gamer.DrawPile.Contains(this);
    /// <summary>
    /// 在场地(不考虑敌我)
    /// </summary>
    public bool InField => Slot != null;
    /// <summary>
    /// 在我方场地
    /// </summary>
    public bool InGamerField => IsMySide && InField;
    /// <summary>
    /// 初始化叠加数(防止数字增减动画播放)
    /// </summary>
    /// <param name="amount"></param>
    public void InitCardAmount(int amount)
    {
        CardAmount = amount;
        attackText.SetValueImmediate();
    }
    public void EnableShadow(bool enable)
    {
        //TODO: shadow
    }
    public ThreeState GetStateInTempreture(float tempreture)
    {
        return Substance.GetStateInTempreture(tempreture);
    }
    /// <summary>
    /// 合并同种类的卡(不会检查是否同种类)
    /// </summary>
    /// <param name="substanceCard"></param>
    public void UnionSameCard(SubstanceCard substanceCard)
    {
        CardAmount += substanceCard.CardAmount;
        substanceCard.Dispose();
    }
    public bool IsSameSubstance(SubstanceCard substanceCard)
    {
        return substance.Equals(substanceCard.substance);
    }
    static SubstanceCard baseSubstanceCard;
    /// <summary>
    /// 生成物质卡牌
    /// </summary>
    /// <param name="substance"></param>
    /// <returns></returns>
    public static SubstanceCard GenerateSubstanceCard(Substance substance, int amount = 1)
    {
        if (baseSubstanceCard == null)
        {
            baseSubstanceCard = Resources.Load<SubstanceCard>("SubstanceCard"); //Assets/GameSystem/Card/Resources/SubstanceCard
        }
        SubstanceCard card = Instantiate(baseSubstanceCard);
        card.Substance = substance;
        card.InitCardAmount(amount);
        card.location = CardTransport.Location.OffSite;
        return card;
    }
    public static List<SubstanceCard> GenerateSubstanceCard(List<Substance> substances)
    {
        if (baseSubstanceCard == null)
        {
            baseSubstanceCard = Resources.Load<SubstanceCard>("SubstanceCard"); //Assets/GameSystem/Card/Resources/SubstanceCard
        }
        List<SubstanceCard> list = new List<SubstanceCard>();
        foreach(Substance substance in substances)
        {
            list.Add(GenerateSubstanceCard(substance));
        }
        return list;
    }
    public static List<SubstanceCard> GenerateSubstanceCard(StackedElementList<Substance> substanceStacks, bool stackCard = false)
    {
        if (baseSubstanceCard == null)
        {
            baseSubstanceCard = Resources.Load<SubstanceCard>("SubstanceCard"); //Assets/GameSystem/Card/Resources/SubstanceCard
        }
        List<SubstanceCard> list = new List<SubstanceCard>();
        if (substanceStacks == null)
            return list;
        if(stackCard)
        {
            foreach (var substanceStack in substanceStacks)
            {
                list.Add(GenerateSubstanceCard(substanceStack.type, substanceStack.amount));
            }
        }
        else
        {
            foreach (var substanceStack in substanceStacks)
            {
                for (int i = 0; i < substanceStack.amount; ++i)
                {
                    list.Add(GenerateSubstanceCard(substanceStack.type));
                }
            }
        }
        return list;
    }
    /// <summary>
    /// 安全丢弃该卡
    /// </summary>
    public void Dispose()
    {
        if (Slot != null)
            Slot.SlotTopClear();
        else if (InGamerHandCards)
            gamer.RemoveHandCard(this);
        else if (InGamerDrawPile)
            gamer.RemoveDrawPile(this);
        Destroy(gameObject);
    }
    /// <summary>
    /// 减少原因
    /// </summary>
    public enum DecreaseReason { Damage, FusionMaterial, SkillCost, SkillEffect, Other }
    /// <summary>
    /// 减少叠加数(到零自动Dispose)
    /// </summary>
    /// <param name="amount"></param>
    /// <returns>是否至零</returns>
    public void RemoveAmount(int amount, DecreaseReason decreaseReason = DecreaseReason.Other)
    {
        int decreasedAmount;
        if (CardAmount > amount)
        {
            CardAmount -= (decreasedAmount = amount);
        }
        else
        {
            CardAmount -= (decreasedAmount = CardAmount);
        }
        if(ChemicalSummonManager.CurrentSceneIsMatch)
        {
            switch (decreaseReason)
            {
                case DecreaseReason.Damage:
                case DecreaseReason.FusionMaterial:
                    MatchManager.MatchLogDisplay.AddCardReturnDeckLog(this, decreasedAmount);
                    for (int i = 0; i < decreasedAmount; ++i)
                    {
                        SubstanceCard card = GenerateSubstanceCard(Substance);
                        card.transform.position = transform.position;
                        card.location = location;
                        card.gamer = Gamer;
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
                        MatchManager.FusionPanel.UpdateList();
                        break;
                }
            }
        }
        if(CardAmount == 0)
            Dispose();
    }
    public void SetDraggable(bool cond)
    {
        cardDrag.enabled = cond;
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
        if (CardAmount != 1)
        {
            amountText.transform.parent.gameObject.SetActive(true);
            amountText.text = cardAmount.ToString();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsCursorPointing = false;
        if (Slot != null)
        {
            amountText.transform.parent.gameObject.SetActive(false);
        }
    }
}