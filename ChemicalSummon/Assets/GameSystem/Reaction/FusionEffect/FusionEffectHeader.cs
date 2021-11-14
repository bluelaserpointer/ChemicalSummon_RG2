using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 融合特殊效果标头
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
    /// 特殊效果名称
    /// </summary>
    public string EffectName => effectName;
    /// <summary>
    /// 代表色
    /// </summary>
    public Color MainColor => mainColor;
    /// <summary>
    /// 图标
    /// </summary>
    public GameObject IconObject => iconObject;
    static FusionEffectHeader heatResources;
    public static FusionEffectHeader HeatResources => heatResources ?? (heatResources = Resources.Load<FusionEffectHeader>("FusionEffectIconResources/FusionHeat"));
}
