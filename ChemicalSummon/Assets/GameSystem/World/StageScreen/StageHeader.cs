using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StageHeader")]
public class StageHeader : ScriptableObject
{
    public string id;
    public int difficulty;
    public new TranslatableSentence name = new TranslatableSentence();
    public Sprite image;
    public TranslatableSentence description = new TranslatableSentence();
    public Event stageEvent;

    public static StageHeader GetByName(string stageHeaderName)
    {
        return Resources.Load<StageHeader>("StageHeader/" + stageHeaderName);
    }
}
