using UnityEngine;

[DisallowMultipleComponent]
public abstract class ReactionCondition : MonoBehaviour
{
    string description;
    /// <summary>
    /// 卡牌条件说明
    /// </summary>
    public string Description => description;
    protected abstract string InitDescription();
    private void OnValidate()
    {
        description = InitDescription();
    }
    /// <summary>
    /// 反应式是否符合条件。
    /// </summary>
    /// <param name="reaction">反应式</param>
    /// <returns></returns>
    public abstract bool Accept(Reaction reaction);
}
