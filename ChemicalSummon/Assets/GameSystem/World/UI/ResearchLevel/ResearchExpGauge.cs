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
    [SerializeField]
    CanvasGroup levelUpEffect;
    [SerializeField]
    Text newLevelNumberText;

    int currentLevel;
    float currentExp;
    float expGaugeDisplayTime;
    float levelUpEffectDisplayTime;

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
            expGaugeDisplayTime = 3;
        }
        else if(expGaugeDisplayTime > 0)
        {
            expGaugeDisplayTime -= Time.deltaTime;
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1, 1 * Time.deltaTime);
        }
        else
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0, 1 * Time.deltaTime);
        if(levelUpEffectDisplayTime > 0)
        {
            levelUpEffectDisplayTime -= Time.deltaTime;
            levelUpEffect.alpha = Mathf.MoveTowards(levelUpEffect.alpha, 1, 2 * Time.deltaTime);
        }
        else
            levelUpEffect.alpha = Mathf.MoveTowards(levelUpEffect.alpha, 0, 2 * Time.deltaTime);
    }
    public void UpdateUI()
    {
        int nextLvExp = General.NextLevelExp(currentLevel);
        float progressRatio = 1 - (nextLvExp - currentExp) / (nextLvExp - General.PrevLevelExp(currentLevel));
        if (progressRatio >= 1)
        {
            newLevelNumberText.text = (++currentLevel).ToString();
            levelUpEffectDisplayTime = 3;
            progressRatio -= 1;
        }
        slider.value = progressRatio;
        text.text = General.LoadSentence("ResearchLevel") + " " + currentLevel + " (" + (int)currentExp + "/" + nextLvExp + ")";
    }
}
