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
public class PlayerSave : MonoBehaviour
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
        ChemicalSummonManager.UpdateAllSentence();
        InitSaveData();
    }
    //inspector
    [SerializeField]
    Canvas permanentCanvas;
    [SerializeField]
    StackedElementList<Item> itemStorage;
    [SerializeField]
    StackedElementList<Substance> substanceStorage;
    [SerializeField]
    List<Reaction> discoveredReactions;
    [SerializeField]
    Deck initialDeck;
    [SerializeField]
    Character selectedCharacter;
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

    //data
    public static Canvas PermanentCanvas => Instance.permanentCanvas;
    public static bool hasLastWorldPositionSave;
    public static Vector3 lastWorldPlayerPosition;
    public static Quaternion lastWorldPlayerRotation;
    public static StackedElementList<Item> ItemStorage => Instance.itemStorage;
    public static StackedElementList<Substance> SubstanceStorage => Instance.substanceStorage;
    /// <summary>
    /// 可用的游戏者
    /// </summary>
    public static List<Character> EnabledCharacters => Instance.enabledCharacters;
    public static List<Character> AllCharacters => Instance.allCharacters;

    /// <summary>
    /// 发现的反应式
    /// </summary>
    public static List<Reaction> DiscoveredReactions => Instance.discoveredReactions;
    List<Reaction> newDiscoveredReactions = new List<Reaction>();
    /// <summary>
    /// 新发现的反应式
    /// </summary>
    public static List<Reaction> NewDicoveredReactions => Instance.newDiscoveredReactions;
    List<Substance> discoveredSubstances = new List<Substance>();
    /// <summary>
    /// 已发现的物质
    /// </summary>
    public static List<Substance> DiscoveredSubstances => Instance.discoveredSubstances;
    /// <summary>
    /// 选定的游戏者
    /// </summary>
    public static Character SelectedCharacter {
        set => Instance.selectedCharacter = value;
        get => Instance.selectedCharacter;
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
    Event activeEvent;
    /// <summary>
    /// 激活的事件
    /// </summary>
    public static Event ActiveEvent => Instance.activeEvent;
    public void InitSaveData()
    {
        hasLastWorldPositionSave = false;
        initialDeck.name = ChemicalSummonManager.LoadSentence("Initiater");
        savedDecks.Add(initialDeck);
        activeDeck = initialDeck;
        foreach(var substanceStack in SubstanceStorage)
        {
            discoveredSubstances.Add(substanceStack.type);
        }
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
    /// <summary>
    /// 增加发现的反应式
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public static bool AddDiscoveredReaction(Reaction reaction)
    {
        if (DiscoveredReactions.Contains(reaction))
            return false;
        DiscoveredReactions.Add(reaction);
        NewDicoveredReactions.Add(reaction);
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
        if (ChemicalSummonManager.CurrentSceneIsWorld)
        {
            hasLastWorldPositionSave = true;
            Transform playerTransform = WorldManager.Player.TargetModel.transform;
            lastWorldPlayerPosition = playerTransform.position;
            lastWorldPlayerRotation = playerTransform.rotation;
        }
        SceneManager.LoadScene("Match");
    }
    public static Event StartEvent(Event newEvent) {
        Instance.activeEvent = Instantiate(newEvent, PermanentCanvas.transform);
        ActiveEvent.Progress();
        return ActiveEvent;
    }
    public static void ProgressActiveEvent()
    {
        if (ActiveEvent != null)
            ActiveEvent.Progress();
    }
}
