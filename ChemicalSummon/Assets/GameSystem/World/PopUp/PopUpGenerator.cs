using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class PopUpGenerator : MonoBehaviour
{
    protected GameObject generatedPopUp;
    public void ShowPopUp()
    {
        generatedPopUp.gameObject.SetActive(true);
    }
    public void HidePopUp()
    {
        generatedPopUp.gameObject.SetActive(false);
    }
}
