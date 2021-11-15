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
    /// ���ڹ��ϵĿ�
    /// </summary>
    public Card Card { get; protected set; }
    public Sprite Icon => icon;
    string description;
    public string Description => description;
    /// <summary>
    /// �Ƿ�Ϊ��������
    /// </summary>
    public bool IsActiveAbility => isActiveAbility;
    /// <summary>
    /// ����˵����
    /// </summary>
    /// <returns></returns>
    public abstract string GetDescription();
    /// <summary>
    /// ����˵���Ĳ�ȡ�����¼����Ҫ���»���
    /// TODO: �����ܻ�����ϵͳ����Ϊsentence�ڲ�����
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
