using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class EventNodeEditor
{
    private static GameObject makeEventNode(GameObject prefab)
    {
        if (Selection.activeTransform == null)
        {
            Debug.LogWarning("EventNodeEditor: 请右键事件(Event)或其子物体选择该功能");
            return null;
        }
        Event activeEvent = Selection.activeTransform.GetComponent<Event>();
        if (activeEvent == null && Selection.activeTransform.parent != null)
            activeEvent = Selection.activeTransform.parent.GetComponent<Event>();
        if (activeEvent == null)
        {
            Debug.LogWarning("EventNodeEditor: 这个物体与事件(Event)无关");
            return null;
        }
        GameObject nodeObject = Object.Instantiate(prefab, activeEvent.transform);
        nodeObject.transform.SetSiblingIndex(Selection.activeTransform.GetSiblingIndex() + 1);
        return nodeObject;
    }
    [MenuItem("GameObject/事件/对话", priority = 1)]
    public static void CreateTalk()
    {
        GameObject eventNodeObject = makeEventNode(Resources.Load<GameObject>("Talk_EventNode"));
        if (eventNodeObject == null)
            return;
        Talk_EventNode eventNode = eventNodeObject.GetComponent<Talk_EventNode>();
        eventNode.OnDataEdit();
    }
    [MenuItem("GameObject/事件/战斗", priority = 1)]
    public static void CreateMatch()
    {
        GameObject eventNodeObject = makeEventNode(Resources.Load<GameObject>("Match_EventNode"));
        if (eventNodeObject == null)
            return;
        Match_EventNode eventNode = eventNodeObject.GetComponent<Match_EventNode>();
        eventNode.OnDataEdit();
    }
    [MenuItem("GameObject/事件/背景", priority = 1)]
    public static void SetBackground()
    {
        GameObject eventNodeObject = makeEventNode(Resources.Load<GameObject>("SetBackground_EventNode"));
        if (eventNodeObject == null)
            return;
        SetBackground_EventNode eventNode = eventNodeObject.GetComponent<SetBackground_EventNode>();
        eventNode.OnDataEdit();
    }
    [MenuItem("GameObject/事件/战斗-等待状态", priority = 1)]
    public static void CreateCheckMatchCondition()
    {
        GameObject eventNodeObject = makeEventNode(Resources.Load<GameObject>("CheckMatchCondition"));
        if (eventNodeObject == null)
            return;
        eventNodeObject.GetComponent<EventNode>().OnDataEdit();
    }
    [MenuItem("GameObject/事件/显示UI", priority = 1)]
    public static void CreateShowUI()
    {
        GameObject eventNodeObject = makeEventNode(Resources.Load<GameObject>("ShowUI_EventNode"));
        if (eventNodeObject == null)
            return;
        eventNodeObject.GetComponent<EventNode>().OnDataEdit();
    }
}
