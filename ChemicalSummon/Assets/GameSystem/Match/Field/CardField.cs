using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 卡牌场地，放置卡牌进行战斗
/// </summary>
[DisallowMultipleComponent]
public class CardField : Field
{
    [SerializeField]
    List<ShieldCardSlot> shieldCardSlots;
    /// <summary>
    /// 格挡区卡槽
    /// </summary>
    public new List<ShieldCardSlot> Slots => shieldCardSlots;
}
