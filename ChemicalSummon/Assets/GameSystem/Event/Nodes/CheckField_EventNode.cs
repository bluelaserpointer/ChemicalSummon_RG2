using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class CheckField_EventNode : EventNode
{
    [SerializeField]
    bool checkPlayerField = true;
    [SerializeField]
    bool checkSomeOfThemExists = true;
    [SerializeField]
    List<Substance> fieldContainsList;

    private Field TargetField => checkPlayerField ? MatchManager.Player.Field : MatchManager.Enemy.Field;

    public override string PreferredGameObjectName
    {
        get
        {
            string listStr = "";
            fieldContainsList.ForEach(substance => listStr += " " + substance.chemicalSymbol);
            return (checkPlayerField ? "player" : "enemy") + " field has " + (checkSomeOfThemExists ? "one" : "all") + " of:" + listStr;
        }
    }

    public override void Reach()
    {
        ConversationWindow.Close();
        TargetField.onCardsChanged.AddListener(CheckField);
    }
    private void CheckField()
    {
        if(checkSomeOfThemExists)
        {
            if (TargetField.Cards.Find(card => fieldContainsList.Contains(card.Substance)) != null)
            {
                TargetField.onCardsChanged.RemoveListener(CheckField);
                ProgressEvent();
            }
        }
        else
        {
            List<SubstanceCard> fieldCards = new List<SubstanceCard>(TargetField.Cards);
            bool cond = true;
            foreach(Substance substance in fieldContainsList)
            {
                SubstanceCard card = fieldCards.Find(card => card.Substance.Equals(substance));
                if (card != null)
                {
                    fieldCards.Remove(card);
                }
                else
                {
                    cond = false;
                    break;
                }
            }
            if (cond)
            {
                TargetField.onCardsChanged.RemoveListener(CheckField);
                ProgressEvent();
            }
        }
    }
}
