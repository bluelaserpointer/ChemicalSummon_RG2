using System;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class SettingScreen : ChemicalSummonManager
{
    [SerializeField]
    Transform thingsInTitle, thingsInWorld, thingsInMatch;
    [SerializeField]
    Text languageText;
    private void Start()
    {
        if(CurrentSceneIsTitle)
        {
            thingsInTitle.gameObject.SetActive(true);
            thingsInWorld.gameObject.SetActive(false);
            thingsInMatch.gameObject.SetActive(false);
        }
        else if (CurrentSceneIsWorld)
        {
            thingsInTitle.gameObject.SetActive(false);
            thingsInWorld.gameObject.SetActive(true);
            thingsInMatch.gameObject.SetActive(false);
        }
        else if (CurrentSceneIsMatch)
        {
            thingsInTitle.gameObject.SetActive(false);
            thingsInWorld.gameObject.SetActive(false);
            thingsInMatch.gameObject.SetActive(true);
        }
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    public void SetLanguage(Language language, bool init = false)
    {
        if (!init && TranslatableSentence.currentLanguage.Equals(language))
            return;
        TranslatableSentence.currentLanguage = language;
        switch (language)
        {
            case Language.Chinese:
                languageText.text = "中文";
                break;
            case Language.English:
                languageText.text = "English";
                break;
            case Language.Japanese:
                languageText.text = "日本語";
                break;
        }
        ChemicalSummonManager.UpdateAllSentence();
    }
    public void NextLanguage()
    {
        if((int)TranslatableSentence.currentLanguage == Enum.GetValues(typeof(Language)).Length - 1)
        {
            SetLanguage(0);
        }
        else
        {
            SetLanguage(TranslatableSentence.currentLanguage + 1);
        }
    }
    public void PrevLanguage()
    {
        if (TranslatableSentence.currentLanguage == 0)
        {
            SetLanguage((Language)(Enum.GetValues(typeof(Language)).Length - 1));
        }
        else
        {
            SetLanguage(TranslatableSentence.currentLanguage - 1);
        }
    }
    public void UnlockAllReaction()
    {
        foreach (Reaction each in Reaction.GetAll())
            PlayerSave.AddDiscoveredReaction(each);
        WorldManager.ReactionScreen.Init();
    }
    public void AllSubstancePlusOne()
    {
        foreach (Substance each in Substance.GetAll())
        {
            PlayerSave.SubstanceStorage.Add(each);
        }
        WorldManager.DeckScreen.Init();
    }
    public void AllItemPlusOne()
    {

    }
}
