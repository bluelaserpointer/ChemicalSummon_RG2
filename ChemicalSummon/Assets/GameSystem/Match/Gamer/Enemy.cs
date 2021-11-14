using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Enemy : Gamer
{
    [SerializeField]
    EnemyThinking enemyThinkingPrefab;
    public void AddEnemyAction(Action action)
    {
        EnemyThinking gamerAction = Instantiate(enemyThinkingPrefab, transform);
        gamerAction.action = action;
    }
    public EnemyAI AI => MatchManager.Match.EnemyAI;
    public override TurnType FusionTurn => TurnType.EnemyFusionTurn;
    public override TurnType AttackTurn => TurnType.EnemyAttackTurn;
    List<Reaction> learnedReactions = new List<Reaction>();
    public override List<Reaction> LearnedReactions => learnedReactions;
    protected override void AfterGamerInit()
    {
        learnedReactions.Clear();
        learnedReactions = MatchManager.Match.GetEnemyAllReactions();
    }
    public override void FusionTurnStart()
    {
        base.FusionTurnStart(); //card draw
        AI.FusionTurnStart();
    }
    public override void AttackTurnStart()
    {
        base.AttackTurnStart();
        AI.AttackTurnStart();
    }
    public void ContinueAttack()
    {
        AI.ContinueAttack();
    }
    public override void Defense(SubstanceCard attacker)
    {
        CurrentAttacker = attacker;
        AI.Defense(attacker);
        CurrentAttacker = null;
    }
    public void DoBurn_FireFairy(int burnDamage)
    {
        ShieldCardSlot slot = new List<ShieldCardSlot>(MatchManager.Player.Field.Slots).FindMostValuable(slot =>
        {
            if (slot.IsEmpty)
                return 0.1F;
            if (slot.MainCard.MeltingPoint > burnDamage * 1000)
                return 0;
            return slot.MainCard.ATK; //burn the card as high ATK as possible
        }).Key;
        if(slot != null)
        {
            BurnSlot(slot, burnDamage);
        }
        DoStackedAction();
    }

    public override void SelectCard(List<Card> cards, int amount, Action<TypeAndCountList<Card>> resultReceiver, Action cancelAction)
    {
        throw new NotImplementedException();
    }

    public override void SelectSlot(bool includeMyField, bool includeEnemyField, Card card)
    {
        throw new NotImplementedException();
    }
}
