using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopUpButton : PopUpText
{
    [Header("Button")]
    [SerializeField]
    UnityEvent onClick = new UnityEvent();
    public UnityEvent OnClick => onClick;

    private void Start()
    {
        generatedPopUp = Instantiate(Resources.Load<GameObject>("ButtonPopUp"), WorldManager.PopUpTransform);
        generatedPopUp.GetComponent<UITraceWorldObject>().target = transform;
        generatedPopUp.GetComponentInChildren<Text>().text = Sentence;
        generatedPopUp.GetComponentInChildren<Button>().onClick.AddListener(() => onClick.Invoke());
        generatedPopUp.SetActive(false);
    }
}
