using System;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class EnemyThinking : MonoBehaviour
{
    public Action action;

    bool finished;
    float generalThinkingTime = 0.75F;
    float awakenTime;
    public void DoAction()
    {
        action.Invoke();
    }
    private void Awake()
    {
        awakenTime = Time.timeSinceLevelLoad;
    }
    private void Update()
    {
        if(!finished && Time.timeSinceLevelLoad - awakenTime > generalThinkingTime)
        {
            DoAction();
            finished = true;
        }
    }
}
