using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class Match_EventNode : EventNode
{
    [SerializeField]
    Match match;

    public Match Match => match;

    public override string PreferredGameObjectName => Match != null ? Match.Name : "(?match?)";

    public override void Reach()
    {
        ConversationWindow.Close();
        PlayerSave.StartMatch(Match);
    }
}
