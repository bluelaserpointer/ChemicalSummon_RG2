using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewSubstanceStep : ResearchStep
{
    [SerializeField]
    Text messageText, nameText, threeStateText, atkText, echelonPhaseText;
    [SerializeField]
    Image cardImage;
    [SerializeField]
    Button nextStepButton;
    public override bool IsAutomatedStep => true;
    [HideInInspector]
    public Substance substance;

    public override void OnReach()
    {
        messageText.text = ChemicalSummonManager.LoadSentence("Analyzing") + ": " + substance.chemicalSymbol;
        Invoke("ShowAnalyzedData", 1);
        nextStepButton.gameObject.SetActive(false);
    }
    void ShowAnalyzedData()
    {
        nameText.text = substance.name;
        threeStateText.text = Substance.ThreeStateToString(substance.GetStateInTempreture(27));
        atkText.text = substance.atk.ToString();
        echelonPhaseText.text = substance.echelon.ToString();
        cardImage.sprite = substance.image;
        nextStepButton.gameObject.SetActive(true);
    }
}
