using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 章节
/// </summary>
[Serializable]
public class Chapter : MonoBehaviour
{
    [SerializeField]
    string chapterName;
    [SerializeField]
    UnityEvent onChapterStart;
    public UnityEvent OnChapterStart => onChapterStart;
    public string Name => chapterName;
    /// <summary>
    /// 判断是否开始章节
    /// </summary>
    /// <returns></returns>
    public bool JudgeCanStart()
    {
        return true; //TODO: edit
    }
    /// <summary>
    /// 开始章节
    /// </summary>
    public void Start()
    {
        OnChapterStart.Invoke();
    }
}
