using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 魔法卡，提供各种效果
/// </summary>
public class MagicCard : Card
{
    //inspector
    [SerializeField]
    Text cardNameText, descriptionText;

    //data
    Magic magic;
    public Magic Magic
    {
        get => magic;
        set
        {
            magic = value;
            cardNameText.text = magic.name;
            cardImage.sprite = magic.image;
            abilities.Clear();
            abilities.AddRange(Magic.AttachAbility(this));
            string cardDescription = "";
            foreach (var ability in abilities)
            {
                if (cardDescription.Length == 0)
                    cardDescription = ability.Description;
                else
                    cardDescription += "\r\n" + ability.Description;
            }
            descriptionText.text = cardDescription;
        }
    }
    public override CardHeader Header => Magic;
    public override string CurrentLanguageName => Magic.name;
    public override Sprite Image => Magic.image;
    public FusionEnhancer TryGetFusionEnhancer
    {
        get
        {
            FusionEnhancer enhancer = null;
            foreach (CardAbility ability in abilities)
            {
                enhancer = ability as FusionEnhancer;
                if (enhancer != null)
                    break;
            }
            return enhancer;
        }
    }
}