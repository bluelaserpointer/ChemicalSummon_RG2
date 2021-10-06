using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class DiscoveredSubstancePreview : MonoBehaviour
{
    [SerializeField]
    Image cardImage;
    [SerializeField]
    Text nameText, symbolText;
    [SerializeField]
    SubstanceCard displayCard;
    public void Init(Substance substance)
    {
        cardImage.sprite = substance.image;
        nameText.text = substance.name;
        symbolText.text = substance.chemicalSymbol;
        displayCard.SetDraggable(false);
        displayCard.Substance = substance;
        displayCard.InitCardAmount(1);
    }
}
