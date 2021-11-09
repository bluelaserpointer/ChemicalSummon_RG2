using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class EchelonPhaseLog : MonoBehaviour
{
    [SerializeField]
    Text logText;

    public void Set(int echelonPhase)
    {
        logText.text = General.LoadSentence("EchelonPhase") + " " + RomanNumerals.convert(echelonPhase);
    }
}
