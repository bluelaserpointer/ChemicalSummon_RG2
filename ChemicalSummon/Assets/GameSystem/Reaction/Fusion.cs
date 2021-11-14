using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ں�
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
    /// ��ʽ(�����ز�)
    /// </summary>
    public TypeAndCountList<Substance> LeftSubstances => stats.leftSubstances;
    /// <summary>
    /// ��ʽ(�����ز�)
    /// </summary>
    public TypeAndCountList<Substance> RightSubstances => stats.rightSubstances;
    /// <summary>
    /// ��ý(�������ز�)
    /// </summary>
    public TypeAndCountList<Substance> Catalysts => stats.catalysts;
    /// <summary>
    /// ��������
    /// </summary>
    public int HeatRequire => stats.heatRequire;
    /// <summary>
    /// ��������
    /// </summary>
    public int ElectricRequire => stats.electricRequire;
    /// <summary>
    /// ���Ҷ�
    /// </summary>
    public int Vigorousness => stats.vigorousness;
    /// <summary>
    /// �����ͷ�
    /// </summary>
    public int Electric => stats.electric;
    /// <summary>
    /// �����ͷ�
    /// </summary>
    public int Heat => stats.heat;
    /// <summary>
    /// ��ըϵ��
    /// </summary>
    public int ExplosionPower => stats.explosionPower;
    /// <summary>
    /// ��ը�˺�
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
