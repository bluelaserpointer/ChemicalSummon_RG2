using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpText : PopUpGenerator
{
    [Header("Text")]
    [SerializeField]
    bool useTranslatedSentecne = true;
    [SerializeField]
    string simpleSentence;
    [SerializeField]
    TranslatableSentenceSO translatedSentence;
    public string Sentence => useTranslatedSentecne ? translatedSentence : simpleSentence;

    public void SetText(string str)
    {
        useTranslatedSentecne = false;
        simpleSentence = str;
    }
    public void SetTranslatedText(TranslatableSentenceSO sentence)
    {
        useTranslatedSentecne = true;
        translatedSentence = sentence;
    }
    private void Start()
    {
        generatedPopUp = Instantiate(Resources.Load<GameObject>("TextPopUp"), WorldManager.PopUpTransform);
        generatedPopUp.GetComponent<UITraceWorldObject>().target = transform;
        generatedPopUp.GetComponentInChildren<Text>().text = Sentence;
        generatedPopUp.SetActive(false);
    }
}
