using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class ItemButton : MonoBehaviour
{
    [SerializeField]
    Image icon;
    [SerializeField]
    Text amountText;

    //data
    Item item;
    public Item Item => item;
    int itemAmount;
    private void Start()
    {
        UpdateUI();
    }
    public void SetItem(Item item, int amount)
    {
        this.item = item;
        icon.sprite = item.Icon;
        itemAmount = amount;
    }
    public void UpdateUI()
    {
        amountText.text = itemAmount.ToString();
    }
    public void OnClick()
    {
        WorldManager.ItemScreen.ItemInfoDisplay.SetItem(this);
    }
    public void OnUse()
    {
        if (itemAmount > 0)
        {
            PlayerSave.ItemStorage.Remove(item);
            item.Use();
            if (--itemAmount <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                UpdateUI();
            }
        }
    }
}
