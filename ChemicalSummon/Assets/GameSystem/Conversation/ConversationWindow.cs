using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[SerializeField]
public class ConversationWindow : MonoBehaviour
{
    public static ConversationWindow instance;
    //inspector
    [SerializeField]
    Image backgroundImage;
    [SerializeField]
    Image speakerPortrait;
    [SerializeField]
    Text speakerNameText;
    [SerializeField]
    Text speakingText;

    //data
    public static bool IsOpen => instance != null;
    /// <summary>
    /// 设置背景
    /// </summary>
    /// <param name="backgroundSprite"></param>
    public static void SetBackground(Sprite backgroundSprite)
    {
        instance.backgroundImage.sprite = backgroundSprite;
        instance.backgroundImage.enabled = backgroundSprite != null;
    }
    /// <summary>
    /// 当前发言
    /// </summary>
    public static Talk_EventNode CurrentTalk { get; protected set; }
    //static data
    public static ConversationWindow BaseConversation => Resources.Load<ConversationWindow>("BaseConversation");

    public static void Open()
    {
        if (IsOpen)
            return;
        Instantiate(BaseConversation, ChemicalSummonManager.MainCanvas.transform);
        SetBackground(null);
    }
    public static void SetTalk(Talk_EventNode talk)
    {
        Open();
        CurrentTalk = talk;
        if (talk.Character.FaceIcon != null)
        {
            instance.speakerPortrait.sprite = talk.Character.FaceIcon;
            instance.speakerPortrait.color = Color.white;
        }
        else
        {
            instance.speakerPortrait.sprite = null;
            instance.speakerPortrait.color = new Color(0, 0, 0, 0);
        }
        instance.speakerNameText.text = talk.Character.Name;
        instance.speakingText.text = talk.Sentence;
    }
    //role
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if (Input.GetButtonDown("Submit"))
            Submit();
    }
    public static void Close()
    {
        if (!IsOpen)
            return;
        Destroy(instance.gameObject);
        instance = null;
        PlayerSave.ActiveEvent = null;
    }
    public void Submit()
    {
        CurrentTalk.BelongEvent.Progress();
    }
}
