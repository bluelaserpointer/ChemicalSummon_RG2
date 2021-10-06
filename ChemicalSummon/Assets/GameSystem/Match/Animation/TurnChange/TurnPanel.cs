using UnityEngine;
using UnityEngine.UI;

public enum TurnType
{
    FirstMoverDecide,
    MyFusionTurn,
    MyAttackTurn,
    EnemyFusionTurn,
    EnemyAttackTurn
}
[DisallowMultipleComponent]
public class TurnPanel : MonoBehaviour
{
    [SerializeField]
    Text turnNumberText, turnTypeText;
    [SerializeField]
    TranslatableSentenceSO turnSentence;
    private void Start()
    {
        turnNumberText.text = turnSentence + " " + 0;
        turnTypeText.text = MatchManager.TurnTypeToString(TurnType.FirstMoverDecide);
    }
    public void SetTurn(int turnNumber, TurnType turnType)
    {
        turnNumberText.text = turnSentence + " " + turnNumber;
        turnTypeText.text = MatchManager.TurnTypeToString(turnType);
    }
}
