using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class CardStorageScreen : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    CardPreview cardPreview;

    [SerializeField]
    Transform cardListTransform;
    [SerializeField]
    float cardScale = 2.0F/3.0F;

    public void Init()
    {
        cardPreview.gameObject.SetActive(false);
        cardListTransform.DestroyAllChildren();
        foreach(var cardHeaderStack in PlayerSave.CardStorage)
        {
            GameObject anchorObject = new GameObject(cardHeaderStack.type.name + " anchor", typeof(RectTransform));
            anchorObject.transform.SetParent(cardListTransform); //anchor protects card scale from grid layout arranging
            Card card = cardHeaderStack.type.GenerateCard(cardHeaderStack.count);
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
            if (obj.GetComponent<CardPreview>() != null)
                return; //keep info display shown
            //if it is card
            Card card = obj.GetComponent<Card>();
            if (card != null)
            {
                if (eventData.button == PointerEventData.InputButton.Left)
                    cardPreview.SetCardHeader(card.Header);
                return;
            }
            cardPreview.gameObject.SetActive(false);
        }
    }
}