using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 给予给定元素列表内的元素N次
/// </summary>
public class UnstableManaStone : Item
{
    [SerializeField]
    List<Substance> pool;
    [SerializeField]
    [Min(0)]
    int lootAmount;

    public override bool Usable => true;
    public override string Description
    {
        get
        {
            string rootsStr = "";
            pool.ForEach(substance => rootsStr += rootsStr.Length == 0 ? substance.name : "/" + substance.name);
            return base.Description.Replace("$root", rootsStr + " x" + lootAmount);
        }
    }

    public override void Use()
    {
        StackedElementList<Substance> results = new StackedElementList<Substance>();
        for (int i = 0; i < lootAmount; ++i)
        {
            Substance loot = pool.GetRandomElement();
            PlayerSave.SubstanceStorage.AddAll(results);
            results.Add(loot);
        }
        //TODO: do something with results
    }
}
