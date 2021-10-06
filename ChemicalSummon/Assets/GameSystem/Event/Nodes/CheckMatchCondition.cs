using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMatchCondition : EventNode
{
    [SerializeField]
    MatchCondition condition;

    public override string PreferredGameObjectName => (condition != null ? condition.Description : "...");

    bool inited;
    public override void Reach()
    {
        ConversationWindow.Close();
        if(!inited)
        {
            condition.onConditionMet.AddListener(ProgressEvent);
            inited = true;
        }
        condition.StartCheck();
    }
}
