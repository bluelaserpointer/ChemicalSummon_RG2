using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ħ�������ṩ��ѧ��Ӧ��Ҫ�İ���
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
