using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawLog : MonoBehaviour
{
    [SerializeField]
    Image cardImageFrame, cardImage;
    [SerializeField]
    Text logText;

    [SerializeField]
    Sprite cardReverseSprite;
    [SerializeField]
    Color mySideColor, enemySideColor;
    public void Set(Gamer gamer, SubstanceCard card)
    {
        if(gamer.IsMySide)
        {
            cardImageFrame.color = mySideColor;
            cardImage.sprite = card.Image;
        }
        else
        {
            cardImageFrame.color = enemySideColor;
            cardImage.sprite = cardReverseSprite;
        }
        logText.text = ChemicalSummonManager.LoadSentence("PlayerActionLog_Draw").ToString()
            .Replace("$card", card.IsMySide ? card.name : "");
    }
}
