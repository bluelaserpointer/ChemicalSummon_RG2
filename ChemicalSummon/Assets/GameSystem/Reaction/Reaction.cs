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
        public Reaction reaction;
        public Dictionary<Card, int> consumingCards;
        public ReactionMethod(Reaction reaction, Dictionary<Card, int> consumingCards)
        {
            this.reaction = reaction;
            this.consumingCards = consumingCards;
        }
    }
    public static bool GenerateReactionMethod(Reaction reaction, Gamer gamer, List<Card> consumableCards, SubstanceCard attacker, out ReactionMethod method)
    {
        if (attacker != null && !consumableCards.Contains(attacker))
        {
            method = default(ReactionMethod);
            return false;
        }
        bool condition = true;
        bool addedAttacker = false;
        Dictionary<Card, int> consumingCards = new Dictionary<Card, int>();
        foreach (var pair in reaction.leftSubstances)
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
