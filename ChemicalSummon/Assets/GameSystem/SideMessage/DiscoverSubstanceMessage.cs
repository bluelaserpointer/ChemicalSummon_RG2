using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscoverSubstanceMessage : SideMessage
{
    [SerializeField]
    Image icon;
    [SerializeField]
    Text substanceNameText, expGainText;
    
    public void Set(Substance substance)
    {
        icon.sprite = substance.image;
        substanceNameText.text = substance.name + "(" + substance.formula + ")";
        expGainText.text = "+" + General.GameRule.SubstanceDiscoverExp + General.LoadSentence("ResearchExp");
    }
}
