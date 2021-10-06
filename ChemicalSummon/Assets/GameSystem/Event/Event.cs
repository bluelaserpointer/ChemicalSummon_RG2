using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 事件
/// </summary>
[DisallowMultipleComponent]
public class Event : MonoBehaviour
{
    [SerializeField]
    UnityEvent onEventNodeChange;
    [SerializeField]
    UnityEvent onEventFinish;

    //data
    public UnityEvent OnEventNodeChange => onEventNodeChange;
    public UnityEvent OnEventFinish => onEventFinish;
    public int NodeCount => transform.childCount;
    int currentIndex = -1;
    public int CurrentIndex {
        get => currentIndex;
        set {
            if (value < 0 || NodeCount <= value || currentIndex == value)
                return;
            currentIndex = value;
            OnEventNodeChange.Invoke();
            //process new event
            Transform newNodeTf = CurrentEventNodeTf;
            EventNode newNode = newNodeTf.GetComponent<EventNode>();
            newNode.Reach();
        }
    }
    public Transform CurrentEventNodeTf => transform.GetChild(currentIndex);
    public void StartEvent()
    {
        PlayerSave.StartEvent(this);
    }
    /// <summary>
    /// 推进事件(每当 推进对话 / 完成战斗 等情况调用)
    /// </summary>
    public void Progress()
    {
        if (CurrentIndex + 1 < NodeCount)
            ++CurrentIndex;
        else
            Finish();
    }
    public void Finish()
    {
        onEventFinish.Invoke();
        ConversationWindow.Close();
    }
}
