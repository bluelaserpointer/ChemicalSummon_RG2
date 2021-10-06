using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class ImageWithTeamFrame : MonoBehaviour
{
    [SerializeField]
    Image contentsImage, frameImage;
    [SerializeField]
    Image plusMinusImage;

    [SerializeField]
    Color mySideColor, enemySideColor;
    [SerializeField]
    Sprite cardReverseSprite;
    [SerializeField]
    Sprite plusSprite, minusSprite;

    public void SetCard(SubstanceCard card, bool hideCardImage = false)
    {
        frameImage.color = card.IsMySide ? mySideColor : enemySideColor;
        contentsImage.sprite = hideCardImage ? cardReverseSprite : card.Image;
    }
    public void SetGamer(Gamer gamer)
    {
        frameImage.color = gamer.IsMySide ? mySideColor : enemySideColor;
        contentsImage.sprite = gamer.Character.faceIcon;
    }
    public void HidePlusMinusIcon()
    {
        plusMinusImage.gameObject.SetActive(false);
    }
    public void SetPlusIcon()
    {
        plusMinusImage.gameObject.SetActive(true);
        plusMinusImage.sprite = plusSprite;
    }
    public void SetMinusIcon()
    {
        plusMinusImage.gameObject.SetActive(true);
        plusMinusImage.sprite = minusSprite;
    }
}
