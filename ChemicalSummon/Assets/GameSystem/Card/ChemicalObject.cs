using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 化学物(元素/物质)(静态数据)
/// </summary>
public abstract class ChemicalObject : ScriptableObject
{
    public string chemicalSymbol;
    public new TranslatableSentence name = new TranslatableSentence();
}
