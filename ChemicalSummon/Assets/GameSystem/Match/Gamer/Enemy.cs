using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Enemy : Gamer
{
    [SerializeField]
    EnemyThinking enemyThinkingPrefab;
    [SerializeField]
    Transform enemyThinkingPreview;
    public void AddEnemyAction(Action action)
    {
        EnemyThinking gamerAction = Instantiate(enemyThinkingPrefab, enemyThinkingPreview);
        gamerAction.action = action;
    }
    public EnemyAI AI => MatchManager.Match.EnemyAI;
    public override TurnType FusionTurn => TurnType.EnemyFusionTurn;
    public override TurnType AttackTurn => TurnType.EnemyAttackTurn;
    public StackedElementList<Reaction> ReactionsPriority => MatchManager.Match.EnemyReactionsPriority;
    public override List<Reaction> LearnedReactions => ReactionsPriority.Types;
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
            if (slot.Card.IsPhenomenon || slot.Card.MeltingPoint > burnDamage * 1000)
                return 0;
            return slot.Card.ATK; //burn the card as high ATK as possible
        }).Key;
        if(slot != null)
        {
            BurnSlot(slot, burnDamage);
        }
        DoStackedAction();
    }

    public override void SelectCard(List<SubstanceCard> cards, int amount, Action<StackedElementList<SubstanceCard>> resultReceiver, Action cancelAction)
    {
        throw new NotImplementedException();
    }

    public override void SelectSlot(bool includeMyField, bool includeEnemyField, SubstanceCard card)
    {
        throw new NotImplementedException();
    }
}
