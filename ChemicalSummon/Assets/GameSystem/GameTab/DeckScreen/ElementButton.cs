using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 元素按钮, 点击可添加对应的单元素物质至当前卡组内
/// </summary>
[DisallowMultipleComponent]
public class ElementButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    Substance substance;
    [SerializeField]
    Image elementImage;
    [SerializeField]
    Text elementText;
    [SerializeField]
    Image amountTextArea;
    [SerializeField]
    Text amountText;
    [SerializeField]
    Color noAmountColor, hasAmountColor, maxAmountColor;

    //data
    int deckCardCount;
    int storageCardCount;

    public void Init()
    {
        deckCardCount = 0;// PlayerSave.ActiveDeck.CountCard(substance);
        storageCardCount = PlayerSave.SubstanceStorage.CountStack(substance);
        UpdateUI();
    }
    public void UpdateUI()
    {
        if (storageCardCount == 0)
            amountTextArea.color = noAmountColor;
        else if (deckCardCount < storageCardCount)
            amountTextArea.color = hasAmountColor;
        else
            amountTextArea.color = maxAmountColor;
        amountText.text = deckCardCount + "/" + storageCardCount;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (substance == null)
            return;
        //add / remove
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (deckCardCount >= storageCardCount)
                return;
            //PlayerSave.ActiveDeck.Add(substance);
            ++deckCardCount;
            UpdateUI();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            //if(PlayerSave.ActiveDeck.Remove(substance))
                //--deckCardCount;
            UpdateUI();
        }
    }
    private void OnValidate()
    {
        if (transform.parent == null || transform.parent.GetComponent<GridLayoutGroup>() == null)
            return;
        if (substance != null)
        {
            elementImage.sprite = substance.Image;
            elementText.text = substance.chemicalSymbol;
        }
        else
        {
            elementImage.sprite = null;
            elementText.text = "";
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (substance == null)
            return;
        //info set
        //WorldManager.DeckScreen.SetCardInfo(substance);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (substance == null)
            return;
        //info set
        //WorldManager.DeckScreen.SetCardInfo(null);
    }
}
