using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ItemScreen : MonoBehaviour
{
    [SerializeField]
    ItemButton itemButtonPrefab;
    [SerializeField]
    Transform itemParentTf;
    [SerializeField]
    ItemInfoDisplay itemInfoDisplay;

    //data
    public ItemInfoDisplay ItemInfoDisplay => itemInfoDisplay;
    public void Init()
    {
        foreach (Transform childTf in itemParentTf)
            Destroy(childTf.gameObject);
        foreach(var each in PlayerSave.ItemStorage)
        {
            ItemButton itemButton = Instantiate(itemButtonPrefab, itemParentTf);
            itemButton.SetItem(each.type, each.amount);
        }
        WorldManager.ItemScreen.ItemInfoDisplay.SetItem(null);
    }
}
