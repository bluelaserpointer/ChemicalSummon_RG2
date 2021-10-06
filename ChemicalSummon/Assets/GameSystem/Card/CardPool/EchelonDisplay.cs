using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class EchelonDisplay : MonoBehaviour
{
    [SerializeField]
    CardPoolDisplay cardPool;
    [SerializeField]
    Button editButton;

    [SerializeField]
    [Range(1, 3)]
    int index = 1;

    public CardPoolDisplay CardPool => cardPool;
    public int ArrayIndex => index - 1;
    public int NameIndex => index;

    private void Start()
    {
        OnValidate();
        EndEdit();
    }
    public void StartEdit()
    {
        editButton.GetComponentInChildren<Text>().text = ChemicalSummonManager.LoadSentence("EndEdit");
    }
    public void EndEdit()
    {
        editButton.GetComponentInChildren<Text>().text = ChemicalSummonManager.LoadSentence("Edit");
    }
    private void OnValidate()
    {
        if(cardPool != null)
            cardPool.PoolNameText.text = ChemicalSummonManager.LoadSentence("Echelon" + NameIndex);
    }
    public void OnClickEdit()
    {
        WorldManager.DeckScreen.EditEchelon(this);
    }
}
