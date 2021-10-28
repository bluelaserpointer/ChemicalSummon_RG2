using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicFusionAI : NoFusionAI
{
    public virtual void FusionAction(int step)
    {
        int maxPriority = 0;
        Reaction.ReactionMethod candidateMethod = default;
        SubstanceCard topATKCard = Enemy.Field.TopATKCard;
       
        foreach (Reaction.ReactionMethod method in Enemy.FindAvailiableReactions(EnhanceReactions))
        {
            if (!Enemy.EnoughEnergyToDo(method.reaction))
                continue;
            //print("can process " + method.reaction.name);
            if (topATKCard != null && method.consumingCards.ContainsKey(topATKCard)) //hate using top ATK
            {
                //TODO: more smart way to decide
                if (Random.Range(0F, 1F) > 0.2F)
                {
                    maxPriority = 1;
                    candidateMethod = method;
                }
            }
            else
            {
                maxPriority = 1;
                candidateMethod = method;
            }
        }
        if (maxPriority > 0)
        {
            Enemy.AddEnemyAction(() =>
            {
                Enemy.DoFusion(candidateMethod);
                OnFusionTurnLoop(step); //recalculate next fusion
            });
            return;
        }
        else
            OnFusionTurnLoop(step + 1);
    }
    public override void OnFusionTurnLoop(int step)
    {
        switch (step)
        {
            case 0:
                FusionAction(step);
                break;
            case 1:
                TakeBackCardsAction(step);
                break;
            case 2:
                UpdateSubstancesTotalATK(step);
                break;
            case 3:
                PlaceCardsAction(step);
                break;
            case 4:
                Enemy.AddEnemyAction(() =>
                {
                    MatchManager.TurnEnd();
                });
                break;
        }
    }
    public override void AttackTurnLoop()
    {
        ShieldCardSlot[] slots = Field.Slots;
        foreach (ShieldCardSlot slot in slots)
        {
            slot.HideAttackButton();
            if (!slot.IsEmpty)
                slot.Card.SetAlpha(1F);
        }
        if (MatchManager.IsMatchFinish)
        {
            return;
        }
        int highestATK = Player.Field.TopATK;
        Enemy.AddEnemyAction(() =>
        {
            foreach (ShieldCardSlot slot in slots)
            {
                if (slot.IsEmpty || slot.Card.DenideAttack || attackedSlot.Contains(slot) || slot.Card.ATK < highestATK)
                    continue;
                //guess counter risk
                float counterRisk = GuessCounterPossibility(ConcernCounters, slot.Card);
                if (counterRisk == 1 || Random.Range(0, 1) < counterRisk)
                    continue;
                attackedSlot.Add(slot);
                slot.ShowAttackButton();
                foreach (ShieldCardSlot notAttackingSlot in slots)
                {
                    if (!notAttackingSlot.Equals(slot) && !notAttackingSlot.IsEmpty)
                        notAttackingSlot.Card.SetAlpha(0.5F);
                }
                MatchManager.MatchLogDisplay.AddDeclareAttackLog(slot.Card);
                Player.Defense(slot.Card);
                return;
            }
            //no more slot can attack
            MatchManager.TurnEnd();
        });
    }
    protected int JudgePriority(Reaction reaction)
    {
        int score = 0;
        score += reaction.explosion * 10;
        
        return score;
    }
    public override void Defense(SubstanceCard attacker)
    {
        SubstanceCard enemyStrongestCard = MatchManager.Enemy.Field.TopATKCard;
        int enemyStrongestATK = enemyStrongestCard == null ? 0 : enemyStrongestCard.ATK;
        int playerStrongestATK = MatchManager.Player.Field.TopATK;
        int maxPriority = 0;
        Reaction.ReactionMethod candidateMethod = default;
        //TODO: improve counter tactics
        foreach (Reaction.ReactionMethod method in Enemy.FindAvailiableReactions(EnhanceReactions, attacker))
        {
            if (!Enemy.EnoughEnergyToDo(method.reaction))
                continue;
            if (enemyStrongestATK > playerStrongestATK && method.consumingCards.ContainsKey(enemyStrongestCard))
            { //if our top ATK is higher than the player, should not do counter fusion that includes the highest ATK card
                continue;
            }
            maxPriority = 1;
            candidateMethod = method;
        }
        if (maxPriority == 0)
        {
            foreach (Reaction.ReactionMethod method in Enemy.FindAvailiableReactions(CounterReactions, attacker))
            {
                if (!Enemy.EnoughEnergyToDo(method.reaction))
                    continue;
                if (enemyStrongestATK > playerStrongestATK && method.consumingCards.ContainsKey(enemyStrongestCard))
                { //if our top ATK is higher than the player, should not do counter fusion that includes the highest ATK card
                    continue;
                }
                maxPriority = 1;
                candidateMethod = method;
            }
        }
        if (maxPriority > 0)
        {
            Enemy.AddEnemyAction(() =>
            {
                Enemy.DoFusion(candidateMethod);
            });
            return;
        }
        base.Defense(attacker);
    }
}
