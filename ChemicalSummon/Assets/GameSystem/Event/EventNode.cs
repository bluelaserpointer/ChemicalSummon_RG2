using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 事件节点
/// </summary>
public abstract class EventNode : MonoBehaviour
{
    //data
    Text descriptionText;
    Text DescriptionText => descriptionText ?? (descriptionText = GetComponentInChildren<Text>());
    public abstract string PreferredGameObjectName { get; }
    public Event BelongEvent => transform.parent == null ? null : transform.parent.GetComponent<Event>();
    protected void ProgressEvent()
    {
        BelongEvent?.Progress();
    }
    protected void HideDescriptionText(bool cond)
    {
        if(cond)
        {
            DescriptionText.gameObject.hideFlags = HideFlags.HideInHierarchy;
        }
        else
        {
            DescriptionText.gameObject.hideFlags = HideFlags.None;
        }
    }
    public abstract void Reach();
    public void OnValidate()
    {
        if (BelongEvent == null) //prevent change object name in the prefab preview screen
            return;
        gameObject.name = PreferredGameObjectName;
        OnDataEdit();
    }
    public virtual void OnDataEdit()
    {
        Text sentenceText = GetComponentInChildren<Text>();
        sentenceText.text = name;
        HideDescriptionText(true);
    }
}
