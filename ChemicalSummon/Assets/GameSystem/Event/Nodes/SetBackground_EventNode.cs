using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class SetBackground_EventNode : EventNode
{
    //inspector
    [SerializeField]
    Sprite backgroundSprite;

    //data
    public override string PreferredGameObjectName => "[背景图变化]" + (backgroundSprite == null ? "(?missing?)" : backgroundSprite.name);
    public override void OnDataEdit()
    {
        if(backgroundSprite != null)
        {
            GetComponent<Image>().sprite = backgroundSprite;
            GetComponentInChildren<Text>().text = "[背景图变化]" + backgroundSprite.name;
        }
        HideDescriptionText(true);
    }

    public override void Reach()
    {
        ConversationWindow.SetBackground(backgroundSprite);
        PlayerSave.ProgressActiveEvent();
    }
}
