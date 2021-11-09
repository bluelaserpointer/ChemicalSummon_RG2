using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����SO��ͷ
/// </summary>
public abstract class CardHeader : ScriptableObject
{
    public new TranslatableSentence name = new TranslatableSentence();
    public TranslatableSentence description = new TranslatableSentence();

    public abstract Card GenerateCard(int amount = 1);
    /// <summary>
    /// �����ݶ�
    /// </summary>
    public int rank;
    public GameObject abilityPrefab;
    /// <summary>
    /// �Ƿ�Ϊ���ʿ���ͷ
    /// </summary>
    public bool IsSubstance => GetType().Equals(typeof(Substance));
    /// <summary>
    /// �Ƿ�Ϊħ������ͷ
    /// </summary>
    public bool IsMagic => GetType().Equals(typeof(Magic));
}
