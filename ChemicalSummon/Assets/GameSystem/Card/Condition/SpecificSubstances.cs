using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecificSubstances : SubstanceCondition
{
    [SerializeField]
    List<Substance> whiteList = new List<Substance>();
    public List<Substance> WhiteList => whiteList;

    protected override string InitDescription()
    {
        if (whiteList.Count == 1)
            return General.LoadSentence("SpecificCard").ToString().Replace("$name", whiteList[0].name);
        string cardsStr = "";
        bool isFirst = true;
        foreach(var each in whiteList)
        {
            if (isFirst)
            {
                isFirst = false;
                cardsStr = each.name;
            }
            else
                cardsStr += "/" + each.name;
        }
        return General.LoadSentence("SpecificCards").ToString().Replace("$cards", cardsStr);
    }
    public override bool Accept(Substance substance)
    {
        return whiteList.Contains(substance);
    }
}
