using UnityEngine;

[DisallowMultipleComponent]
public class MatchLogDisplay : MonoBehaviour
{
    [SerializeField]
    DrawLog drawLogPrefab;
    [SerializeField]
    DeclareAttackLog declareAttackLog;
    [SerializeField]
    AttackLog attackLogPrefab;
    [SerializeField]
    CardReturnDeckLog cardReturnDeckLogPrefab;
    [SerializeField]
    TurnLog turnLogPrefab;
    [SerializeField]
    EchelonPhaseLog echelonPhaseLog;
    [SerializeField]
    HPLog hpLogPrefab;
    [SerializeField]
    FusionLog fusionLogPrefab;
    [SerializeField]
    Transform scrollViewContent;

    public void AddTurnLog(int turn, string turnTypeStr)
    {
        Instantiate(turnLogPrefab, scrollViewContent).Set(turn, turnTypeStr);
    }
    public void AddDrawLog(Gamer gamer, SubstanceCard card)
    {
        Instantiate(drawLogPrefab, scrollViewContent).Set(gamer, card);
    }
    public void AddFusionLog(Gamer gamer, Reaction reaction)
    {
        Instantiate(fusionLogPrefab, scrollViewContent).Set(gamer, reaction);
    }
    public void AddBattleLog(SubstanceCard card1, SubstanceCard card2)
    {
        Instantiate(attackLogPrefab, scrollViewContent).Set(card1, card2);
    }
    public void AddDeclareAttackLog(SubstanceCard card)
    {
        Instantiate(declareAttackLog, scrollViewContent).Set(card);
    }
    public void AddAttackPlayerLog(SubstanceCard card)
    {
        Instantiate(attackLogPrefab, scrollViewContent).Set(card, card.Gamer.Opponent);
    }
    public void AddPlayerHPLog(Gamer gamer, int hpChange)
    {
        Instantiate(hpLogPrefab, scrollViewContent).Set(gamer, hpChange);
    }
    public void AddCardReturnDeckLog(SubstanceCard card, int amount)
    {
        Instantiate(cardReturnDeckLogPrefab, scrollViewContent).Set(card, amount);
    }
    public void AddEchelonPhaseLog(int echelonPhase)
    {
        Instantiate(echelonPhaseLog, scrollViewContent).Set(echelonPhase);
    }
}
