using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 战斗关卡
/// </summary>
public class Match : MonoBehaviour
{
    //inspector
    [SerializeField]
    new TranslatableSentence name;
    [SerializeField]
    MatchBackGround backGround;
    [SerializeField]
    BGMRandomChooser bgmSets;
    [SerializeField]
    protected Character enemySideCharacter;
    [SerializeField]
    protected Deck enemyDeck;
    public List<Reaction> enhanceReactions;
    public List<Reaction> counterReactions;
    public List<Reaction> concernCounters;
    [SerializeField]
    EnemyAI enemyAI;
    [SerializeField]
    List<Item> loots;
    [SerializeField]
    List<Reaction> unlockReactions;

    /// <summary>
    /// 战斗名
    /// </summary>
    public string Name => name.ToString();
    /// <summary>
    /// 背景
    /// </summary>
    public MatchBackGround BackGround => backGround;
    /// <summary>
    /// 我方游戏者信息
    /// </summary>
    public virtual Character MySideCharacter => PlayerSave.SelectedCharacter;
    /// <summary>
    /// 敌方游戏者信息
    /// </summary>
    public Character EnemySideCharacter => enemySideCharacter;
    /// <summary>
    /// 敌方卡组
    /// </summary>
    public Deck EnemyDeck => enemyDeck;
    public EnemyAI EnemyAI => enemyAI;
    public List<Reaction> GetEnemyAllReactions() {
        List<Reaction> list = new List<Reaction>(enhanceReactions);
        list.AddRange(counterReactions);
        return list;
    }
    public AudioClip PickRandomBGM() {
        return bgmSets == null ? null : bgmSets.PickRandom();
    }
    public void Win()
    {
        foreach(Item item in loots)
        {
            PlayerSave.ItemStorage.Add(item);
            unlockReactions.ForEach(each => PlayerSave.AddDiscoveredReaction(each));
        }
    }
}
