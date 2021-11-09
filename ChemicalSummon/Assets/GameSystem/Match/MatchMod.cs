using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class MatchMod : MonoBehaviour
{
    [Header("EchelonPhase")]
    [SerializeField]
    bool disableEchelonPhaseProgressing;

    [Header("LimitTurn")]
    [SerializeField]
    bool setLimitTurn;
    [SerializeField]
    [Min(2)]
    int limitTurn = 2;

    [Header("InitialTurn")]
    [SerializeField]
    bool setInitialTurn;
    [SerializeField]
    [Min(0)]
    int initialTurn = 0;
    [SerializeField]
    bool playerIsFirst;

    [Header("HP")]
    [SerializeField]
    bool setEnemyHP;
    [SerializeField]
    [Min(0)]
    int enemyHP = 0;
    [SerializeField]
    bool setPlayerHP;
    [SerializeField]
    [Min(0)]
    int playerHP = 0;

    [Header("Field")]
    [SerializeField]
    Substance enemySlot1;
    [SerializeField]
    Substance enemySlot2, enemySlot3, playerSlot1, playerSlot2, playerSlot3;

    [Header("Deck")]
    [SerializeField]
    bool setPlayerDeck;
    [SerializeField]
    Deck playerDeck;

    [Header("DrawPile(These disables deck setting)")]
    [SerializeField]
    List<Substance> enemyDrawPile;
    [SerializeField]
    List<Substance> playerDrawPile;

    [Header("HandCard")]
    [SerializeField]
    TypeAndCountList<Substance> enemyHandCards;
    [SerializeField]
    TypeAndCountList<Substance> playerHandCards;

    private Enemy Enemy => MatchManager.Enemy;
    private Player Player => MatchManager.Player;
    void Awake()
    {
        MatchManager.Instance.disableEchelonPhaseChange = disableEchelonPhaseProgressing;
        if (setLimitTurn)
        {
            MatchManager.Instance.onTurnStart.AddListener(() =>
            {
                if (MatchManager.Turn == limitTurn)
                {
                    MatchManager.Instance.Defeat();
                }
            });
        }
        if (setInitialTurn) MatchManager.SetInitialTurn(initialTurn, playerIsFirst ? (Gamer)Player : (Gamer)Enemy);
        if (setEnemyHP) Enemy.InitHP(enemyHP);
        if (setPlayerHP) Player.InitHP(playerHP);
        if (enemySlot1 != null)
            Enemy.Field.Slots[0].SetMainCard(enemySlot1.GenerateSubstanceCard());
        if (enemySlot2 != null)
            Enemy.Field.Slots[1].SetMainCard(enemySlot2.GenerateSubstanceCard());
        if (enemySlot3 != null)
            Enemy.Field.Slots[2].SetMainCard(enemySlot3.GenerateSubstanceCard());
        if (playerSlot1 != null)
            Player.Field.Slots[0].SetMainCard(playerSlot1.GenerateSubstanceCard());
        if (playerSlot2 != null)
            Player.Field.Slots[1].SetMainCard(playerSlot2.GenerateSubstanceCard());
        if (playerSlot3 != null)
            Player.Field.Slots[2].SetMainCard(playerSlot3.GenerateSubstanceCard());
        if (setPlayerDeck)
        {
            Player.deck = playerDeck;
        }
        if (enemyDrawPile.Count > 0)
        {
            Enemy.ContinuousAddDrawPile(Card.GenerateCards(enemyDrawPile));
            Enemy.deck = new Deck();
        }
        if (playerDrawPile.Count > 0)
        {
            Player.ContinuousAddDrawPile(Card.GenerateCards(playerDrawPile));
            Player.deck = new Deck();
        }
        enemyHandCards.ForEach(stackedSubstance => {
            for (int i = 0; i < stackedSubstance.count; ++i)
                Enemy.AddHandCard(stackedSubstance.type);
        });
        playerHandCards.ForEach(stackedSubstance => {
            for (int i = 0; i < stackedSubstance.count; ++i)
                Player.AddHandCard(stackedSubstance.type);
        });
    }
}
