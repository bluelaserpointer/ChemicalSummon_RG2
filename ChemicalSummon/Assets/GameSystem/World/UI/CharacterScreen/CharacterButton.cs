using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class CharacterButton : MonoBehaviour
{
    //inspector
    [SerializeField]
    Image portrait;
    [SerializeField]
    Text nameText;
    [SerializeField]
    Image selectIndicator;
    [SerializeField]
    Button selectButton;

    //data
    Character character;
    public Character Character => character;
    public Button SelectButton => selectButton;

    public void SetCharacter(Character character)
    {
        this.character = character;
        UpdateUI();
    }
    public void UpdateUI()
    {
        portrait.sprite = character.FaceIcon;
        nameText.text = character.Name;
        selectIndicator.gameObject.SetActive(PlayerSave.SelectedCharacter.Equals(character));
    }
}
