using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class HandCardsDisplay : HandCardsArrange
{
    public override void Arrange()
    {
        Vector3 myPos = transform.position;
        Vector3 myAngle = transform.eulerAngles;
        float radius = arrangeRadius * transform.lossyScale.magnitude;
        int cardCount = CardCount;
        float enumerateDirection = rightIsUpper ? -1 : 1;
        float angle = -enumerateDirection * (cardCount - 1) * AngleSpanRad / 2 + Mathf.Deg2Rad * arrangeDirection;
        float rotate = -enumerateDirection * (cardCount - 1) * cardAngleSpan / 2;
        //keep removing cards layer when switching stack mode
        Queue<int> occpupiedIndexes = new Queue<int>();
        for (int index = 0; index < transform.childCount; ++index)
        {
            Transform childTf = transform.GetChild(index);
            if (cards.Find(card => card.transform.Equals(childTf)))
                occpupiedIndexes.Enqueue(index);
        }
        foreach (GameObject cardObj in cards)
        {
            SubstanceCard card = cardObj.GetComponent<SubstanceCard>();
            card.transform.SetSiblingIndex(occpupiedIndexes.Dequeue());
            card.TracePosition(myPos + transform.right * radius * Mathf.Cos(angle) + transform.up * radius * Mathf.Sin(angle));
            card.TraceRotation(Quaternion.Euler(myAngle + new Vector3(card.IsMySide ? 0 : 180, 0, card.IsMySide ? rotate : -rotate)));
            angle += enumerateDirection * AngleSpanRad;
            rotate += enumerateDirection * cardAngleSpan;
        }
    }
}
