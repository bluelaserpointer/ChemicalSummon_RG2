using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 物质卡(动态数据)
/// </summary>
[DisallowMultipleComponent]
public class SubstanceCard : Card
{
    //inspector
    [SerializeField]
    Text formulaText;
    [SerializeField]
    Text currentLanguageNameText;
    [SerializeField]
    Image echelonLabel;
    [SerializeField]
    Text echelonText;
    [SerializeField]
    SBA_NumberApproachingTextMeshPro attackText;
    [SerializeField]
    Text meltingPointText, boilingPointText;
    [SerializeField]
    Text descriptionText;

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
            currentLanguageNameText.text = CurrentLanguageName;
            formulaText.text = Formula;
            gameObject.name = "Card " + Formula;
            MeltingPoint = substance.MeltingPoint;
            BoilingPoint = substance.BoilingPoint;
            mol = substance.GetMol();
            cardImage.sprite = Image;
            descriptionText.text = substance.description;
            InitCardAmount(1);
            echelonText.text = "Rank-" + Rank.ToString();
            if(Rank < 1 || Rank > 3)
            {
                Debug.LogWarning(Formula + " invalid echelon: " + Rank);
            }
            else
            {
                echelonLabel.color = echelonColors[Rank - 1];
            }
            if (substance.abilityPrefab == null)
                abilities = new CardAbility[0];
            else
            {
                GameObject abilitiesObject = Instantiate(substance.abilityPrefab, transform);
                abilitiesObject.name = "Abilities";
                abilities = abilitiesObject.GetComponentsInChildren<CardAbility>();
            }
        }
    }
    public override CardHeader Header => Substance;
    public override int CardAmount {
        get => base.CardAmount;
        set
        {
            base.CardAmount = value;
            if (General.CurrentSceneIsMatch)
                attackText.targetValue = ATK;
            else
                attackText.SetValueImmediate(ATK);
        }
    }
    /// <summary>
    /// 初始化叠加数(防止ATK值变化动画播放)
    /// </summary>
    /// <param name="amount"></param>
    public void InitCardAmount(int amount)
    {
        CardAmount = amount;
        attackText.SetValueImmediate();
    }
    /// <summary>
    /// 物质名（当前语言）
    /// </summary>
    public override string CurrentLanguageName => Substance.name;
    /// <summary>
    /// 化学表达式
    /// </summary>
    public string Formula => Substance.formula;
    /// <summary>
    /// 三态
    /// </summary>
    [HideInInspector]
    public ThreeState threeState = ThreeState.Gas;
    /// <summary>
    /// 卡牌图片
    /// </summary>
    public override Sprite Image => Substance.Image;
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
    public int ATK => General.CurrentSceneIsMatch ? OriginalATK * CardAmount + ATKChange : OriginalATK;
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
            RemoveAmount(1, DecreaseReason.Damage);
            if (overDamage > 0)
            {
                MatchManager.StartDamageAnimation(transform.position, overDamage, Gamer);
            }
        }
    }

    /// <summary>
    /// 原本攻击力
    /// </summary>
    public int OriginalATK => Substance.ATK;
    public ThreeState GetStateInTempreture(float tempreture)
    {
        return Substance.GetStateInTempreture(tempreture);
    }
    public bool IsSameSubstance(SubstanceCard substanceCard)
    {
        return substance.Equals(substanceCard.substance);
    }
}