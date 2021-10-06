using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 元素(静态数据)
/// </summary>
[CreateAssetMenu(fileName = "NewElement", menuName = "Chemical/Element")]
public class Element : ChemicalObject
{
    /// <summary>
    /// 原子数
    /// </summary>
    public int atom;
    /// <summary>
    /// 原子量
    /// </summary>
    public int mol;
    /// <summary>
    /// 说明
    /// </summary>
    public TranslatableSentence description = new TranslatableSentence();
    public static Element GetByName(string name)
    {
        return Resources.Load<Element>("Chemical/Element/" + name);
    }
    public static Element GetByNameWithWarn(string name)
    {
        Element element = GetByName(name);
        if (element == null)
            Debug.LogWarning("Cannot find element: " + name);
        return element;
    }
    public static List<Element> GetAll()
    {
        return new List<Element>(Resources.LoadAll<Element>("Chemical/Element"));
    }
}
