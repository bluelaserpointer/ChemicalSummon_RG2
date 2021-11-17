using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class SideMessage : MonoBehaviour
{
    [SerializeField]
    CanvasGroup canvasGroup;
    [SerializeField]
    SBA_TracePosition tracer;

    public CanvasGroup CanvasGroup => canvasGroup;
    public SBA_TracePosition Tracer => tracer;

    public void ShowNextMessage()
    {
        General.Instance.messageGenerator.ShowNextMessage();
    }
}
