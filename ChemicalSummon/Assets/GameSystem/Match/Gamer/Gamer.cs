using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 游戏者战斗中信息栏
/// </summary>
public abstract class Gamer : MonoBehaviour
{
    //inspecter
    [SerializeField]
    Image faceImage;
    [SerializeField]
    Text gamerNameText;
    [SerializeField]
    StatusPanels statusPanels;
    [SerializeField]
    InMatchDialog inMatchDialog;
    [SerializeField]
    HandCardsArrange handCardsDisplay;
    [SerializeField]
    DrawPileDisplay drawPileDisplay;

    [SerializeField]
    UnityEvent onDrawPileChange, onHandCardsChanged;
    [SerializeField]
    UnityEvent onFusionTurnStart, onFusiontTurnEnd, onAttackTurnStart, onAttackTurnEnd;

    public UnityEvent OnFusionTurnStart => onFusionTurnStart;
    public UnityEvent OnFusionTurnEnd => onFusiontTurnEnd;
    public UnityEvent OnAttackTurnStart => onAttackTurnStart;
    public UnityEvent OnAttackTurnEnd => onAttackTurnEnd;
    public readonly UnityEvent<Reaction.ReactionMethod> onFusionExecute = new UnityEvent<Reaction.ReactionMethod>();
    //data
    /// <summary>
    /// 对手
    /// </summary>
    public Gamer Opponent => IsMySide ? MatchManager.Enemy : (Gamer)MatchManager.Player;
    /// <summary>
    /// 游戏者
    /// </summary>
    public Character Character { get; protected set; }
    /// <summary>
    /// 属性面板
    /// </summary>
    public StatusPanels StatusPanels => statusPanels;
    /// <summary>
    /// 战斗前卡组(静态卡组)
    /// </summary>
    public Deck deck;
    List<SubstanceCard> drawPile = new List<SubstanceCard>();
    /// <summary>
    /// 战斗内卡组(动态卡组)
    /// </summary>
    public List<SubstanceCard> DrawPile => drawPile;
    List<SubstanceCard> handCards = new List<SubstanceCard>();
    /// <summary>
    /// 手牌
    /// </summary>
    public List<SubstanceCard> HandCards => handCards;
    /// <summary>
    /// 手牌叠加模式
    /// </summary>
    public bool StackHandCardMode { get; protected set; }
    /// <summary>
    /// 可见手牌
    /// </summary>
    public HandCardsArrange HandCardsDisplay => handCardsDisplay;
    int hp;
    int heatGem, electricGem;
    /// <summary>
    /// 体力初始值
    /// </summary>
    public int InitialHP => Character.initialHP;
    /// <summary>
    /// 习得的反应式
    /// </summary>
    public abstract List<Reaction> LearnedReactions { get; }
    public SubstanceCard CurrentAttacker { get; protected set; }
    public readonly UnityEvent onHPChange = new UnityEvent();
    public readonly UnityEvent onHeatGemChange = new UnityEvent();
    public readonly UnityEvent onElectricGemChange = new UnityEvent();
    public int HP
    {
        get => hp;
        set
        {
            int change = value - HP;
            hp = value;
            MatchManager.MatchLogDisplay.AddPlayerHPLog(this, change);
            onHPChange.Invoke();
        }
    }
    public int HeatGem
    {
        get => heatGem;
        set
        {
            heatGem = value;
            onHeatGemChange.Invoke();
        }
    }
    public int ElectricGem
    {
        get => electricGem;
        set
        {
            electricGem = value;
            onElectricGemChange.Invoke();
        }
    }
    /// <summary>
    /// 是我方玩家
    /// </summary>
    public bool IsMySide => MatchManager.Player.Equals(this);
    /// <summary>
    /// 是敌方玩家
    /// </summary>
    public bool IsEnemyside => MatchManager.Enemy.Equals(this);
    /// <summary>
    /// 融合阶段
    /// </summary>
    public abstract TurnType FusionTurn { get; }
    /// <summary>
    /// 攻击阶段
    /// </summary>
    public abstract TurnType AttackTurn { get; }
    /// <summary>
    /// 在融合阶段
    /// </summary>
    public bool InFusionTurn => MatchManager.CurrentTurnType.Equals(FusionTurn);
    /// <summary>
    /// 在攻击阶段
    /// </summary>
    public bool InAttackTurn => MatchManager.CurrentTurnType.Equals(AttackTurn);
    /// <summary>
    /// 场地
    /// </summary>
    public Field Field => IsMySide ? MatchManager.MyField : MatchManager.EnemyField;
    /// <summary>
    /// 卡组变化事件
    /// </summary>
    public UnityEvent OnDrawPileChange => onDrawPileChange;
    /// <summary>
    /// 手牌变化事件
    /// </summary>
    public UnityEvent OnHandCardsChanged => onHandCardsChanged;
    [HideInInspector]
    public StackedElementList<Substance> exposedSubstances = new StackedElementList<Substance>();
    public void Init(Character character)
    {
        Character = character;
        SetStackHandCardMode(false);
        if (hp == 0) //if MatchStateChanger didnt jack hp
            hp = InitialHP;
        heatGem = 16;
        electricGem = 8;
        faceImage.sprite = character.FaceIcon;
        gamerNameText.text = character.Name;
        statusPanels.SetData(this);
    }
    public void InitHP(int value)
    {
        statusPanels.HPText.SetValueImmediate(hp = value);
    }
    public void InstallEchelon(int echelonNameIndex, UnityAction afterAction = null)
    {
        ContinuousAddDrawPile(SubstanceCard.GenerateSubstanceCard(deck.Echelons[echelonNameIndex - 1]), CardTransport.Method.Bottom,
            () => { ShuffleDrawPile(); afterAction?.Invoke(); });
    }
    public void ShuffleDrawPile()
    {
        drawPile = drawPile.Shuffle_InsideOut();
    }
    public virtual bool DrawCard()
    {
        if (DrawPile.Count > 0)
        {
            SubstanceCard card = RemoveDrawPileTop();
            MatchManager.MatchLogDisplay.AddDrawLog(this, card);
            AddHandCard(card);
            return true;
        }
        return false;
    }
    public void AddDrawPile(SubstanceCard card, CardTransport.Method method = CardTransport.Method.Bottom, UnityAction afterAction = null)
    {
        if (card.location.Equals(CardTransport.Location.OffSite))
        {
            card.transform.position = drawPileDisplay.CardSpawnPosition.position;
            card.transform.eulerAngles = card.IsMySide ? Vector3.zero : new Vector3(0, 180, 0);
        }
        card.Gamer = this;
        card.location = IsMySide ? CardTransport.Location.MyDeck : CardTransport.Location.EnemyDeck;
        card.SetDraggable(false);
        switch (method)
        {
            case CardTransport.Method.Bottom:
                drawPileDisplay.SlotSet(card, () => { DrawPile.Add(card); afterAction?.Invoke(); });
                break;
            case CardTransport.Method.Top:
                drawPileDisplay.SlotSet(card, () => { DrawPile.Insert(0, card); afterAction?.Invoke(); });
                break;
            case CardTransport.Method.Select:
                Debug.LogError("Insert to deck with specified index is not supported: " + card.Substance.name);
                break;
        }
        OnDrawPileChange.Invoke();
    }
    public void ContinuousAddDrawPile(List<SubstanceCard> cards, CardTransport.Method method = CardTransport.Method.Bottom, UnityAction afterAction = null)
    {
        if (cards.Count == 0)
        {
            afterAction?.Invoke();
            return;
        }
        AddDrawPile(cards.RemoveFirst(), method, () => ContinuousAddDrawPile(cards, method, afterAction));
    }
    public void RemoveDrawPile(SubstanceCard card)
    {
        if (DrawPile.Remove(card))
        {
            drawPileDisplay.SlotClear(card);
            OnDrawPileChange.Invoke();
        }
    }
    public SubstanceCard RemoveDrawPileTop()
    {
        if (DrawPile.Count == 0)
            return null;
        SubstanceCard card = DrawPile.RemoveFirst();
        drawPileDisplay.SlotClear(card);
        OnDrawPileChange.Invoke();
        return card;
    }
    public SubstanceCard RemoveDrawPileBottom()
    {
        if (DrawPile.Count == 0)
            return null;
        SubstanceCard card = DrawPile.RemoveLast();
        drawPileDisplay.SlotClear(card);
        OnDrawPileChange.Invoke();
        return card;
    }
    public SubstanceCard RemoveDrawPileRandom()
    {
        if (DrawPile.Count == 0)
            return null;
        SubstanceCard card = DrawPile.RemoveRandomElement();
        drawPileDisplay.SlotClear(card);
        OnDrawPileChange.Invoke();
        return card;
    }
    public SubstanceCard FindHandCard(Substance substance)
    {
        return HandCards.Find(card => card.Substance.Equals(substance));
    }
    public List<SubstanceCard> FindAllHandCard(Substance substance)
    {
        return HandCards.FindAll(card => card.Substance.Equals(substance));
    }
    public SubstanceCard FindHandCard(SubstanceCard substanceCard)
    {
        return FindHandCard(substanceCard.Substance);
    }
    /// <summary>
    /// 加入手牌
    /// </summary>
    /// <param name="substanceCard"></param>
    public virtual void AddHandCard(SubstanceCard substanceCard, bool playSE = true)
    {
        if(playSE)
            MatchManager.PlaySE(substanceCard.CardMoveSE);
        SubstanceCard duplicatedCard = FindHandCard(substanceCard);
        if (StackHandCardMode && duplicatedCard != null)
        {
            substanceCard.TracePosition(duplicatedCard.transform.position, () =>
            {
                duplicatedCard.UnionSameCard(substanceCard);
                MatchManager.FusionPanel.UpdateList();
                OnHandCardsChanged.Invoke();
            });
        }
        else
        {
            substanceCard.Gamer = this;
            HandCards.Add(substanceCard);
            substanceCard.location = IsMySide ? CardTransport.Location.MyHandCard : CardTransport.Location.EnemyHandCard;
            if (duplicatedCard == null)
                HandCardsDisplay.Add(substanceCard.gameObject);
            else
                HandCardsDisplay.Insert(HandCardsDisplay.cards.IndexOf(duplicatedCard.gameObject) + 1, substanceCard.gameObject);
            substanceCard.SetDraggable(IsMySide);
            MatchManager.FusionPanel.UpdateList();
            OnHandCardsChanged.Invoke();
        }
    }
    public void AddHandCard(Substance substance)
    {
        AddHandCard(SubstanceCard.GenerateSubstanceCard(substance));
    }
    public void RemoveHandCard(Substance substance)
    {
        RemoveHandCard(FindHandCard(substance));
    }
    public virtual bool RemoveHandCard(SubstanceCard substanceCard)
    {
        if (HandCards.Remove(substanceCard))
        {
            HandCardsDisplay.Remove(substanceCard.gameObject);
            OnHandCardsChanged.Invoke();
            return true;
        }
        return false;
    }
    /// <summary>
    /// 手牌总数
    /// </summary>
    /// <returns></returns>
    public int HandCardCount 
    {
        get {
            int count = 0;
            HandCards.ForEach(card => count += card.CardAmount);
            return count;
        }
    }
    public virtual void SetStackHandCardMode(bool cond)
    {
        if(StackHandCardMode = cond)
        {
            List<SubstanceCard> checkedCards = new List<SubstanceCard>();
            Dictionary<SubstanceCard, List<SubstanceCard>> unionPairs = new Dictionary<SubstanceCard, List<SubstanceCard>>();
            foreach(SubstanceCard card in new List<SubstanceCard>(HandCards))
            {
                SubstanceCard duplicatedCard = checkedCards.Find(each => each.IsSameSubstance(card));
                if (duplicatedCard != null)
                {
                    RemoveHandCard(card);
                    List<SubstanceCard> removingList;
                    if (!unionPairs.TryGetValue(duplicatedCard, out removingList))
                        unionPairs.Add(duplicatedCard, removingList = new List<SubstanceCard>());
                    removingList.Add(card);
                }
                else
                {
                    checkedCards.Add(card);
                }
            }
            foreach(var pair in unionPairs)
            {
                SubstanceCard remainingCard = pair.Key;
                foreach(SubstanceCard removingCard in pair.Value)
                {
                    removingCard.TraceRotation(remainingCard.TraceRotationTarget);
                    removingCard.TracePosition(remainingCard.TracePositionTarget, () =>
                    {
                        remainingCard.UnionSameCard(removingCard);
                    });
                }
            }
        }
        else
        {
            foreach (SubstanceCard card in new List<SubstanceCard>(HandCards))
            {
                while(card.CardAmount > 1)
                {
                    card.RemoveAmount(1);
                    SubstanceCard newCard = SubstanceCard.GenerateSubstanceCard(card.Substance);
                    newCard.location = IsMySide ? CardTransport.Location.MyHandCard : CardTransport.Location.EnemyHandCard;
                    newCard.transform.SetPositionAndRotation(card.transform.position, card.transform.rotation);
                    AddHandCard(newCard, false);
                }
            }
        }
    }
    public void SwitchStackHandCardMode()
    {
        SetStackHandCardMode(!StackHandCardMode);
    }
    /// <summary>
    /// 融合回合开始
    /// </summary>
    public virtual void FusionTurnStart()
    {
        SpeakInMatch(Character.SpeakType.StartFusion);
        foreach(var card in Opponent.Field.Cards)
        {
            if(card.Symbol.Equals("FireFairy"))
            {
                MatchManager.StartDamageAnimation(card.transform.position, (int)card.MeltingPoint / 1000, Opponent);
            }
        }
        OnFusionTurnStart.Invoke();
        DrawCard(); //抽牌
    }
    /// <summary>
    /// 融合回合结束
    /// </summary>
    public virtual void FusionTurnEnd()
    {
        OnFusionTurnEnd.Invoke();
    }
    /// <summary>
    /// 攻击回合开始
    /// </summary>
    public virtual void AttackTurnStart()
    {
        SpeakInMatch(Character.SpeakType.StartAttack);
        OnAttackTurnStart.Invoke();
    }
    /// <summary>
    /// 攻击回合结束
    /// </summary>
    public virtual void AttackTurnEnd()
    {
        OnAttackTurnEnd.Invoke();
    }
    /// <summary>
    /// 获得可消耗为融合素材的所有卡牌(手牌+场地)
    /// </summary>
    /// <returns></returns>
    public List<SubstanceCard> GetConsumableCards()
    {
        List<SubstanceCard> cards = new List<SubstanceCard>();
        cards.AddRange(HandCards);
        cards.AddRange(Field.Cards);
        return cards;
    }
    public abstract void Defense(SubstanceCard attacker);
    public void SpeakInMatch(string str)
    {
        inMatchDialog.SetText(str);
    }
    public void SpeakInMatch(Character.SpeakType speakType)
    {
        foreach(var pair in Character.speaks)
        {
            if (pair.speakType.Equals(speakType))
            {
                inMatchDialog.SetText(pair.translatableSentence);
                break;
            }
        }
    }
    public List<Reaction.ReactionMethod> FindAvailiableReactions(SubstanceCard attacker = null)
    {
        List<SubstanceCard> consumableCards = GetConsumableCards();
        //in counterMode, only counter fusions are avaliable
        if (attacker != null)
        {
            consumableCards.Insert(0, attacker);
        }
        List<Reaction.ReactionMethod> results = new List<Reaction.ReactionMethod>();
        foreach (Reaction reaction in LearnedReactions)
        {
            Reaction.ReactionMethod method;
            if(Reaction.GenerateReactionMethod(reaction, this, consumableCards, attacker, out method))
                results.Add(method);
        }
        return results;
    }
    public virtual void DoReaction(Reaction.ReactionMethod method)
    {
        onFusionExecute.Invoke(method);
        MatchManager.MatchLogDisplay.AddFusionLog(this, method.reaction);
        foreach (KeyValuePair<SubstanceCard, int> consume in method.consumingCards)
        {
            consume.Key.RemoveAmount(consume.Value, SubstanceCard.DecreaseReason.FusionMaterial);
        }
        //AddHandCard has updatability but also has animation time so we need update at this time
        MatchManager.FusionPanel.UpdateList();
        foreach (var pair in method.reaction.RightSubstances)
        {
            SubstanceCard newCard = SubstanceCard.GenerateSubstanceCard(pair.type);
            newCard.InitCardAmount(pair.amount);
            exposedSubstances.Add(newCard.Substance);
            AddHandCard(newCard);
        }
        Reaction reaction = method.reaction;
        //special
        if (reaction.heatRequire > 0)
            HeatGem -= reaction.heatRequire;
        if (reaction.electricRequire > 0)
            ElectricGem -= reaction.electricRequire;
        if (reaction.heat > 0)
            PushActionStack(() => DoBurn(reaction.heat));
        if (reaction.electric > 0)
            PushActionStack(() => DoElectricShock(reaction.electric));
        if (reaction.explosion > 0)
            PushActionStack(() => DoExplosion(reaction.explosion));
        if (reaction.explosion == 0)
            MatchManager.PlaySE("Sound/SE/powerup10");
        //talk
        SpeakInMatch(MatchManager.CurrentTurnType.Equals(FusionTurn) ? Character.SpeakType.Fusion : Character.SpeakType.Counter);
        //event invoke
        MatchManager.Instance.onFusionFinish.Invoke();
    }
    public virtual void DoBurn(int burnDamage)
    {
        for (int i = 0; i < burnDamage; ++i)
        {
            MatchManager.StartGemMoveAnimation(Color.red, MatchManager.FusionDisplay.transform.position, StatusPanels.HeatGemPanel.transform.position, () => ++HeatGem);
        }
        DoStackedAction();
    }
    public virtual void DoExplosion(int explosionDamage)
    {
        MatchManager.PlaySE("Sound/SE/attack2");
        MatchManager.StartExplosionAnimation(Opponent.Field);
        foreach (ShieldCardSlot cardSlot in Opponent.Field.Slots)
        {
            cardSlot.Damage(explosionDamage);
        }
        DoStackedAction();
    }
    public virtual void DoElectricShock(int electricDamage)
    {
        for (int i = 0; i < electricDamage; ++i)
        {
            MatchManager.StartGemMoveAnimation(Color.yellow, MatchManager.FusionDisplay.transform.position, StatusPanels.ElectricGemPanel.transform.position, () => ++ElectricGem);
        }
        DoStackedAction();
    }
    public bool BurnSlot(ShieldCardSlot cardSlot, int burnDamage)
    {
        if (!cardSlot.IsEmpty)
        {
            SubstanceCard placedCard = cardSlot.Card;
            if (placedCard.MeltingPoint < burnDamage * 1000)
            {
                cardSlot.TakeBackCard();
            }
            else //cannot overplace
            {
                //TODO: show invalid effect
                return false;
            }
        }
        //TODO: show valid effect
        SubstanceCard fireFairyCard = SubstanceCard.GenerateSubstanceCard(Substance.GetByName("FireFairy"));
        fireFairyCard.InitCardAmount(1);
        fireFairyCard.MeltingPoint = fireFairyCard.BoilingPoint = burnDamage * 1000;
        cardSlot.SlotSet(fireFairyCard.gameObject);
        return true;
    }
    Queue<Action> actionStack = new Queue<Action>();
    public bool HasStackedAction => actionStack.Count > 0;
    public void PushActionStack(Action action)
    {
        if (HasStackedAction)
        {
            actionStack.Enqueue(action);
        }
        else
        {
            actionStack.Enqueue(action);
            action.Invoke();
        }
    }
    public void DoStackedAction()
    {
        if (HasStackedAction)
        {
            actionStack.Dequeue();
            if (HasStackedAction)
                actionStack.Peek().Invoke();
        }
    }
    public abstract void SelectCard(List<SubstanceCard> cards, int amount, Action<StackedElementList<SubstanceCard>> resultReceiver, Action cancelAction);
    public abstract void SelectSlot(bool includeMyField, bool includeEnemyField, SubstanceCard card);
}
