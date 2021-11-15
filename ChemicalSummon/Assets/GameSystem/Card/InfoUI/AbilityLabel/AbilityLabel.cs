using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��ʾ����������UI
/// </summary>
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
    Button invokeButton;

    Card card;
    CardAbility ability;
    /// <summary>
    /// ��������������յ�ǰ��������
    /// </summary>
    /// <param name="abilityIndex"></param>
    /// <param name="ability"></param>
    public void Init(int abilityIndex, CardAbility ability)
    {
        card = null;
        invokeButton.enabled = false;
        this.ability = ability;
        headerText.text = abilitySentence + abilityIndex;
        abilityIcon.sprite = ability.Icon;
        descriptionText.text = ability.Description;
    }
    /// <summary>
    /// ���ÿ��ƣ�����֮�������ʾ����ʹ�ð�ť
    /// </summary>
    /// <param name="card"></param>
    public void SetCard(Card card)
    {
        this.card = card;
        invokeButton.enabled = true;
        UpdateAbilityState();
    }
    public void UpdateAbilityState()
    {
        invokeButton.interactable = card.IsMySide && ability.IsAvaliable(card);
    }
    public void InvokeAbility()
    {
        ability.DoAbility(card);
        UpdateAbilityState();
    }
}
