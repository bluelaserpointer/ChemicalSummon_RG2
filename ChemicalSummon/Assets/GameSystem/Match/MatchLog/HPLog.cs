using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class HPLog : MonoBehaviour
{
    [SerializeField]
    Image gamerFrameImage, gamerImage;
    [SerializeField]
    Color mySideColor, enemySideColor;
    [SerializeField]
    Text hpText;

    public void Set(Gamer gamer, int hpChange)
    {
        gamerFrameImage.color = gamer.IsMySide ? mySideColor : enemySideColor;
        gamerImage.sprite = gamer.Character.FaceIcon;
        hpText.text = (hpChange > 0 ? "+" + hpChange : hpChange.ToString()) + " HP";
    }
}
