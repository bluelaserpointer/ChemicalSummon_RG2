using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class ReactionSpecialDamageLabel : MonoBehaviour
{
    [SerializeField]
    Image icon;
    [SerializeField]
    Text descriptionText;

    [SerializeField]
    Sprite explosionIcon, heatIcon, electricIcon;
    [SerializeField]
    TranslatableSentenceSO explosionDescription, heatDescription, electricDescription;

    public void SetReactionDamageType(DamageType damageType)
    {
        switch(damageType)
        {
            case DamageType.Explosion:
                icon.sprite = explosionIcon;
                descriptionText.text = explosionDescription;
                break;
            case DamageType.Heat:
                icon.sprite = heatIcon;
                descriptionText.text = heatDescription;
                break;
            case DamageType.Electronic:
                icon.sprite = electricIcon;
                descriptionText.text = electricDescription;
                break;
        }
    }
}
