using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardAbility : MonoBehaviour
{
    [SerializeField]
    Sprite icon;
    [SerializeField]
    bool isActiveAbility = true;

    //data
    /// <summary>
    /// 现在挂上的卡
    /// </summary>
    public Card Card { get; protected set; }
    public Sprite Icon => icon;
    string description;
    public string Description => description;
    /// <summary>
    /// 是否为主动技能
    /// </summary>
    public bool IsActiveAbility => isActiveAbility;
    /// <summary>
    /// 生成说明文
    /// </summary>
    /// <returns></returns>
    public abstract string GetDescription();
    /// <summary>
    /// 技能说明文采取缓冲记录，需要更新缓冲
    /// TODO: 今后可能会简化这个系统，改为sentence内部缓冲
    /// </summary>
    public void SetCard(Card card)
    {
        Card = card;
        description = GetDescription();
    }
    public abstract bool IsAvaliable(Card card);
    public abstract void DoAbility(Card card);
    public static GameObject LoadFromResources(string abilityPrefabName)
    {
        return Resources.Load<GameObject>(General.ResourcePath.CardAbility + abilityPrefabName);
    }
    public static GameObject[] LoadAllFromResources()
    {
        return Resources.LoadAll<GameObject>(General.ResourcePath.CardAbility);
    }
}
