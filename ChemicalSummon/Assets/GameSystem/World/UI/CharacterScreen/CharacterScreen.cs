using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class CharacterScreen : MonoBehaviour
{
    [SerializeField]
    CharacterButton characterButtonSample;
    [SerializeField]
    Transform characterListTransform;
    //data
    // Start is called before the first frame update
    void Start()
    {
        foreach (Character character in PlayerSave.AllCharacters)
        {
            CharacterButton characterPanel = Instantiate(characterButtonSample, characterListTransform);
            characterPanel.SetCharacter(character);
            characterPanel.SelectButton.onClick.AddListener(() => { PlayerSave.SelectedCharacter = character; UpdateUI(); });
        }
    }

    // Update is called once per frame
    void UpdateUI()
    {
        foreach (Transform characterPanelTf in characterListTransform)
        {
            CharacterButton panel = characterPanelTf.GetComponent<CharacterButton>();
            if (panel != null)
                panel.UpdateUI();
        }
    }
}
