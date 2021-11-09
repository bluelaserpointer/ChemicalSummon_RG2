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
    /// ���ڼ���˵���Ĳ�ȡ�����¼����Ҫ�����л�ʱ���»���
    /// TODO: �����ܻ�����ϵͳ����Ϊsentence�ڲ�����
    /// </summary>
    public void UpdateDescriptionLanguage()
    {
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
