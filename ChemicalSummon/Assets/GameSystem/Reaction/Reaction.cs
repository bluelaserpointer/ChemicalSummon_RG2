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

    public StackedElementList<Substance> leftSubstances = new StackedElementList<Substance>();
    public StackedElementList<Substance> rightSubstances = new StackedElementList<Substance>();
    public StackedElementList<Substance> catalysts = new StackedElementList<Substance>();

    public int explosion, electric, heat, heatRequire, electricRequire;
    public List<ResearchStep> researchSteps = new List<ResearchStep>();

    public StackedElementList<Substance> LeftSubstances => leftSubstances;
    public StackedElementList<Substance> RightSubstances => rightSubstances;
    public bool IsRequiredSubstance(Substance substance)
    {
        return GetRequiredAmount(substance) > 0;
    }
    public bool IsProducingSubstance(Substance substance)
    {
        return GetProducingSubstance(substance) > 0;
    }
    public int GetRequiredAmount(Substance substance) {
        foreach(var pair in LeftSubstances)
        {
            if(pair.type.Equals(substance))
            {
                return pair.amount;
            }
        }
        return 0;
    }
    public int GetProducingSubstance(Substance substance)
    {
        foreach (var pair in RightSubstances)
        {
            if (pair.type.Equals(substance))
            {
                return pair.amount;
            }
        }
        return 0;
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
        foreach (var pair in reaction.LeftSubstances)
        {
            Substance requiredSubstance = pair.type;
            int requiredAmount = pair.amount;
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
