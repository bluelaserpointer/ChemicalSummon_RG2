using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class AttackLog : MonoBehaviour
{
    [SerializeField]
    ImageWithTeamFrame attackerDisplay, targetDisplay;
    [SerializeField]
    Text attackerText, targetText, logText;
    public void Set(SubstanceCard attackerCard, SubstanceCard targetCard)
    {
        attackerDisplay.SetCard(attackerCard);
        targetDisplay.SetCard(targetCard);
        attackerText.text = attackerCard.ATK.ToString();
        targetText.text = targetCard.ATK.ToString();
        logText.text = ChemicalSummonManager.LoadSentence("AttackCard");
    }
    public void Set(SubstanceCard attackerCard, Gamer targetGamer)
    {
        attackerDisplay.SetCard(attackerCard);
        targetDisplay.SetGamer(targetGamer);
        attackerText.text = attackerCard.ATK.ToString();
        targetText.text = targetGamer.HP.ToString() + "HP";
        logText.text = ChemicalSummonManager.LoadSentence("AttackGamer");
    }
}
