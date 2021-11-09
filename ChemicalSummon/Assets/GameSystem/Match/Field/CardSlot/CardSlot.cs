using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 卡槽
/// </summary>
public class CardSlot : ObjectSlot
{
    public Card TopCard => IsEmpty ? null : GetTop().GetComponent<Card>();
    public void DoAlignment(Transform childTransform, UnityAction afterAction = null)
    {
        childTransform.GetComponent<Card>().TracePosition(ArrangeParent.position, () => {
            afterAction?.Invoke();
        });
        if (doArrangeRotation)
        {
            oldLocalRotation = childTransform.localEulerAngles;
            childTransform.GetComponent<Card>().TraceRotation(arrangeLocalRotation);
        }
        if (doArrangeScale)
        {
            oldLocalScale = childTransform.localScale;
            childTransform.localScale = arrangeLocalScale;
        }
    }
    public void SlotSet(Card card, UnityAction afterAction = null)
    {
        if (!AllowSlotSet(card.gameObject))
            return;
        oldParent = card.transform.parent;
        card.transform.SetParent(ArrangeParent);
        DoAlignment(card.transform, afterAction);
        onSet.Invoke();
    }
}
