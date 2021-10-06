using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 魔法卡，提供化学反应必要的帮助
/// </summary>
[DisallowMultipleComponent]
public class MagicCard : MonoBehaviour
{
    Magicals magicals;
    public Magicals Magicals
    {
        get => magicals;
        set {
            magicals = value;
            //change card appearence
        }
    }
}
