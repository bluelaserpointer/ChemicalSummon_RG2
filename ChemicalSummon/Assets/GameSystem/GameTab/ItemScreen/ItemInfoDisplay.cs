using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class ItemInfoDisplay : MonoBehaviour
{
    [SerializeField]
    Text itemName, itemDescription;
    [SerializeField]
    TranslatableSentenceSO clickAnyItemGuide;

    //data
    ItemButton itemButton;
    public Item Item => itemButton.Item;
    public void SetItem(ItemButton itemButton)
    {
        if(itemButton == null)
        {
            this.itemButton = null;
            itemName.text = "";
            itemDescription.text = clickAnyItemGuide;
            return;
        }
        this.itemButton = itemButton;
        itemName.text = Item.Name;
        itemDescription.text = Item.Description;
    }
    public void OnUseButtonClick()
    {
        if (itemButton != null)
            itemButton.OnUse();
    }
}
