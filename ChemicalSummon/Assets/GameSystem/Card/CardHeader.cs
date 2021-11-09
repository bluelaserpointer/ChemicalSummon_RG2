using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 卡牌SO标头
/// </summary>
public abstract class CardHeader : ScriptableObject
{
    public new TranslatableSentence name = new TranslatableSentence();
    public TranslatableSentence description = new TranslatableSentence();

    public abstract Card GenerateCard(int amount = 1);
    /// <summary>
    /// 卡牌梯队
    /// </summary>
    public int rank;
    public GameObject abilityPrefab;
    /// <summary>
    /// 是否为物质卡标头
    /// </summary>
    public bool IsSubstance => GetType().Equals(typeof(Substance));
    /// <summary>
    /// 是否为魔法卡标头
    /// </summary>
    public bool IsMagic => GetType().Equals(typeof(Magic));
}
