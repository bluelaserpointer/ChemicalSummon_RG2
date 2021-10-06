using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class DrawPileDisplay : CardSlot
{
    [SerializeField]
    Text amountText;
    [SerializeField]
    Shadow shadow;
    [SerializeField]
    Transform cardSpawnPosition;

    public Transform CardSpawnPosition => cardSpawnPosition;

    private void Start()
    {
        UpdateAmountText();
        onSet.AddListener(UpdateAmountText);
        onClear.AddListener(UpdateAmountText);
    }
    public void UpdateAmountText()
    {
        int count = transform.childCount;
        amountText.text = count.ToString();
        shadow.enabled = count > 0;
    }
    public void SlotClear(SubstanceCard card)
    {
        base.SlotClear(card.transform);
    }

    public override void OnAlignmentEnd(Transform childTransform)
    {
    }
}
