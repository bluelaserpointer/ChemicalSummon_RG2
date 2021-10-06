using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 物体三态
/// </summary>
public enum ThreeState { Solid, Liquid, Gas }
/// <summary>
/// 物质(静态数据)
/// </summary>
[CreateAssetMenu(fileName = "NewSubstance", menuName = "Chemical/Substance")]
public class Substance : ChemicalObject
{
    //inspector
    /// <summary>
    /// 组成元素
    /// </summary>
    public StackedElementList<Element> elements = new StackedElementList<Element>();
    /// <summary>
    /// 三态卡牌图片
    /// </summary>
    public Sprite image;
    /// <summary>
    /// 梯队
    /// </summary>
    public int echelon;
    public int atk;
    [Min(-273.15f)]
    public float meltingPoint;
    [Min(-273.15f)]
    public float boilingPoint;
    public bool isPhenomenon;
    public bool waterSoluble;
    public bool isOre;
    public CardAbility[] abilities = new CardAbility[0];
    public TranslatableSentence description = new TranslatableSentence();
    public List<ResearchStep> researchSteps = new List<ResearchStep>();

    //data
    public Sprite Image => image;
    /// <summary>
    /// 基础攻击力
    /// </summary>
    public int ATK => atk;
    /// <summary>
    /// 融点
    /// </summary>
    public float MeltingPoint => meltingPoint;
    /// <summary>
    /// 沸点
    /// </summary>
    public float BoilingPoint => boilingPoint;
    /// <summary>
    /// 计算Mol量
    /// </summary>
    /// <returns></returns>
    public int GetMol()
    {
        int mol = 0;
        foreach (var elementAndAmount in elements)
        {
            mol += elementAndAmount.type.mol;
        }
        return mol;
    }
    /// <summary>
    /// 说明
    /// </summary>
    public TranslatableSentence Description => description;
    /// <summary>
    /// 是否由单元素组成(能成为卡组牌的条件)
    /// </summary>
    public bool IsPureElement => elements.CountType() == 1;
    /// <summary>
    /// 水溶
    /// </summary>
    public bool WaterSoluble => waterSoluble;
    /// <summary>
    /// 是否为矿物主成分(魔法阵"镐子"的效果对象)
    /// </summary>
    public bool IsOre => isOre;
    public ThreeState GetStateInTempreture(float tempreture)
    {
        if (tempreture < MeltingPoint)
        {
            return ThreeState.Solid;
        }
        else if (tempreture < BoilingPoint)
        {
            return ThreeState.Liquid;
        }
        return ThreeState.Gas;
    }
    public static string ThreeStateToString(ThreeState state)
    {
        switch(state)
        {
            case ThreeState.Gas:
                return ChemicalSummonManager.LoadSentence("Gas");
            case ThreeState.Solid:
                return ChemicalSummonManager.LoadSentence("Solid");
            case ThreeState.Liquid:
                return ChemicalSummonManager.LoadSentence("Liquid");
        }
        return "InvalidState";
    }
    public static Substance GetByName(string name)
    {
        Substance substance = Resources.Load<Substance>("Chemical/Substance/" + name);
        if(substance == null || !substance.chemicalSymbol.Equals(name))
        {
            substance = Resources.Load<Substance>("Chemical/Substance/AvoidCaseConflict/" + name);
        }
        return substance;
    }
    public static Substance GetByNameWithWarn(string name)
    {
        Substance substance = GetByName(name);
        if (substance == null)
            Debug.LogWarning("Cannot find substance: " + name);
        return substance;
    }
    public static List<Substance> GetAll()
    {
        List<Substance> list = new List<Substance>(Resources.LoadAll<Substance>("Chemical/Substance"));
        foreach (var each in Resources.LoadAll<Substance>("Chemical/Substance/AvoidCaseConflict"))
        {
            list.Add(each);
        }
        return list;
    }
}
