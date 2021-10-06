using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class CardStorageScreen : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    CardInfoDisplay cardInfoDisplay;

    [SerializeField]
    Transform cardListTransform;
    [SerializeField]
    float cardScale = 2.0F/3.0F;

    public void Init()
    {
        cardInfoDisplay.gameObject.SetActive(false);
        cardListTransform.DestroyAllChildren();
        foreach(var subtanceStack in PlayerSave.SubstanceStorage)
        {
            GameObject anchorObject = new GameObject(subtanceStack.type.chemicalSymbol + " anchor", typeof(RectTransform));
            anchorObject.transform.SetParent(cardListTransform); //anchor protects card scale from grid layout arranging
            SubstanceCard card = SubstanceCard.GenerateSubstanceCard(subtanceStack.type, subtanceStack.amount);
            card.transform.SetParent(anchorObject.transform);
            card.SetDraggable(false);
            card.transform.localScale = cardScale * Vector3.one;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        foreach (RaycastResult rayResult in results)
        {
            GameObject obj = rayResult.gameObject;
            //if it is CardInfoDisplay
            if (obj.GetComponent<CardInfoDisplay>() != null)
                return; //keep info display shown
            //if it is card
            SubstanceCard card = obj.GetComponent<SubstanceCard>();
            if (card != null)
            {
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    cardInfoDisplay.gameObject.SetActive(true);
                    cardInfoDisplay.SetSubstance(card.Substance);
                }
                return;
            }
            cardInfoDisplay.gameObject.SetActive(false);
        }
    }
}