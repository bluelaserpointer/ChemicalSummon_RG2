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
        messageText.text = ChemicalSummonManager.LoadSentence("Analyzing");
        Invoke("ShowAnalyzedData", 1);
        nextStepButton.gameObject.SetActive(false);
    }
    private void ShowAnalyzedData()
    {
        messageText.text = "";
        formulaText.text = Reaction.description;
        heatRqText.text = Reaction.heatRequire.ToString();
        elecRqText.text = Reaction.electricRequire.ToString();
        expText.text = Reaction.explosion.ToString();
        heatText.text = Reaction.heat.ToString();
        elecText.text = Reaction.electricRequire.ToString();
        nextStepButton.gameObject.SetActive(true);
    }
}
