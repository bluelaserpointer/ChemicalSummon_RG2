using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSubstance", menuName = "Chemical/MagicCard")]
public class Magic : CardHeader
{
    public Sprite image;

    /// <summary>
    /// Éú³ÉÄ§·¨¿¨
    /// </summary>
    /// <param name="magic"></param>
    /// <returns></returns>
    public MagicCard GenerateMagicCard(int amount = 1)
    {
        MagicCard card = Instantiate(General.Instance.magicCardPrefab);
        card.Magic = this;
        card.CardAmount = amount;
        card.location = CardTransport.Location.OffSite;
        return card;
    }
    public override Card GenerateCard(int amount = 1)
    {
        return GenerateMagicCard(amount);
    }
    public static Magic LoadFromResources(string magicName)
    {
        return Resources.Load<Magic>(General.ResourcePath.Magic + magicName);
    }
}
