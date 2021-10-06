using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public abstract class MatchCondition : MonoBehaviour
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
    public abstract void StartCheck();
    [HideInInspector]
    public readonly UnityEvent onConditionMet = new UnityEvent();
}
