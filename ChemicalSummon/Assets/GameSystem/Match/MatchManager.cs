using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 所有战斗内公用功能的集合
///  - 寻找物体(我方手牌、我方信息栏...)
///  , 生成卡牌
///  , 管理回合
/// </summary>
[DisallowMultipleComponent]
public class MatchManager : ChemicalSummonManager, IPointerDownHandler
{
    public static MatchManager Instance { get; protected set; }
    //inspector
    [Header("Field")]
    [SerializeField]
    CardField myField;
    [SerializeField]
    CardField enemyField;
    [SerializeField]
    Player player;
    [SerializeField]
    Enemy enemy;

    [Header("Info")]
    [SerializeField]
    CardInfoDisplay cardInfoDisplay;
    [SerializeField]
    FusionPanelButton fusionPanel;
    [SerializeField]
    DecideCardSelectButton cardSelectPanel;
    [SerializeField]
    FusionDisplay fusionDisplay;
    [SerializeField]
    MatchLogDisplay matchLogDisplay;
    [SerializeField]
    MessagePanel messagePanel;
    [SerializeField]
    ResultPanel resultPanel;

    [Header("Turn")]
    [SerializeField]
    FirstMoverDecider firstMoverDecider;
    [SerializeField]
    TurnPanel turnPanel;
    public UnityEvent onTurnStart;
    public UnityEvent onFusionFinish;
    public Animator animatedTurnPanel;

    [Header("Animation")]
    [SerializeField]
    GameObject attackEffectPrefab;
    [SerializeField]
    GameObject damageTextPrefab;
    [SerializeField]
    GameObject explosionEffectPrefab;
    [SerializeField]
    GameObject movingGemPrefab;
    [SerializeField]
    DrawCardAnchor drawCardAnchorPrefab;
    [SerializeField]
    Transform drawCardAnchorParent;

    [Header("SE/BGM")]
    [SerializeField]
    AudioClip clickSE;
    [SerializeField]
    AudioClip victorySE;

    //data
    /// <summary>
    /// 回合开始事件
    /// </summary>
    public static UnityEvent OnTurnStart => Instance.onTurnStart;
    /// <summary>
    /// 当前战斗关卡
    /// </summary>
    public static Match Match => PlayerSave.ActiveMatch;
    /// <summary>
    /// 环境温度
    /// </summary>
    public static float DefaultTempreture => 27.0f;
    /// <summary>
    /// 我方场地
    /// </summary>
    public static CardField MyField => Instance.myField;
    /// <summary>
    /// 敌方场地
    /// </summary>
    public static CardField EnemyField => Instance.enemyField;
    /// <summary>
    /// 我方手牌
    /// </summary>
    public static HandCardsArrange MyHandCards => Player.HandCardsDisplay;
    /// <summary>
    /// 我方信息栏
    /// </summary>
    public static Player Player => Instance.player;
    /// <summary>
    /// 敌方信息栏
    /// </summary>
    public static Enemy Enemy => Instance.enemy;
    /// <summary>
    /// 先手
    /// </summary>
    public static Gamer FirstMover { get; protected set; }
    /// <summary>
    /// 后手
    /// </summary>
    public static Gamer SecondMover => FirstMover.Opponent;
    TurnType currentTurnType;
    public static TurnType CurrentTurnType => Instance.currentTurnType;
    int turn;
    int echelonPhase;
    public bool disableEchelonPhaseChange;
    /// <summary>
    /// 卡牌信息栏
    /// </summary>
    public static CardInfoDisplay CardInfoDisplay => Instance.cardInfoDisplay;
    /// <summary>
    /// 融合列表
    /// </summary>
    public static FusionPanelButton FusionPanel => Instance.fusionPanel;
    /// <summary>
    /// 卡牌选择列表
    /// </summary>
    public static DecideCardSelectButton CardSelectPanel => Instance.cardSelectPanel;
    /// <summary>
    /// 融合展示
    /// </summary>
    public static FusionDisplay FusionDisplay => Instance.fusionDisplay;
    /// <summary>
    /// 行动历史栏
    /// </summary>
    public static MatchLogDisplay MatchLogDisplay => Instance.matchLogDisplay;
    /// <summary>
    /// 消息栏
    /// </summary>
    public static MessagePanel MessagePanel => Instance.messagePanel;
    /// <summary>
    /// 先手决定栏
    /// </summary>
    public static FirstMoverDecider FirstMoverDecider => Instance.firstMoverDecider;
    /// <summary>
    /// 回合栏
    /// </summary>
    public static TurnPanel TurnPanel => Instance.turnPanel;
    /// <summary>
    /// 结果页面
    /// </summary>
    public static ResultPanel ResultPanel => Instance.resultPanel;
    /// <summary>
    /// 是否对局结束
    /// </summary>
    public static bool IsMatchFinish => ResultPanel.IsMatchFinish;
    /// <summary>
    /// 回合
    /// </summary>
    public static int Turn => Instance.turn;
    /// <summary>
    /// 卡组阶段
    /// </summary>
    public static int EchelonPhase => Instance.echelonPhase;
    private void Awake()
    {
        ManagerInit(Instance = this);
        //set background and music
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = Match.PickRandomBGM();
        audioSource.Play();
        Instantiate(Match.BackGround);
        //deck install
        Player.deck = PlayerSave.ActiveDeck;
        Enemy.deck = Match.EnemyDeck;
        //mod
        Instantiate(Match.gameObject);
        //gamer info init
        Player.Init(Match.MySideCharacter);
        Enemy.Init(Match.EnemySideCharacter);
        onTurnStart.AddListener(Player.Field.UpdateCardsDraggable);
        Player.onHPChange.AddListener(() => { if (Player.HP <= 0) Defeat(); });
        Enemy.onHPChange.AddListener(() => { if (Enemy.HP <= 0) Victory(); });
        //first turn start
        if(FirstMover == null)
        {
            currentTurnType = TurnType.FirstMoverDecide;
            EchelonPhaseUp();
            matchLogDisplay.AddTurnLog(0, TurnTypeToString(TurnType.FirstMoverDecide));
            firstMoverDecider.Draw();
        }
    }
    public void Victory()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
        audioSource.clip = victorySE;
        audioSource.Play();
        Player.SpeakInMatch(Character.SpeakType.Win);
        Enemy.SpeakInMatch(Character.SpeakType.Lose);
        resultPanel.SetResult(true);
        Match.Win();
    }
    public void Defeat()
    {
        Player.SpeakInMatch(Character.SpeakType.Lose);
        Enemy.SpeakInMatch(Character.SpeakType.Win);
        resultPanel.SetResult(false);
    }
    /// <summary>
    /// 决定先手
    /// </summary>
    /// <param name="gamer"></param>
    public static void SetFirstMover(Gamer gamer)
    {
        FirstMover = gamer;
        TurnEnd();
    }
    public static void SetInitialTurn(int turn, Gamer firstMover)
    {
        Instance.turn = turn - 1;
        FirstMover = firstMover;
        TurnEnd();
    }
    /// <summary>
    /// 结束回合
    /// </summary>
    public static void TurnEnd() {
        Instance.TurnEnd_nonstatic();
    }
    /// <summary>
    /// 结束回合非静态函数(用于按钮事件)
    /// </summary>
    public void TurnEnd_nonstatic()
    {
        switch (CurrentTurnType)
        {
            case TurnType.MyFusionTurn:
                Player.FusionTurnEnd();
                break;
            case TurnType.MyAttackTurn:
                Player.AttackTurnEnd();
                break;
            case TurnType.EnemyFusionTurn:
                Enemy.FusionTurnEnd();
                break;
            case TurnType.EnemyAttackTurn:
                Enemy.AttackTurnEnd();
                break;
        }
        Player.RemoveAttackButtons();
        ++turn;
        if(turn == 1)
        {
            //initial draw
            for (int i = 0; i < 4; ++i)
            {
                Player.DrawCard();
                Enemy.DrawCard();
            }
            currentTurnType = FirstMover.FusionTurn;
        }
        else
        {
            switch(turn % 4)
            {
                case 0:
                    currentTurnType = FirstMover.FusionTurn;
                    break;
                case 1:
                    currentTurnType = FirstMover.AttackTurn;
                    break;
                case 2:
                    currentTurnType = SecondMover.FusionTurn;
                    break;
                case 3:
                    currentTurnType = SecondMover.AttackTurn;
                    break;
            }
        }
        turnPanel.SetTurn(turn, currentTurnType);
        matchLogDisplay.AddTurnLog(turn, TurnTypeToString(currentTurnType));
        if (turn % 16 == 0)
            EchelonPhaseUp(GamerStartNewTurn);
        else
            GamerStartNewTurn();
    }
    private static void GamerStartNewTurn()
    {
        Instance.onTurnStart.Invoke();
        switch (CurrentTurnType)
        {
            case TurnType.MyFusionTurn:
                Player.FusionTurnStart();
                break;
            case TurnType.MyAttackTurn:
                Player.AttackTurnStart();
                break;
            case TurnType.EnemyFusionTurn:
                Enemy.FusionTurnStart();
                break;
            case TurnType.EnemyAttackTurn:
                Enemy.AttackTurnStart();
                break;
            default:
                break;
        }
        Instance.animatedTurnPanel.GetComponent<AnimatedTurnPanel>().Play();
    }
    public static void EchelonPhaseUp(UnityAction afterAction = null)
    {
        if (EchelonPhase > 0 && Instance.disableEchelonPhaseChange)
        {
            afterAction?.Invoke();
            return;
        }
        int echelonPhase = ++Instance.echelonPhase;
        MatchLogDisplay.AddEchelonPhaseLog(echelonPhase);
        bool playerDone = false, enemyDone = false;
        Player.InstallEchelon(echelonPhase, () =>
        {
            if (enemyDone)
            {
                afterAction?.Invoke();
            }
            else
                playerDone = true;
        });
        Enemy.InstallEchelon(echelonPhase, () =>
        {
            if (playerDone)
            {
                afterAction?.Invoke();
            }
            else
                enemyDone = true;
        });
    }
    public static string TurnTypeToString(TurnType turnType)
    {
        switch (turnType)
        {
            case TurnType.FirstMoverDecide:
                return LoadSentence("DecidingFirstMover");
            case TurnType.MyFusionTurn:
                return LoadSentence("FusionTurn");
            case TurnType.MyAttackTurn:
                return LoadSentence("AttackTurn");
            case TurnType.EnemyFusionTurn:
                return LoadSentence("EnemyFusionTurn");
            case TurnType.EnemyAttackTurn:
                return LoadSentence("EnemyAttackTurn");
        }
        return null;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PlaySE(clickSE);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        foreach(RaycastResult rayResult in results)
        {
            GameObject obj = rayResult.gameObject;
            //if it is CardInfoDisplay
            if (obj.GetComponent<CardInfoDisplay>() != null)
                return; //keep info display shown
            //if it is card
            SubstanceCard card = obj.GetComponent<SubstanceCard>();
            if (card != null)
            {
                if (Player.TrySelectSlotEvent(card.Slot))
                {
                    return;
                }
                //set card info display
                if (card.InField || card.IsMySide && card.InGamerHandCards)
                {
                    CardInfoDisplay.SetCard(card);
                    return;
                }
                continue;
            }
            //if it is slot
            ShieldCardSlot slot = obj.GetComponent<ShieldCardSlot>();
            if (slot != null)
            {
                if(Player.TrySelectSlotEvent(slot))
                {
                    return;
                }
                continue;
            }
        }
        CardInfoDisplay.gameObject.SetActive(false);
    }
    //sounds
    public static void PlaySE(string seResourcePass)
    {
        AudioClip clip = Resources.Load<AudioClip>(seResourcePass);
        if (clip == null)
        {
            Debug.LogWarning(seResourcePass + " is not a valid AudioClip resource pass.");
            return;
        }
        PlaySE(clip);
    }
    public static void PlaySE(AudioClip clip)
    {
        if(clip != null)
            AudioSource.PlayClipAtPoint(clip, GameObject.FindGameObjectWithTag("SE Listener").transform.position);
    }
    //animations
    public static void StartAttackAnimation(ShieldCardSlot slot1, ShieldCardSlot slot2, UnityAction onBump)
    {
        slot1.SBA_Bump.target = Instance.transform;
        slot1.SBA_Bump.StartAnimation();
        if(slot2 != null)
        {
            slot2.SBA_Bump.target = Instance.transform;
            slot2.SBA_Bump.StartAnimation();
        }
        slot1.SBA_Bump.AddBumpAction(() =>
        {
            Instantiate(Instance.attackEffectPrefab, Instance.transform);
            if (onBump != null)
                onBump.Invoke();
        });
    }
    public static void StartExplosionAnimation(Field field)
    {
        foreach(var each in field.Slots)
            Instantiate(Instance.explosionEffectPrefab, Instance.transform).transform.position = each.transform.position;
    }
    public static void StartDamageAnimation(Vector3 startPosition, int damage, Gamer damagedGamer)
    {
        GameObject damageText = Instantiate(Instance.damageTextPrefab, MainCanvas.transform);
        damageText.transform.position = startPosition;
        damageText.GetComponent<Text>().text = (-damage).ToString();
        SBA_TracePosition trace = damageText.GetComponent<SBA_TracePosition>();
        trace.targetTransform = damagedGamer.StatusPanels.HPText.transform;
        trace.AddReachAction(() => {
            damagedGamer.SpeakInMatch(Character.SpeakType.Damage);
            damagedGamer.HP -= damage;
            Destroy(damageText);
        });
        trace.StartAnimation();
    }
    public static void StartGemMoveAnimation(Color color, Vector3 src, Vector3 dst, UnityAction reachAction = null)
    {
        GameObject gem = Instantiate(Instance.movingGemPrefab, Instance.transform);
        gem.GetComponent<Image>().color = color;
        SBA_TracePosition tracer = gem.GetComponent<SBA_TracePosition>();
        tracer.transform.position = src;
        tracer.SetTarget(dst);
        tracer.AddReachAction(reachAction);
        tracer.StartAnimation();
    }
    public static void StartDrawCardAnimation(SubstanceCard substanceCard)
    {
        DrawCardAnchor anchor = Instantiate(Instance.drawCardAnchorPrefab, Instance.drawCardAnchorParent);
        anchor.SetCard(substanceCard);
    }
}
