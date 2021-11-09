using System;
using UnityEngine;

/// <summary>
/// 战斗前卡组(静态卡组)
/// </summary>
[Serializable]
public class Deck
{
    public Deck()
    {
        for (int i = 0; i < echelons.Length; ++i)
            echelons[i] = new TypeAndCountList<CardHeader>();
    }
    public Deck(Deck sampleDeck)
    {
        for(int i = 0; i < echelons.Length; ++i)
        {
            echelons[i] = new TypeAndCountList<CardHeader>(sampleDeck.echelons[i]);
        }
    }
    public string name;
    [SerializeField]
    TypeAndCountList<CardHeader>[] echelons = new TypeAndCountList<CardHeader>[3];
    public TypeAndCountList<CardHeader>[] Echelons => echelons;
    public bool IsEmpty => CardCount == 0;
    public int CardCount {
        get
        {
            int sum = 0;
            foreach(var list in echelons)
            {
                sum += list.TotalCount();
            }
            return sum;
        }
    }
}
