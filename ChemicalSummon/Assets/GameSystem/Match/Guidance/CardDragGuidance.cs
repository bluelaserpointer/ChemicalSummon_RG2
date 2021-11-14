using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// ¿¨ÅÆÅ²¶¯Ïòµ¼
/// </summary>
[DisallowMultipleComponent]
public class CardDragGuidance : MonoBehaviour
{
    [SerializeField]
    GameObject guidancePrefab;

    public void StartCardDrag(Card card)
    {
        if(General.CurrentSceneIsMatch)
        {
            if (card.Header.IsSubstance)
            {
                foreach (ShieldCardSlot slot in MatchManager.MyField.Slots)
                {
                    if (slot.AllowSetAsMainCard(card as SubstanceCard, false))
                    {
                        if (slot.IsEmpty)
                        {
                            SetGuidance(slot.gameObject, General.LoadSentence("summon"));
                        }
                        else
                        {
                            SetGuidance(slot.gameObject, General.LoadSentence("stack"));
                        }
                    }
                }
                SetGuidance(MatchManager.CardDragAreaForOpenReactionList, General.LoadSentence("fusion"));
            }
            else if (card.Header.IsMagic)
            {
                MagicCard magicCard = card as MagicCard;
                //fusion enchance
                if (magicCard.TryGetFusionEnhancer != null)
                    SetGuidance(MatchManager.CardDragAreaForOpenReactionList, General.LoadSentence("enhance"));
                else
                    print("that null");
            }
        }
    }
    public void SetGuidance(GameObject target, string text)
    {
        GameObject guidance = Instantiate(guidancePrefab, transform);
        guidance.GetComponent<RectTransform>().sizeDelta = target.GetComponent<RectTransform>().sizeDelta;
        guidance.transform.position = target.transform.position;
        guidance.GetComponentInChildren<Text>().text = text;
    }
    public void ClearAllGuidances()
    {
        transform.DestroyAllChildren();
    }
}
