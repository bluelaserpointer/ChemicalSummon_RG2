using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class CardCondition : MonoBehaviour
{
    string description;
    /// <summary>
    /// 卡牌条件说明
    /// </summary>
    public string Description => description;
    protected abstract string InitDescription();
    private void OnValidate()
    {
        description = InitDescription();
    }
    /// <summary>
    /// 卡牌是否符合条件。
    /// </summary>
    /// <param name="slot">所在卡槽</param>
    /// <param name="substance">物质</param>
    /// <returns></returns>
    public abstract bool Accept(SubstanceCard card);
}
