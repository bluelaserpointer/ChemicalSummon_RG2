using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class MessagePanel : MonoBehaviour
{
    [SerializeField]
    Text messageText;
    int appearTimeLength;

    [SerializeField]
    TranslatableSentenceSO warnNotPlaceBeforeFusionTurn, warnNotPlaceNonSolid, selectOpponentSlot;

    float appearedTime;
    public void WarnNotPlaceBeforeFusionTurn()
    {
        ShowMessage(warnNotPlaceBeforeFusionTurn, 1);
    }
    public void WarnNotPlaceNonSolid()
    {
        ShowMessage(warnNotPlaceNonSolid, 1);
    }
    public void SelectOpponentSlot()
    {
        ShowMessage(selectOpponentSlot, -1);
    }
    public void ShowMessage(string text, int appearTimeLength = -1)
    {
        messageText.text = text;
        gameObject.SetActive(true);
        appearedTime = Time.timeSinceLevelLoad;
        this.appearTimeLength = appearTimeLength;
    }
    public void Hide()
    {
        appearTimeLength = 0;
    }
    private void Update()
    {
        if(appearTimeLength != -1 && Time.timeSinceLevelLoad - appearedTime > appearTimeLength)
        {
            gameObject.SetActive(false);
        }
    }
}