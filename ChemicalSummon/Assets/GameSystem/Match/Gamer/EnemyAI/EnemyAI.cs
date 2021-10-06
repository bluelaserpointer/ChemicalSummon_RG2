using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAI : MonoBehaviour
{
    //data
    [HideInInspector]
    public Enemy Enemy => MatchManager.Enemy;
    public Player Player => MatchManager.Player;
    public Field Field => Enemy.Field;
    public List<SubstanceCard> HandCards => Enemy.HandCards;
    public abstract void FusionTurnStart();
    public abstract void AttackTurnStart();
    public abstract void ContinueAttack();
    public abstract void Defense(SubstanceCard attacker);
    /// <summary>
    /// 玩家反击融合的概率(不精确)
    /// </summary>
    /// <returns></returns>
    protected float GuessCounterPossibility(SubstanceCard attacker)
    {
        List<SubstanceCard> consumableCards = new List<SubstanceCard>();
        consumableCards.AddRange(Player.Field.Cards);
        consumableCards.Insert(0, attacker);
        float maxPossiblity = 0;
        int opponentHandCardCount = Player.HandCardCount;
        foreach (Reaction reaction in Player.LearnedReactions)
        {
            if (reaction.heatRequire > Player.HeatGem || reaction.electricRequire > Player.ElectricGem)
                continue;
            bool condition = true;
            bool addedAttacker = false;
            Dictionary<SubstanceCard, int> consumingCards = new Dictionary<SubstanceCard, int>();
            StackedElementList<Substance> lackedSubstances = new StackedElementList<Substance>();
            int lackCardCount = 0;
            foreach (var pair in reaction.LeftSubstances)
            {
                Substance requiredSubstance = pair.type;
                int requiredAmount = pair.amount;
                foreach (SubstanceCard card in consumableCards)
                {
                    if (card.Substance.Equals(requiredSubstance))
                    {
                        if (!addedAttacker && card.Equals(attacker))
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
                lackCardCount += requiredAmount;
                if (lackCardCount > opponentHandCardCount) //impossible reaction
                {
                    //print("luck of requiredAmount: " + requiredAmount + " of " + requiredSubstance.Name + " in " + reaction.Description);
                    condition = false;
                    break;
                }
                lackedSubstances.Add(requiredSubstance, requiredAmount);
            }
            if (condition && addedAttacker)
            {
                float possiblity = Mathf.Clamp(0.75f + 0.25F * (opponentHandCardCount - lackCardCount) / lackCardCount, 0, 1);
                foreach(var each in lackedSubstances)
                {
                    Substance substance = each.type;
                    if(!substance.IsPureElement)
                    {
                        int exposedCount = Player.exposedSubstances.CountStack(substance);
                        if (exposedCount > 0)
                        {
                            float subPossibility = 1;
                            int playerTotalCards = Player.DrawPile.Count + opponentHandCardCount;
                            for (int i = 0; i < opponentHandCardCount; ++i)
                            {
                                subPossibility *= (playerTotalCards - exposedCount - i) / (playerTotalCards - i);
                            }
                            print(substance.chemicalSymbol + " chance: " + (1 - subPossibility) + " of total " + playerTotalCards);
                            possiblity *= 1 - subPossibility;
                        }
                        else
                        {
                            //print("player never fusioned " + substance.chemicalSymbol);
                            possiblity = 0;
                        }
                    }
                }
                //print(reaction.description + " -> lackCardCount: " + lackCardCount + ", counter risk: " + possiblity);
                //ignore not dangerous counter
                if (possiblity > 0)
                {
                    float consumptionRate = (float)attacker.CardAmount / lackCardCount;
                    if (consumptionRate <= 1)
                        possiblity = 0; //never mind same or worse comsumption rate counter risk
                    else
                        possiblity *= consumptionRate;
                }
                if (possiblity > maxPossiblity)
                {
                    maxPossiblity = possiblity;
                    if (maxPossiblity >= 1)
                        return 1;
                }
            }
        }
        return maxPossiblity;
    }
}
