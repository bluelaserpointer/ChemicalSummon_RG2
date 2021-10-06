using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class AbilityLabel : MonoBehaviour
{
    [SerializeField]
    Text headerText;
    [SerializeField]
    Image abilityIcon;
    [SerializeField]
    Text descriptionText;
    [SerializeField]
    TranslatableSentenceSO abilitySentence;
    [SerializeField]
    Color avaliableColor, unavaliableColor;

    SubstanceCard card;
    CardAbility ability;
    bool abilityAvaliable;
    public void Set(SubstanceCard card, int abilityIndex, CardAbility ability)
    {
        this.card = card;
        this.ability = ability;
        headerText.text = abilitySentence + abilityIndex;
        abilityIcon.sprite = ability.Icon;
        descriptionText.text = ability.Description;
        UpdateAbilityState();
    }
    public void UpdateAbilityState()
    {
        GetComponent<Image>().color = (abilityAvaliable = ability.IsAvaliable(card)) ? avaliableColor : unavaliableColor;
    }
    public void OnClick()
    {
        if(abilityAvaliable && card.IsMySide)
        {
            ability.DoAbility(card);
            UpdateAbilityState();
        }
    }
}
