using UnityEngine;

/// <summary>
/// 物品(初始物品/怪物掉落/任务要求/任务报酬 etc.)
/// </summary>
[CreateAssetMenu(menuName = "Item")]
public class ItemHeader : ScriptableObject
{
    public new TranslatableSentence name = new TranslatableSentence();
    public TranslatableSentence description = new TranslatableSentence();

    public static ItemHeader GetByName(string itemHeaderName)
    {
        return Resources.Load<ItemHeader>("ItemHeader/" + itemHeaderName);
    }
}
