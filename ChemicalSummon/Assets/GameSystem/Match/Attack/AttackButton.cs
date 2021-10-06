using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class AttackButton : MonoBehaviour
{
    [SerializeField]
    Image arrowImage;
    [SerializeField]
    Button button;
    public Button Button => button;

    UnityAction addedButtonAction;
    public void SetDirection(bool upside)
    {
        arrowImage.transform.localEulerAngles = new Vector3(0, 0, upside ? 90 : -90);
    }
    public void SetButtonAction(UnityAction buttonAction)
    {
        if(addedButtonAction != null)
        {
            Button.onClick.RemoveListener(addedButtonAction);
        }
        if((addedButtonAction = buttonAction) != null)
        {
            Button.onClick.AddListener(addedButtonAction);
        }
    }
}
