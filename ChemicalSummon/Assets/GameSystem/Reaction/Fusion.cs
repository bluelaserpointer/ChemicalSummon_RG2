using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 融合
/// </summary>
public class Fusion
{
    public Fusion(Reaction reaction)
    {
        Reaction = reaction;
        stats = Reaction.initialStats;
    }
    public Reaction Reaction { get; protected set; }
    public Reaction.Stats stats;

    /// <summary>
    /// 左式(消耗素材)
    /// </summary>
    public TypeAndCountList<Substance> LeftSubstances => stats.leftSubstances;
    /// <summary>
    /// 右式(生成素材)
    /// </summary>
    public TypeAndCountList<Substance> RightSubstances => stats.rightSubstances;
    /// <summary>
    /// 触媒(非消耗素材)
    /// </summary>
    public TypeAndCountList<Substance> Catalysts => stats.catalysts;
    /// <summary>
    /// 热量消耗
    /// </summary>
    public int HeatRequire => stats.heatRequire;
    /// <summary>
    /// 电量消耗
    /// </summary>
    public int ElectricRequire => stats.electricRequire;
    /// <summary>
    /// 剧烈度
    /// </summary>
    public int Vigorousness => stats.vigorousness;
    /// <summary>
    /// 电量释放
    /// </summary>
    public int Electric => stats.electric;
    /// <summary>
    /// 热量释放
    /// </summary>
    public int Heat => stats.heat;
    /// <summary>
    /// 爆炸系数
    /// </summary>
    public int ExplosionPower => stats.explosionPower;
    /// <summary>
    /// 爆炸伤害
    /// </summary>
    public int ExplosionDamage => stats.ExplosionDamage;
    public bool ApplyEnhancement(FusionEnhancer enhancer)
    {
        if (enhancer.Appliable(this))
        {
            enhancer.Apply(this);
            return true;
        }
        return false;
    }
}
