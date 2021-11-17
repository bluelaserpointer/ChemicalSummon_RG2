using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 用户数据
/// </summary>
[Serializable]
[RequireComponent(typeof(DontDestroyOnLoad))]
public sealed class PlayerSave : MonoBehaviour
{
    private static PlayerSave instance;
    public static PlayerSave Instance {
        get
        {
            if (instance != null)
                return instance;
            return Instantiate(Resources.Load<PlayerSave>("__PlayerSave__"));
        }
    }
    public void OnFirstInit() //refered by DontDestroyOnLoad
    {
        instance = this;
        InitSaveData();
    }
    //inspector
    [SerializeField]
    General general;
    [SerializeField]
    Canvas permanentCanvas;
    [SerializeField]
    WorldHeader currentWorldHeader;
    [SerializeField]
    Vector3 currentCharacterPosition;
    [SerializeField]
    TypeAndCountList<Item> itemStorage;
    [SerializeField]
    TypeAndCountList<CardHeader> initialCardStorage;
    [SerializeField]
    List<Reaction> discoveredReactions;
    [SerializeField]
    Deck initialDeck;
    [SerializeField]
    Character currentCharacter;
    [SerializeField]
    int currentCharacterModelIndex;
    [SerializeField]
    List<Character> enabledCharacters;
    [SerializeField]
    List<Character> allCharacters;
    [SerializeField]
    Match activeMatch;

    Deck activeDeck = new Deck();
    List<Deck> savedDecks = new List<Deck>();
    Chapter activeChapter;
    List<Chapter> openedChapters = new List<Chapter>();
    List<Chapter> allChapters = new List<Chapter>();

    /// <summary>
    /// 研究等级总经验
    /// </summary>
    public static int ResearchExp { get; private set; }
    /// <summary>
    /// 研究等级
    /// </summary>
    public static int ResearchLevel { get; private set; }
    //data
    public static General General => Instance.general;
    public static Canvas PermanentCanvas => Instance.permanentCanvas;
    public static WorldHeader CurrentWorldHeader
    {
        get => Instance.currentWorldHeader;
        set => Instance.currentWorldHeader = value;
    }
    public static World CurrentWorld => WorldManager.World;
    public static int CurrentCharacterModelIndex
    {
        get => Instance.currentCharacterModelIndex;
        set => Instance.currentCharacterModelIndex = value;
    }
    public static Vector3 CurrentCharacterPosition
    {
        get => Instance.currentCharacterPosition;
        set => Instance.currentCharacterPosition = value;
    }
    
    public static bool hasLastWorldPositionSave;
    public static Vector3 lastWorldPlayerPosition;
    public static Quaternion lastWorldPlayerRotation;
    /// <summary>
    /// 持有道具
    /// </summary>
    public static TypeAndCountList<Item> ItemStorage => Instance.itemStorage;
    /// <summary>
    /// 持有卡牌(要添加应使用AddCard方法)
    /// </summary>
    public static TypeAndCountList<CardHeader> CardStorage => Instance.initialCardStorage;
    /// <summary>
    /// 可用的游戏者
    /// </summary>
    public static List<Character> EnabledCharacters => Instance.enabledCharacters;
    public static List<Character> AllCharacters => Instance.allCharacters;

    /// <summary>
    /// 发现的反应式(要添加应使用DiscoverReaction方法)
    /// </summary>
    public static List<Reaction> DiscoveredReactions => Instance.discoveredReactions;
    List<Reaction> newDiscoveredReactions = new List<Reaction>();
    /// <summary>
    /// 新发现的反应式
    /// </summary>
    public static List<Reaction> NewDicoveredReactions => Instance.newDiscoveredReactions;
    List<CardHeader> discoveredCards = new List<CardHeader>();
    /// <summary>
    /// 已发现的物质(要添加应使用DiscoverCard方法)
    /// </summary>
    public static List<CardHeader> DiscoveredCards => Instance.discoveredCards;
    /// <summary>
    /// 选定的游戏者
    /// </summary>
    public static Character SelectedCharacter {
        set => Instance.currentCharacter = value;
        get => Instance.currentCharacter;
    }
    /// <summary>
    /// 当前卡组
    /// </summary>
    public static Deck ActiveDeck {
        get => Instance.activeDeck;
        set => Instance.activeDeck = value;
    }
    
    /// <summary>
    /// 预留卡组
    /// </summary>
    public static List<Deck> SavedDecks => Instance.savedDecks;
    /// <summary>
    /// 当前章节
    /// </summary>
    public static Chapter ActiveChapter => Instance.activeChapter;
    /// <summary>
    /// 已开放章节
    /// </summary>
    public static List<Chapter> OpenedChapters => Instance.openedChapters;
    /// <summary>
    /// 预留章节
    /// </summary>
    public static List<Chapter> AllChapters => Instance.allChapters;
    /// <summary>
    /// 当前战斗
    /// </summary>
    public static Match ActiveMatch => Instance.activeMatch;
    public List<Event> aliveEvents = new List<Event>();
    /// <summary>
    /// 活动事件
    /// </summary>
    public static List<Event> AliveEvents => Instance.aliveEvents;
    Event activeEvent;
    /// <summary>
    /// 占用当前会话窗的事件
    /// </summary>
    public static Event ActiveEvent
    {
        get => Instance.activeEvent;
        set => Instance.activeEvent = value;
    }
    public void InitSaveData()
    {
        ResearchExp = 0;
        ResearchLevel = 1;
        hasLastWorldPositionSave = false;
        initialDeck.name = General.LoadSentence("Initiater");
        savedDecks.Add(initialDeck);
        activeDeck = initialDeck;
        foreach(var substanceStack in CardStorage)
        {
            discoveredCards.Add(substanceStack.type);
        }
        if (General.CurrentSceneIsWorld)
            WorldManager.OnPlayerDataLoaded();
    }
    private void Update()
    {
        //check new opened chapter
        foreach(Chapter chapter in allChapters)
        {
            if(!openedChapters.Contains(chapter) && chapter.JudgeCanStart())
            {
                openedChapters.Add(chapter);
                if(activeChapter == null)
                {
                    activeChapter = chapter;
                    chapter.Start();
                }
            }
        }
    }
    public static void AddResearchExp(int exp)
    {
        int currentExp = ResearchExp += exp;
        int level = 1;
        foreach (int expRequirement in General.researchLevelEachExp)
            if (currentExp >= expRequirement)
            {
                ++level;
                //TODO: level up event
            }
            else
                break;
        ResearchLevel = level;
        if (!General.CurrentSceneIsWorld)
            Debug.LogWarning("Don't add research exp when not in scene world.");
    }
    /// <summary>
    /// 增加卡(使用该函数才能解锁图鉴或获得经验)
    /// </summary>
    /// <param name="cardHeader"></param>
    public static void AddCard(CardHeader cardHeader, int amount = 1)
    {
        DiscoverCard(cardHeader);
        CardStorage.Add(cardHeader, amount);
    }
    public static void AddCard<T>(TypeAndCountList<T> cardHeaders) where T : CardHeader
    {
        foreach(var pair in cardHeaders)
            AddCard(pair.type, pair.count);
    }
    /// <summary>
    /// 发现卡
    /// </summary>
    /// <param name="cardHeader"></param>
    /// <returns></returns>
    public static bool DiscoverCard(CardHeader cardHeader)
    {
        if (cardHeader.IsSubstance && !DiscoveredCards.Contains(cardHeader))
        {
            DiscoveredCards.Add(cardHeader);
            AddResearchExp(General.GameRule.SubstanceDiscoverExp);
            DiscoverSubstanceMessage discoverSubstanceMessage = Instantiate(General.Instance.discoverSubstanceMessagePrefab);
            discoverSubstanceMessage.Set(cardHeader as Substance);
            General.Instance.messageGenerator.AddMessage(discoverSubstanceMessage);
            return true;
        }
        return false;
    }
    /// <summary>
    /// 发现反应式
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public static bool DiscoverReaction(Reaction reaction)
    {
        if (DiscoveredReactions.Contains(reaction))
            return false;
        DiscoveredReactions.Add(reaction);
        NewDicoveredReactions.Add(reaction);
        AddResearchExp(General.GameRule.ReactionDiscoverExp);
        return true;
    }
    /// <summary>
    /// 标记已查看反应式
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public static bool CheckedReaction(Reaction reaction)
    {
        return NewDicoveredReactions.Remove(reaction);
    }
    public static List<Reaction> FindDiscoveredReactionsByLeftSubstance(Substance substance)
    {
        List<Reaction> reactions = new List<Reaction>();
        foreach(Reaction reaction in DiscoveredReactions)
        {
            if (reaction.IsRequiredSubstance(substance))
                reactions.Add(reaction);
        }
        return reactions;
    }
    /// <summary>
    /// 进入战斗
    /// </summary>
    /// <param name="match"></param>
    public static void StartMatch(Match match)
    {
        Instance.activeMatch = match;
        if (General.CurrentSceneIsWorld)
        {
            hasLastWorldPositionSave = true;
            Transform playerTransform = WorldManager.Player.Model.transform;
            lastWorldPlayerPosition = playerTransform.position;
            lastWorldPlayerRotation = playerTransform.rotation;
        }
        SceneManager.LoadScene("Match");
    }
    public static Event StartEvent(Event eventPrefab) {
        Event newEvent = Instantiate(eventPrefab, General.eventsParent);
        Instance.aliveEvents.Add(newEvent);
        newEvent.Progress();
        return newEvent;
    }
}
