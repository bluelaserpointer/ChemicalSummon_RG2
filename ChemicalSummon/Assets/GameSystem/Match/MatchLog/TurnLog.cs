using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class TurnLog : MonoBehaviour
{
    [SerializeField]
    Text turnText, turnTypeText;

    public void Set(int turn, string turnTypeStr)
    {
        turnText.text = ChemicalSummonManager.LoadSentence("Turn") + " " + turn.ToString();
        turnTypeText.text = turnTypeStr;
    }
}
