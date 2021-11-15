using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ����Դ/��̬������������ʱ����PlayerSave��
/// </summary>
[DisallowMultipleComponent]
public class General : MonoBehaviour
{
    //inspector
    [Header("Card")]
    public SubstanceCard substanceCardPrefab;
    public MagicCard magicCardPrefab;
    [Header("FusionButton")]
    public GameObject FusionHeatIcon;
    public GameObject FusionElectricIcon;
    public GameObject FusionVigorousnessIcon;
    public GameObject FusionExplosionIcon;
    public GameObject FusionCounterIcon;

    //data
    public static General Instance => PlayerSave.General;
    //static constants (version & path & func)
    public static string Version => "alpha5.5.0pre";
    public static class ResourcePath
    {
        public static string Absolute => "Assets/GameContents/Resources/";
        public static string Substance => "Substance/";
        public static string Magic => "Magic/";
        public static string CardAbility => "Ability/CardAbility/";
        public static string CardSprite => "CardSprite/";
        public static string Reaction => "Reaction/";
        public static string Element => "Element/";
        public static string Item => "ItemHeader/";
        public static string Stage => "StageHeader/";
        public static string Character => "Character/";
    }
    public static Canvas CurrentMainCanvas => AbstractManager.MainCanvas;
    public static bool CurrentSceneIsMatch => SceneManager.GetActiveScene().name.Equals("Match");
    public static bool CurrentSceneIsWorld => SceneManager.GetActiveScene().name.Equals("World");
    public static bool CurrentSceneIsTitle => SceneManager.GetActiveScene().name.Equals("Title");
    public static TranslatableSentenceSO LoadSentence(string name)
    {
        TranslatableSentenceSO sentence = Resources.Load<TranslatableSentenceSO>("TranslatableSentence/" + name);
        if (sentence == null)
            Debug.LogWarning("Cannot find TranslatableSentence ScriptableObject by name: " + name);
        return sentence;
    }
}
