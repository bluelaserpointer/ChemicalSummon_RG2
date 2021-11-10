using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ħ�������ṩ����Ч��
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
            descriptionText.text = magic.description;
            if (magic.abilityPrefab == null)
                abilities = new CardAbility[0];
            else
            {
                GameObject abilitiesObject = Instantiate(magic.abilityPrefab, transform);
                abilitiesObject.name = "Abilities";
                abilities = abilitiesObject.GetComponentsInChildren<CardAbility>();
            }
        }
    }
    public override CardHeader Header => Magic;
    public override string CurrentLanguageName => Magic.name;
    public override Sprite Image => Magic.image;
    static MagicCard templatePrefab;
    public static MagicCard TemplatePrefab => templatePrefab ?? (templatePrefab = Resources.Load<MagicCard>("CardPrefab/MagicCard")); //Assets/GameSystem/Card/Magic/Resources/SubstanceCard
}