using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class FormulaStep : ResearchStep
{
    [SerializeField]
    Text messageText, formulaText, heatRqText, elecRqText, expText, heatText, elecText;

    [SerializeField]
    Button nextStepButton;

    public override bool IsAutomatedStep => true;

    public override void OnReach()
    {
        messageText.text = General.LoadSentence("Analyzing");
        Invoke("ShowAnalyzedData", 1);
        nextStepButton.gameObject.SetActive(false);
    }
    private void ShowAnalyzedData()
    {
        messageText.text = "";
        formulaText.text = Reaction.formula;
        heatRqText.text = Reaction.HeatRequire.ToString();
        elecRqText.text = Reaction.ElectricRequire.ToString();
        expText.text = Reaction.ExplosionPower.ToString();
        heatText.text = Reaction.Heat.ToString();
        elecText.text = Reaction.Electric.ToString();
        nextStepButton.gameObject.SetActive(true);
    }
}
