using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magicals : ScriptableObject
{
    [SerializeField]
    new TranslatableSentence name;
    [SerializeField]
    List<Substance> requireSubstances;
    [SerializeField]
    Sprite image;
}
