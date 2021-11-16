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
    Button clearInputFieldButton;
    [SerializeField]
    Transform buttonListTransform;
    [SerializeField]
    ToggleGroup searchModeToggleGroup;
    [SerializeField]
    Toggle leftMode, catalystMode, rightMode, allMode;

    List<FusionButton> fusionButtons = new List<FusionButton>();
    public List<FusionButton> FusionButtons => fusionButtons;

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
            fusionButtons.ForEach(button => button.gameObject.SetActive(true));
            clearInputFieldButton.gameObject.SetActive(false);
            return;
        }
        clearInputFieldButton.gameObject.SetActive(true);
        //because reaction formula does not have fixed ordering between items, we need search each of them
        string[] searchNames = searchStr.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
        if(searchNames.Length == 0)
        {
            fusionButtons.ForEach(button => button.gameObject.SetActive(true));
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
        foreach (FusionButton button in fusionButtons)
        {
            string searchPlace = searchPlaceConverter.Invoke(button.Reaction.formula);
            bool hit = true;
            foreach (string searchName in searchNames)
            {
                //search hole word (exp. don't match "Fe2O3" by "Fe"). Regex expression sample in "Fe": (^|[+=])[0-9]*Fe($|[+=]);
                System.Text.RegularExpressions.Match match = Regex.Match(searchPlace, "(^|[+=])[0-9]*" + searchName + "($|[+=])", RegexOptions.IgnoreCase);
                if (!match.Success)
                {
                    hit = false;
                    break;
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
        fusionButtons = buttons;
        buttonListTransform.DestroyAllChildren();
        fusionButtons.ForEach(button => button.transform.SetParent(buttonListTransform));
        ClearSearchInputField();
    }
    public void ClearSearchInputField()
    {
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
    public bool TrySetCard(Card card)
    {
        SubstanceCard scard = card as SubstanceCard;
        if (scard != null)
        {
            AddSearchSubstance(scard.Substance);
            return true;
        }
        MagicCard mcard = card as MagicCard;
        if (mcard != null)
        {
            return TrySetMagicCard(mcard);
        }
        return false;
    }
    /// <summary>
    /// 增加搜索条件(同一张卡牌限定张数)
    /// </summary>
    /// <param name="substance"></param>
    public void AddSearchSubstance(Substance substance)
    {
        string formula = substance.formula;
        if (searchInputField.text.Length == 0)
        {
            searchInputField.text = formula;
            return;
        }
        //Find the amount number for this substance. Regular expression sample for "Fe": (?<=^|[+=])[0-9]*(?=Fe($|[+=]))
        System.Text.RegularExpressions.Match numStrMatch = Regex.Match(searchInputField.text, "(?<=^|[+=])[0-9]*(?=" + formula + "($|[+=]))");
        if(numStrMatch.Success)
        {
            string numStr = numStrMatch.Value;
            if(numStr.Length == 0)
            {
                searchInputField.text = searchInputField.text.Insert(numStrMatch.Index, "2");
            }
            else
            {
                searchInputField.text = searchInputField.text.Remove(numStrMatch.Index, numStr.Length);
                searchInputField.text = searchInputField.text.Insert(numStrMatch.Index, (int.Parse(numStr) + 1).ToString());
            }
        }
        else
            searchInputField.text += "+" + substance.formula;
    }
    /// <summary>
    /// 设置魔法卡强化（仅战斗内使用）
    /// </summary>
    /// <param name="magicCard"></param>
    /// <returns></returns>
    public bool TrySetMagicCard(MagicCard magicCard)
    {
        FusionEnhancer enhancer = null;
        foreach (CardAbility ability in magicCard.abilities)
        {
            if ((enhancer = ability as FusionEnhancer) != null)
                break;
        }
        if (enhancer == null)
            return false;
        if (!MatchManager.EnhancementList.AddAbility(magicCard, enhancer))
            return false;
        return true;
    }

}
