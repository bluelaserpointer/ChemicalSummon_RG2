using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class ReactionListDisplay : MonoBehaviour
{
    [SerializeField]
    InputField searchInputField;
    [SerializeField]
    Transform buttonListTransform;

    List<FusionButton> originalButtons;

    public void OnSearchFieldChanged()
    {
        if (searchInputField.isFocused && ChemicalSummonManager.CurrentSceneIsWorld)
            WorldManager.Player.IsDoingInput = true;
        string searchStr = searchInputField.text;
        if (searchStr.Length == 0)
            originalButtons.ForEach(button => button.gameObject.SetActive(true));
        else
        {
            foreach(FusionButton button in originalButtons)
            {
                button.gameObject.SetActive(button.Reaction.description.IndexOf(searchStr, System.StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }
    }
    public void DoneSearhFieldEdit()
    {
        if(ChemicalSummonManager.CurrentSceneIsWorld)
            WorldManager.Player.IsDoingInput = false;
    }
    public void InitList(List<FusionButton> buttons)
    {
        originalButtons = buttons;
        buttonListTransform.DestroyAllChildren();
        originalButtons.ForEach(button => button.transform.SetParent(buttonListTransform));
        searchInputField.text = "";
    }
    public void AddButtonAction(UnityAction<FusionButton> buttonAction)
    {
        foreach (Transform buttonTf in buttonListTransform)
        {
            FusionButton fusionButton = buttonTf.GetComponent<FusionButton>();
            fusionButton.Button.onClick.AddListener(() => buttonAction.Invoke(fusionButton));
        }
    }
}
