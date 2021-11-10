using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
    [SerializeField]
    ToggleGroup searchModeToggleGroup;
    [SerializeField]
    Toggle leftMode, catalystMode, rightMode, allMode;

    List<FusionButton> originalButtons = new List<FusionButton>();

    public void OnInputSearchField()
    {
        if (searchInputField.isFocused && General.CurrentSceneIsWorld)
            WorldManager.Player.LockMovement = true;
        UpdateList();
    }
    public void OnSearchModeToggled(bool value)
    {
        if (value)
            UpdateList();
    }
    public void UpdateList()
    {
        string searchStr = searchInputField.text;
        if (searchStr.Length == 0)
        {
            originalButtons.ForEach(button => button.gameObject.SetActive(true));
            return;
        }
        //because reaction formula does not have fixed ordering between items, we need search each of them
        string[] searchNames = searchStr.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
        if(searchNames.Length == 0)
        {
            originalButtons.ForEach(button => button.gameObject.SetActive(true));
            return;
        }
        //search mode
        Func<string, string> searchPlaceConverter;
        Toggle activeToggle = searchModeToggleGroup.GetFirstActiveToggle();
        if (activeToggle.Equals(allMode))
            searchPlaceConverter = (str) => str;
        else if(activeToggle.Equals(leftMode))
            searchPlaceConverter = (str) => str.Split('=')[0];
        else if (activeToggle.Equals(catalystMode))
            searchPlaceConverter = (str) => str.Split('=')[1];
        else if (activeToggle.Equals(rightMode))
            searchPlaceConverter = (str) => str.Split('=')[2];
        else
            return;
        foreach (FusionButton button in originalButtons)
        {
            string searchPlace = searchPlaceConverter.Invoke(button.Reaction.formula);
            bool hit = true;
            foreach (string searchName in searchNames)
            {
                int hitIndex = searchPlace.IndexOf(searchName, StringComparison.OrdinalIgnoreCase);
                if (hitIndex == -1)
                {
                    hit = false;
                    break;
                }
                //search hole word (exp. don't search "Fe2O3" by "Fe")
                int nextLetterIndex = hitIndex + searchName.Length;
                if(searchPlace.Length > nextLetterIndex)
                {
                    char nextLetter = searchPlace[nextLetterIndex];
                    if(!nextLetter.Equals('+') && !nextLetter.Equals('='))
                    {
                        hit = false;
                        break;
                    }
                }
            }
            button.gameObject.SetActive(hit);
        }
    }
    public void DoneSearhFieldEdit()
    {
        if(General.CurrentSceneIsWorld)
            WorldManager.Player.LockMovement = false;
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
