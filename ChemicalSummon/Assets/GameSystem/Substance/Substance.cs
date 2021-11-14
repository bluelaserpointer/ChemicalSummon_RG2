using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// 物体三态
/// </summary>
public enum ThreeState { Solid, Liquid, Gas }
/// <summary>
/// 物质(静态数据)
/// </summary>
[CreateAssetMenu(fileName = "NewSubstance", menuName = "Chemical/Substance")]
public class Substance : CardHeader
{
    /// <summary>
    /// 化学式（或通用英文名称）
    /// </summary>
    public string formula;
    //inspector
    /// <summary>
    /// 组成元素
    /// </summary>
    public TypeAndCountList<Element> elements = new TypeAndCountList<Element>();
    /// <summary>
    /// 三态卡牌图片
    /// </summary>
    public Sprite image;
    public int atk;
    [Min(-273.15f)]
    public float meltingPoint;
    [Min(-273.15f)]
    public float boilingPoint;
    public bool isPhenomenon;
    public bool isWhat;
    public bool waterSoluble;
    public bool isOre;
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
    /// 是否由单元素组成(能成为卡组牌的条件)
    /// </summary>
    public bool IsPureElement => elements.TypeCount() == 1;
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
                return General.LoadSentence("Gas");
            case ThreeState.Solid:
                return General.LoadSentence("Solid");
            case ThreeState.Liquid:
                return General.LoadSentence("Liquid");
        }
        return "InvalidState";
    }
    /// <summary>
    /// Like CO and Co, some substances' formulas coflict in different case. This function put markings helping Unity distingush them in asset file form.
    /// </summary>
    /// <param name="formulaStr"></param>
    /// <returns></returns>
    public static string RemoveFormulaCaseConflict(string formulaStr)
    {
        formulaStr = Regex.Replace(formulaStr, "Co(?=[+=]|$)", "Co_");
        return formulaStr;
    }
    public static Substance LoadFromResources(string name)
    {
        Substance substance = Resources.Load<Substance>(General.ResourcePath.Substance + RemoveFormulaCaseConflict(name));
        return substance;
    }
    public static Substance GetByNameWithWarn(string name)
    {
        Substance substance = LoadFromResources(name);
        if (substance == null)
            Debug.LogWarning("Cannot find substance: " + name);
        return substance;
    }
    public static List<Substance> GetAll()
    {
        return new List<Substance>(Resources.LoadAll<Substance>(General.ResourcePath.Substance));
    }
    /// <summary>
    /// 生成物质卡
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public SubstanceCard GenerateSubstanceCard(int amount = 1)
    {
        SubstanceCard card = Instantiate(General.Instance.substanceCardPrefab);
        card.Substance = this;
        card.InitCardAmount(amount);
        card.location = CardTransport.Location.OffSite;
        return card;
    }
    public override Card GenerateCard(int amount = 1)
    {
        return GenerateSubstanceCard(amount);
    }
}
