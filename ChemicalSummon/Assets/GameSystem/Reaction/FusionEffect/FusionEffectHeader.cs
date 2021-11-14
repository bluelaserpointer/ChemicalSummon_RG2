using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ں�����Ч����ͷ
/// </summary>
[CreateAssetMenu(menuName = "Fusion/FusionEffectHeader")]
public class FusionEffectHeader : ScriptableObject
{
    //inspector
    [SerializeField]
    TranslatableSentenceSO effectName;
    [SerializeField]
    Color mainColor;
    [SerializeField]
    GameObject iconObject;

    //data
    /// <summary>
    /// ����Ч������
    /// </summary>
    public string EffectName => effectName;
    /// <summary>
    /// ����ɫ
    /// </summary>
    public Color MainColor => mainColor;
    /// <summary>
    /// ͼ��
    /// </summary>
    public GameObject IconObject => iconObject;
    static FusionEffectHeader heatResources;
    public static FusionEffectHeader HeatResources => heatResources ?? (heatResources = Resources.Load<FusionEffectHeader>("FusionEffectIconResources/FusionHeat"));
}
