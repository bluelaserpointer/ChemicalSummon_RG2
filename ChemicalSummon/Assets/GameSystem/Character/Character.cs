using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Character", fileName = "NewCharacter", order = -1)]
public class Character : ScriptableObject
{
    [Serializable]
    public struct SpeakTypeAndSentence
    {
        public SpeakType speakType;
        public TranslatableSentence translatableSentence;
        public SpeakTypeAndSentence(SpeakType speakType, TranslatableSentence translatableSentence)
        {
            this.speakType = speakType;
            this.translatableSentence = translatableSentence;
        }
    }
    /// <summary>
    /// 发言种类
    /// </summary>
    public enum SpeakType { StartFusion, StartAttack, Fusion, Counter, Damage, Win, Lose }
    /// <summary>
    /// 发言集合
    /// </summary>
    public List<SpeakTypeAndSentence> speaks = new List<SpeakTypeAndSentence>();
    //inspector
    public new TranslatableSentence name = new TranslatableSentence();
    public int initialHP;
    public Sprite faceIcon;
    public Sprite portrait;
    //data
    /// <summary>
    /// 角色名称
    /// </summary>
    public string Name => name.ToString();
    /// <summary>
    /// 图标-脸
    /// </summary>
    public Sprite FaceIcon => faceIcon;
    /// <summary>
    /// 立绘
    /// </summary>
    public Sprite Portrait => portrait;

    public static Character GetByName(string name)
    {
        return Resources.Load<Character>("Character/" + name);
    }
}