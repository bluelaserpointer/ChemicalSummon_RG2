using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class StageScreen : MonoBehaviour
{
    StageHeader stageHeader;
    public StageHeader StageHeader
    {
        get => stageHeader;
        set
        {
            stageHeader = value;
            UpdateUI();
        }
    }

    private void Start()
    {
        UpdateUI();
    }
    public void UpdateUI()
    {
        
    }
    public void StartEvent()
    {

    }
}
