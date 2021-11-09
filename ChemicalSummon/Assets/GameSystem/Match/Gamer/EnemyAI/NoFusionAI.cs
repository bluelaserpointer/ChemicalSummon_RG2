using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class NoFusionAI : EnemyAI
{
    public override void AttackTurnStart()
    {
        attackedSlot.Clear();
        AttackTurnLoop();
    }
    public override void ContinueAttack()
    {
        AttackTurnLoop();
    }
    public override void FusionTurnStart()
    {
        OnFusionTurnLoop(0);
    }
    protected readonly Queue<Substance> aboutToSummonSubstances = new Queue<Substance>();
    protected readonly List<ShieldCardSlot> lestEmptySlots = new List<ShieldCardSlot>();
    protected readonly List<ShieldCardSlot> attackedSlot = new List<ShieldCardSlot>();
    public virtual void TakeBackCardsAction(int step)
    {
        ShieldCardSlot[] slots = Field.Slots;
        Enemy.AddEnemyAction(() =>
        {
            foreach (ShieldCardSlot slot in slots)
            {
                slot.TakeBackCard();
            }
            lestEmptySlots.Clear();
            lestEmptySlots.AddRange(slots);
            OnFusionTurnLoop(step + 1);
        });
    }
    public virtual void UpdateSubstancesTotalATK(int step)
    {
        TypeAndCountList<Substance> aboutToSummonSubstancesTotalATK = new TypeAndCountList<Substance>();
        foreach (SubstanceCard card in HandCards)
        {
            aboutToSummonSubstancesTotalATK.Add(card.Substance, card.ATK);
        }
        aboutToSummonSubstancesTotalATK.SortByTypeCount(false);
        aboutToSummonSubstances.Clear();
        foreach (var stack in aboutToSummonSubstancesTotalATK)
            aboutToSummonSubstances.Enqueue(stack.type);
        OnFusionTurnLoop(step + 1);
    }
    public virtual void PlaceCardsAction(int step)
    {
        if (aboutToSummonSubstances.Count == 0 || lestEmptySlots.Count == 0) {
            OnFusionTurnLoop(step + 1);
            return;
        }
        Substance aboutToSummonSubstance = aboutToSummonSubstances.Dequeue();
        foreach (ShieldCardSlot eachSlot in lestEmptySlots)
        {
            if (!aboutToSummonSubstance.GetStateInTempreture(eachSlot.Tempreture).Equals(ThreeState.Solid))
                continue;
            lestEmptySlots.Remove(eachSlot);
            Enemy.AddEnemyAction(() =>
            {
                foreach(SubstanceCard handCard in new List<Card>(HandCards))
                {
                    if(handCard.Substance.Equals(aboutToSummonSubstance))
                        eachSlot.SetMainCard(handCard);
                }
                PlaceCardsAction(step);
            });
            return;
        }
        //when the substance cannot placed
        PlaceCardsAction(step);
    }
    public virtual void OnFusionTurnLoop(int step)
    {
        switch (step)
        {
            case 0:
                TakeBackCardsAction(step);
                break;
            case 1:
                UpdateSubstancesTotalATK(step);
                break;
            case 2:
                PlaceCardsAction(step);
                break;
            case 3:
                Enemy.AddEnemyAction(() =>
                {
                    MatchManager.TurnEnd();
                });
                break;
        }
    }
    public virtual void AttackTurnLoop()
    {
        ShieldCardSlot[] slots = Field.Slots;
        foreach (ShieldCardSlot slot in slots)
        {
            slot.HideAttackButton();
            if (!slot.IsEmpty)
                slot.MainCard.SetAlpha(1F);
        }
        if (MatchManager.IsMatchFinish)
        {
            return;
        }
        Enemy.AddEnemyAction(() =>
        {
            foreach (ShieldCardSlot slot in slots)
            {
                if (slot.IsEmpty || slot.MainCard.DenideAttack || attackedSlot.Contains(slot))
                    continue;
                attackedSlot.Add(slot);
                slot.ShowAttackButton();
                foreach (ShieldCardSlot notAttackingSlot in slots)
                {
                    if (!notAttackingSlot.Equals(slot) && !notAttackingSlot.IsEmpty)
                        notAttackingSlot.MainCard.SetAlpha(0.5F);
                }
                MatchManager.MatchLogDisplay.AddDeclareAttackLog(slot.MainCard);
                MatchManager.Player.Defense(slot.MainCard);
                return;
            }
            //no more slot can attack
            MatchManager.TurnEnd();
        });
    }

    public override void Defense(SubstanceCard attacker)
    {
        SubstanceCard candidateCard = Enemy.Field.TopATKCard;
        if (candidateCard == null)
        {
            attacker.Battle(Enemy);
            return;
        }
        int atk = candidateCard.ATK;
        if (atk > attacker.ATK)
        {
            attacker.Battle(candidateCard);
            return;
        }
        SubstanceCard subCandidateCard = null;
        foreach (var slot in Enemy.Field.Slots)
        {
            SubstanceCard card = slot.MainCard;
            if (card == null || card.Equals(candidateCard))
                continue;
            if (subCandidateCard == null || card.ATK > subCandidateCard.ATK)
                subCandidateCard = card;
        }
        int dangerHP = 10 + Random.Range(0, 5);
        if (subCandidateCard != null && Enemy.HP - attacker.ATK + subCandidateCard.ATK > dangerHP)
        {
            attacker.Battle(subCandidateCard);
        }
        else if (Enemy.HP - attacker.ATK > dangerHP)
        {
            attacker.Battle(Enemy);
        }
        else
        {
            attacker.Battle(candidateCard);
        }
    }
}
