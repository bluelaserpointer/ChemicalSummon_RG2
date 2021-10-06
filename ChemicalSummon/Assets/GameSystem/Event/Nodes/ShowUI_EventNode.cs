using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ShowUI_EventNode : EventNode
{
    [SerializeField]
    GameObject ui;
    public override string PreferredGameObjectName => "Show an UI";

    public override void Reach()
    {
        ConversationWindow.Close();
        Instantiate(ui, ChemicalSummonManager.MainCanvas.transform).SetActive(true);
    }
    public void Progress()
    {
        ProgressEvent();
    }
}
