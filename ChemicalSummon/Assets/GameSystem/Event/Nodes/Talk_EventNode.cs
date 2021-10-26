using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class Talk_EventNode : EventNode
{
    [SerializeField]
    Character character;
    [SerializeField]
    TranslatableSentenceSO sentenceSO;
    [SerializeField]
    TranslatableSentence sentence;

    public Character Character => character;
    public string Sentence => sentence.ToString();

    public override string PreferredGameObjectName => (Character != null ? Character.Name : "(?character?)") + ": " + Sentence;

    public override void Reach()
    {
        ConversationWindow.SetTalk(this);
    }

    public override void OnDataEdit()
    {
        if (Character != null)
            GetComponent<Image>().sprite = Character.FaceIcon;
        if (sentenceSO != null)
            sentence = new TranslatableSentence(sentenceSO.sentence);
        Text sentenceText = GetComponentInChildren<Text>();
        sentenceText.text = Sentence;
        HideDescriptionText(true);
    }
}
