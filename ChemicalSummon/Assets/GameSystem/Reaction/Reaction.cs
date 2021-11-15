using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType { Explosion, Heat, Electronic, None }
/// <summary>
/// 反应式(静态数据)
/// </summary>
[CreateAssetMenu(fileName = "NewReaction", menuName = "Chemical/Reaction")]
public class Reaction : ScriptableObject
{
    public string formula;

    [Serializable]
    public struct Stats
    {
        public TypeAndCountList<Substance> leftSubstances;
        public TypeAndCountList<Substance> rightSubstances;
        public TypeAndCountList<Substance> catalysts;

        public int heatRequire, electricRequire;
        public int vigorousness, electric, heat, explosionDamage;
        public int ExplosionDamage => explosionDamage;
    }
    public Stats initialStats;

    /// <summary>
    /// 左式(消耗素材)
    /// </summary>
    public TypeAndCountList<Substance> LeftSubstances => initialStats.leftSubstances;
    /// <summary>
    /// 右式(生成素材)
    /// </summary>
    public TypeAndCountList<Substance> RightSubstances => initialStats.rightSubstances;
    /// <summary>
    /// 触媒(非消耗素材)
    /// </summary>
    public TypeAndCountList<Substance> Catalysts => initialStats.catalysts;
    /// <summary>
    /// 热量消耗
    /// </summary>
    public int HeatRequire => initialStats.heatRequire;
    /// <summary>
    /// 电量消耗
    /// </summary>
    public int ElectricRequire => initialStats.electricRequire;
    /// <summary>
    /// 剧烈度
    /// </summary>
    public int Vigorousness => initialStats.vigorousness;
    /// <summary>
    /// 电量释放
    /// </summary>
    public int Electric => initialStats.electric;
    /// <summary>
    /// 热量释放
    /// </summary>
    public int Heat => initialStats.heat;
    /// <summary>
    /// 爆炸系数
    /// </summary>
    public int ExplosionDamage => initialStats.explosionDamage;

    public List<ResearchStep> researchSteps = new List<ResearchStep>();

    //data
    public bool IsRequiredSubstance(Substance substance)
    {
        return GetRequiredAmount(substance) > 0;
    }
    public bool IsProducingSubstance(Substance substance)
    {
        return GetProducingSubstance(substance) > 0;
    }
    public int GetRequiredAmount(Substance substance) {
        return LeftSubstances.StackCount(substance);
    }
    public int GetProducingSubstance(Substance substance)
    {
        return RightSubstances.StackCount(substance);
    }
    public static Reaction LoadFromResources(string name)
    {
        return Resources.Load<Reaction>(General.ResourcePath.Reaction + name);
    }
    public static List<Reaction> GetAll()
    {
        return new List<Reaction>(Resources.LoadAll<Reaction>(General.ResourcePath.Reaction));
    }
    public struct ReactionMethod
    {
        public Fusion fusion;
        public Dictionary<Card, int> consumingCards;
        public ReactionMethod(Fusion fusion, Dictionary<Card, int> consumingCards)
        {
            this.fusion = fusion;
            this.consumingCards = consumingCards;
        }
    }
    public static bool GenerateFusionMethod(Fusion fusion, Gamer gamer, List<Card> consumableCards, SubstanceCard attacker, out ReactionMethod method)
    {
        if (attacker != null && !consumableCards.Contains(attacker))
        {
            method = default(ReactionMethod);
            return false;
        }
        bool condition = true;
        bool addedAttacker = false;
        Dictionary<Card, int> consumingCards = new Dictionary<Card, int>();
        foreach (var pair in fusion.LeftSubstances)
        {
            Substance requiredSubstance = pair.type;
            int requiredAmount = pair.count;
            foreach (Card card in consumableCards)
            {
                SubstanceCard substanceCard = card as SubstanceCard;
                if (substanceCard == null)
                    continue; //TODO: magic card consuming
                if (substanceCard.Substance.Equals(requiredSubstance))
                {
                    if (attacker != null && !addedAttacker && card.Equals(attacker))
                    {
                        addedAttacker = true;
                    }
                    if (card.CardAmount >= requiredAmount)
                    {
                        consumingCards.Add(card, requiredAmount);
                        requiredAmount = 0;
                        break;
                    }
                    else
                    {
                        consumingCards.Add(card, card.CardAmount);
                        requiredAmount -= card.CardAmount;
                    }
                }
            }
            if (requiredAmount > 0)
            {
                //print("luck of requiredAmount: " + requiredAmount + " of " + requiredSubstance.Name + " in " + reaction.Description);
                condition = false;
                break;
            }
        }
        if (condition && (attacker == null || addedAttacker))
        {
            method = new ReactionMethod(fusion, consumingCards);
            return true;
        }
        else
        {
            method = default(ReactionMethod);
            return false;
        }
    }
}
