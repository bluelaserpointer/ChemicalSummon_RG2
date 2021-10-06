using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 卡槽
/// </summary>
public class CardSlot : ObjectSlot
{
    public SubstanceCard Card => IsEmpty ? null : GetTop().GetComponent<SubstanceCard>();
    public void DoAlignment(Transform childTransform, UnityAction afterAction)
    {
        childTransform.GetComponent<SubstanceCard>().TracePosition(ArrangeParent.position, () => {
            OnAlignmentEnd(childTransform);
            afterAction?.Invoke();
        });
        if (doArrangeRotation)
        {
            oldLocalRotation = childTransform.localEulerAngles;
            childTransform.GetComponent<SubstanceCard>().TraceRotation(arrangeLocalRotation);
        }
        if (doArrangeScale)
        {
            oldLocalScale = childTransform.localScale;
            childTransform.localScale = arrangeLocalScale;
        }
    }
    public override void DoAlignment(Transform childTransform)
    {
        DoAlignment(childTransform, null);
    }
    public virtual void OnAlignmentEnd(Transform childTransform) { }
    public void SlotSet(SubstanceCard card, UnityAction afterAction = null)
    {
        if(afterAction == null)
            base.SlotSet(card.gameObject);
        else
        {
            if (!AllowSlotSet(card.gameObject))
                return;
            oldParent = card.transform.parent;
            card.transform.SetParent(ArrangeParent);
            DoAlignment(card.transform, afterAction);
            onSet.Invoke();
        }
    }
}
