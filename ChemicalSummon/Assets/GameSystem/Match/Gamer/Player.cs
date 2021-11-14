﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class Player : Gamer
{
    [SerializeField]
    Text switchStackHandCardText;

    /// <summary>
    /// 即将使用的魔法卡（攻击强化、融合强化等情况被使用）
    /// </summary>
    public readonly List<MagicCard> aboutToUseMagicCards = new List<MagicCard>();

    public override TurnType FusionTurn => TurnType.MyFusionTurn;
    public override TurnType AttackTurn => TurnType.MyAttackTurn;
    public override List<Reaction> LearnedReactions => PlayerSave.DiscoveredReactions;
    public override void AddHandCard(Card substanceCard, bool playSE = true)
    {
        if (!substanceCard.location.Equals(CardTransport.Location.MyField) && !substanceCard.location.Equals(CardTransport.Location.MyHandCard))
            MatchManager.StartDrawCardAnimation(substanceCard);
        else
            base.AddHandCard(substanceCard, playSE);
    }
    public override void SetStackHandCardMode(bool cond)
    {
        base.SetStackHandCardMode(cond);
        switchStackHandCardText.text = General.LoadSentence(cond ? "DestackHandCard" : "StackHandCard");
    }
    public override void FusionTurnStart()
    {
        base.FusionTurnStart();
        MatchManager.Player.HandCardsDisplay.transform.position += new Vector3(0, 120, 0);
    }
    public override void FusionTurnEnd()
    {
        MatchManager.Player.HandCardsDisplay.transform.position -= new Vector3(0, 120, 0);
        MatchManager.OpenReactionListButton.HideFusionList();
    }
    public override void AttackTurnStart()
    {
        base.AttackTurnStart();
        foreach(ShieldCardSlot slot in Field.Slots)
        {
            if (slot.IsEmpty || slot.MainCard.DenideAttack)
                continue;
            SubstanceCard card = slot.MainCard;
            slot.ShowAttackButton(() =>
            {
                MatchManager.MatchLogDisplay.AddDeclareAttackLog(card);
                MatchManager.Enemy.Defense(card);
                slot.HideAttackButton();
            });
        }
    }
    public void RemoveAttackButtons()
    {
        foreach(ShieldCardSlot slot in Field.Slots)
        {
            slot.HideAttackButton();
        }
    }
    public override void Defense(SubstanceCard attacker)
    {
        CurrentAttacker = attacker;
        foreach (ShieldCardSlot slot in Field.Slots)
        {
            if (slot.IsEmpty || slot.MainCard.DenideAttack)
                continue;
            SubstanceCard card = slot.MainCard;
            slot.ShowAttackButton(() =>
            {
                attacker.Battle(card);
                EndDefence();
            });
        }
        MatchManager.OpenReactionListButton.UpdateList();
    }
    public void EndDefence()
    {
        if (CurrentAttacker == null)
            return;
        CurrentAttacker = null;
        RemoveAttackButtons();
        MatchManager.Enemy.ContinueAttack();
    }
    public void PlayerBlock()
    {
        if (CurrentAttacker == null)
            return;
        CurrentAttacker.Battle(this);
        EndDefence();
    }
    Func<ShieldCardSlot, bool> selectSlotAction;
    public void DoBurn_FireFairy(int burnDamage)
    {
        MatchManager.MessagePanel.SelectOpponentSlot();
        selectSlotAction = cardSlot =>
        {
            BurnSlot(cardSlot, burnDamage);
            MatchManager.MessagePanel.Hide();
            selectSlotAction = null;
            DoStackedAction();
            return true;
        };
    }
    /// <summary>
    /// 玩家点击了卡槽，如果有事件发生会返回True
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    public bool TrySelectSlotEvent(ShieldCardSlot slot)
    {
        if (selectSlotAction == null || slot == null)
            return false;
        selectSlotAction.Invoke(slot);
        return true;
    }

    public override void SelectCard(List<Card> cards, int amount, Action<TypeAndCountList<Card>> resultReceiver, Action cancelAction)
    {
        MatchManager.CardSelectPanel.InitList(amount, resultReceiver, cancelAction);
        cards.ForEach(card => MatchManager.CardSelectPanel.AddSelection(card));
    }

    public override void SelectSlot(bool includeMyField, bool includeEnemyField, Card card)
    {
        throw new NotImplementedException();
    }
    public override void DoFusion(Reaction.ReactionMethod method)
    {
        base.DoFusion(method);
        MatchManager.CardPreview.SetCardHeader(method.fusion.RightSubstances.list[0].type);
    }
}
