using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 画面顶部的研究等级经验条
/// </summary>
[DisallowMultipleComponent]
public class ResearchExpGauge : MonoBehaviour
{
    [SerializeField]
    Slider slider;
    [SerializeField]
    Text text;
    [SerializeField]
    CanvasGroup canvasGroup;

    int currentLevel;
    float currentExp;
    float displayTime;

    public void Init()
    {
        currentLevel = PlayerSave.ResearchLevel;
        currentExp = PlayerSave.ResearchExp;
        UpdateUI();
    }
    private void Update()
    {
        if(currentExp < PlayerSave.ResearchExp)
        {
            currentExp = Mathf.MoveTowards(currentExp, PlayerSave.ResearchExp, 20 * Time.deltaTime);
            UpdateUI();
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1, 1 * Time.deltaTime);
            displayTime = 3;
        }
        else if(displayTime > 0)
        {
            displayTime -= Time.deltaTime;
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1, 1 * Time.deltaTime);
        }
        else
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0, 1 * Time.deltaTime);
    }
    public void UpdateUI()
    {
        int nextLvExp = General.NextLevelExp(currentLevel);
        float progressRatio = 1 - (nextLvExp - currentExp) / (nextLvExp - General.PrevLevelExp(currentLevel));
        slider.value = progressRatio;
        if (progressRatio >= 1)
        {
            //TODO: Level up animation
            ++currentLevel;
        }
        text.text = General.LoadSentence("ResearchLevel") + " " + currentLevel + " (" + (int)currentExp + "/" + nextLvExp + ")";
    }
}
