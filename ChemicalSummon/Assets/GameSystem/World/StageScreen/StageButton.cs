using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class StageButton : MonoBehaviour
{
    [SerializeField]
    StageHeader stageHeader;
    [SerializeField]
    Event theEvent;

    private void Start()
    {
        //WorldManager.StageScreen.StageHeader = stageHeader;
        GetComponent<Button>().onClick.AddListener(() => WorldManager.Instance.StartEvent(theEvent));
    }
    private void OnValidate()
    {
        if (stageHeader == null)
            return;
        GetComponentInChildren<Text>().text = stageHeader.name;
        GetComponentInChildren<Image>().sprite = stageHeader.image;
    }
}
