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
    public string description;

    public TypeAndCountList<Substance> leftSubstances = new TypeAndCountList<Substance>();
    public TypeAndCountList<Substance> rightSubstances = new TypeAndCountList<Substance>();
    public TypeAndCountList<Substance> catalysts = new TypeAndCountList<Substance>();

    public int explosion, electric, heat, heatRequire, electricRequire;
    public List<ResearchStep> researchSteps = new List<ResearchStep>();
    public bool IsRequiredSubstance(Substance substance)
    {
        return GetRequiredAmount(substance) > 0;
    }
    public bool IsProducingSubstance(Substance substance)
    {
        return GetProducingSubstance(substance) > 0;
    }
    public int GetRequiredAmount(Substance substance) {
        return leftSubstances.StackCount(substance);
    }
    public int GetProducingSubstance(Substance substance)
    {
        return rightSubstances.StackCount(substance);
    }
    public static Reaction GetByName(string name)
    {
        return Resources.Load<Reaction>("Chemical/Reaction/" + name);
    }
    public static List<Reaction> GetAll()
    {
        return new List<Reaction>(Resources.LoadAll<Reaction>("Chemical/Reaction"));
    }
    public struct ReactionMethod
    {
        public Reaction reaction;
        public Dictionary<SubstanceCard, int> consumingCards;
        public ReactionMethod(Reaction reaction, Dictionary<SubstanceCard, int> consumingCards)
        {
            this.reaction = reaction;
            this.consumingCards = consumingCards;
        }
    }
    public static bool GenerateReactionMethod(Reaction reaction, Gamer gamer, List<SubstanceCard> consumableCards, SubstanceCard attacker, out ReactionMethod method)
    {
        if (reaction.heatRequire > gamer.HeatGem || reaction.electricRequire > gamer.ElectricGem || attacker != null && !consumableCards.Contains(attacker))
        {
            method = default(ReactionMethod);
            return false;
        }
        bool condition = true;
        bool addedAttacker = false;
        Dictionary<SubstanceCard, int> consumingCards = new Dictionary<SubstanceCard, int>();
        foreach (var pair in reaction.leftSubstances)
        {
            Substance requiredSubstance = pair.type;
            int requiredAmount = pair.count;
            foreach (SubstanceCard card in consumableCards)
            {
                if (card.Substance.Equals(requiredSubstance))
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
            method = new ReactionMethod(reaction, consumingCards);
            return true;
        }
        else
        {
            method = default(ReactionMethod);
            return false;
        }
    }
}
