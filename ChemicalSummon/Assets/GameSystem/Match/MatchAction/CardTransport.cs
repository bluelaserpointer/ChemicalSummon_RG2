using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CardTransport
{
    public enum Location { Field, MyField, EnemyField, MyHandCard, MyDeck, EnemyHandCard, EnemyDeck, OffSite }
    public enum Method { Top, Bottom, Select }
    public static string LocationName(Location location)
    {
        switch (location)
        {
            case Location.OffSite:
                return ChemicalSummonManager.LoadSentence("OffSite");
            case Location.Field:
                return ChemicalSummonManager.LoadSentence("Field");
            case Location.MyField:
                return ChemicalSummonManager.LoadSentence("MyField");
            case Location.EnemyField:
                return ChemicalSummonManager.LoadSentence("EnemyField");
            case Location.MyHandCard:
                return ChemicalSummonManager.LoadSentence("MyHandCard");
            case Location.MyDeck:
                return ChemicalSummonManager.LoadSentence("MyDeck");
            case Location.EnemyHandCard:
                return ChemicalSummonManager.LoadSentence("EnemyHandCard");
            case Location.EnemyDeck:
                return ChemicalSummonManager.LoadSentence("EnemyDeck");
        }
        return "";
    }
    public static string MethodName(Method method)
    {
        switch (method)
        {
            case Method.Top:
                return ChemicalSummonManager.LoadSentence("Top");
            case Method.Bottom:
                return ChemicalSummonManager.LoadSentence("Bottom");
            case Method.Select:
                return "";
        }
        return "";
    }
    public static List<SubstanceCard> SearchCard(Location location, CardCondition condition)
    {
        List<SubstanceCard> cards = new List<SubstanceCard>();
        switch (location)
        {
            case Location.OffSite:
                Type conditionType = condition.GetType();
                if (conditionType.Equals(typeof(Condition_Any)))
                {
                    Debug.LogWarning("<ChemicalSummon>Denided trying get all cards from OffSite.");
                }
                else if (conditionType.Equals(typeof(SpecificSubstances)))
                {
                    ((SpecificSubstances)condition).WhiteList.ForEach(substance => cards.Add(SubstanceCard.GenerateSubstanceCard(substance, 99)));
                }
                else
                {
                    Debug.LogWarning("<ChemicalSummon>Using default card search on offsite for \"" + conditionType.Name + "\"  which is bad for performance.");
                    foreach(Substance substance in Substance.GetAll())
                    {
                        SubstanceCard card = SubstanceCard.GenerateSubstanceCard(substance);
                        if (condition.Accept(card))
                            cards.Add(card);
                        else
                            card.Dispose();
                    }
                }
                return cards;
            case Location.Field:
                cards = MatchManager.MyField.Cards.FindAll(card => condition.Accept(card));
                cards.AddRange(MatchManager.EnemyField.Cards.FindAll(card => condition.Accept(card)));
                return cards;
            case Location.MyField:
                return MatchManager.MyField.Cards.FindAll(card => condition.Accept(card));
            case Location.EnemyField:
                return MatchManager.EnemyField.Cards.FindAll(card => condition.Accept(card));
            case Location.MyHandCard:
                return MatchManager.Player.HandCards.FindAll(card => condition.Accept(card));
            case Location.MyDeck:
                return MatchManager.Player.DrawPile.FindAll(card => condition.Accept(card));
            case Location.EnemyHandCard:
                return MatchManager.Enemy.HandCards.FindAll(card => condition.Accept(card));
            case Location.EnemyDeck:
                return MatchManager.Enemy.DrawPile.FindAll(card => condition.Accept(card));
        }
        return null;
    }
    public static void AddCard(Gamer gamer, Location location, Method method, List<SubstanceCard> cards)
    {
        switch (location)
        {
            case Location.OffSite:
                return;
            case Location.Field:
                cards.ForEach(card => gamer.SelectSlot(true, true, card));
                break;
            case Location.MyField:
                cards.ForEach(card => gamer.SelectSlot(true, false, card));
                break;
            case Location.EnemyField:
                cards.ForEach(card => gamer.SelectSlot(false, true, card));
                break;
            case Location.MyHandCard:
                cards.ForEach(card => MatchManager.Player.AddHandCard(card));
                break;
            case Location.MyDeck:
                cards.ForEach(card => MatchManager.Player.AddDrawPile(card, method));
                break;
            case Location.EnemyHandCard:
                cards.ForEach(card => MatchManager.Enemy.AddHandCard(card));
                break;
            case Location.EnemyDeck:
                cards.ForEach(card => MatchManager.Enemy.AddDrawPile(card, method));
                break;
        }
    }
    public static void SelectCard(Gamer gamer, List<SubstanceCard> cards, Method method, int amount, Action<StackedElementList<SubstanceCard>> resultReceiver, Action cancelAction)
    {
        if (method.Equals(Method.Select))
        {
            gamer.SelectCard(cards, amount, resultReceiver, cancelAction);
        }
        else
        {
            StackedElementList<SubstanceCard> selectedCards = new StackedElementList<SubstanceCard>();
            int addedAmount = 0;
            switch (method)
            {
                case Method.Top:
                    foreach (var card in cards)
                    {
                        int lestRequired = amount - addedAmount;
                        if (card.CardAmount <= lestRequired)
                        {
                            selectedCards.Add(card, card.CardAmount);
                            addedAmount += card.CardAmount;
                        }
                        else
                        {
                            if (lestRequired > 0)
                            {
                                selectedCards.Add(card, lestRequired);
                            }
                            break;
                        }
                    }
                    break;
                case Method.Bottom:
                    for (int i = cards.Count - 1; i >= 0; --i)
                    {
                        SubstanceCard card = cards[i];
                        int lestRequired = amount - addedAmount;
                        if (card.CardAmount <= lestRequired)
                        {
                            selectedCards.Add(card, card.CardAmount);
                            addedAmount += card.CardAmount;
                        }
                        else
                        {
                            if (lestRequired > 0)
                            {
                                selectedCards.Add(card, lestRequired);
                            }
                            break;
                        }
                    }
                    break;
            }
            resultReceiver.Invoke(selectedCards);
        }
    }
    public static void Transport(bool isCopy, Gamer gamer, CardCondition cond, int amount, Location src, Method srcMethod, Location dst, Method dstMethod, Action afterAction, Action cancelAction = null)
    {
        SelectCard(gamer, SearchCard(src, cond), srcMethod, amount, (selectedCards) =>
        {
            if(!isCopy && !src.Equals(Location.OffSite))
            {
                selectedCards.ForEach(each => each.type.RemoveAmount(each.amount, SubstanceCard.DecreaseReason.Other));
            }
            List<SubstanceCard> cards = new List<SubstanceCard>();
            selectedCards.ForEach(selection => cards.Add(SubstanceCard.GenerateSubstanceCard(selection.type.Substance, selection.amount)));
            AddCard(gamer, dst, dstMethod, cards);
            if(afterAction != null)
                afterAction.Invoke();
        }, cancelAction);
    }
    public static bool CanTransport(Location src, CardCondition cond, int amount)
    {
        return SearchCard(src, cond).Count >= amount;
    }
}
