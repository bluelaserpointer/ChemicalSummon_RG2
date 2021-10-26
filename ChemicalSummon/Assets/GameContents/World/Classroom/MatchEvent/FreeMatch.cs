using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class FreeMatch : Match
{
    public void OnSetEnemyDeck(SelectDeckWindow selectDeckWindow)
    {
        enemyDeck = selectDeckWindow.SelectedDeck;
    }
    public void OnSetEnemyReactions(SelectReactionWindow selectReactionWindow)
    {
        foreach(var each in selectReactionWindow.SelectedButtons)
        {
            enhanceReactions.Add(each.Reaction);
        }
    }
    public void OnSetEnemyCounterReactions(SelectReactionWindow selectReactionWindow)
    {
        foreach (var each in selectReactionWindow.SelectedButtons)
        {
            counterReactions.Add(each.Reaction);
        }
    }
}
