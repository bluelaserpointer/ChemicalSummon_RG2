using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class DeckButton : MonoBehaviour
{
    [SerializeField]
    Text deckNameText;
    [SerializeField]
    Button button;
    [SerializeField]
    Image deckButtonImage;
    [SerializeField]
    Color unlitColor, litColor;

    [HideInInspector]
    public Deck deck;
    public Text DeckNameText => deckNameText;
    public Button Button => button;

    public void Lit(bool cond)
    {
        deckButtonImage.color = cond ? litColor : unlitColor;
    }
}
