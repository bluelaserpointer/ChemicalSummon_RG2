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
            echelons[i] = new StackedElementList<Substance>();
    }
    public Deck(Deck sampleDeck)
    {
        for(int i = 0; i < echelons.Length; ++i)
        {
            echelons[i] = new StackedElementList<Substance>(sampleDeck.echelons[i]);
        }
    }
    public string name;
    [SerializeField]
    StackedElementList<Substance>[] echelons = new StackedElementList<Substance>[3];
    public StackedElementList<Substance>[] Echelons => echelons;
    public bool IsEmpty => CardCount == 0;
    public int CardCount {
        get
        {
            int sum = 0;
            foreach(var list in echelons)
            {
                sum += list.CountStack();
            }
            return sum;
        }
    }
}
