using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 总管理
/// </summary>
public abstract class General : MonoBehaviour
{
    //static constants (version & path)
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

    //inspector
    [SerializeField]
    Canvas mainCanvas;

    //data
    protected void ManagerInit(General manager)
    {
        CurrentManagerInstance = manager;
        DynamicGI.UpdateEnvironment();
    }
    public static General CurrentManagerInstance { get; private set; }
    public static Canvas MainCanvas => CurrentManagerInstance.mainCanvas;
    public static bool CurrentSceneIsMatch => SceneManager.GetActiveScene().name.Equals("Match");
    public static bool CurrentSceneIsWorld => SceneManager.GetActiveScene().name.Equals("World");
    public static bool CurrentSceneIsTitle => SceneManager.GetActiveScene().name.Equals("Title");
    /// <summary>
    /// 进入战斗(按钮事件参照用)
    /// </summary>
    /// <param name="match"></param>
    public void GotoMatch(Match match)
    {
        PlayerSave.StartMatch(match);
    }
    public void GotoMenu()
    {
        SceneManager.LoadScene("Title");
    }
    public void GotoWorld()
    {
        SceneManager.LoadScene("World");
    }
    public void StartEvent(Event newEvent)
    {
        PlayerSave.StartEvent(newEvent);
    }
    public static TranslatableSentenceSO LoadSentence(string name)
    {
        TranslatableSentenceSO sentence = Resources.Load<TranslatableSentenceSO>("TranslatableSentence/" + name);
        if (sentence == null)
            Debug.LogWarning("Cannot find TranslatableSentence ScriptableObject by name: " + name);
        return sentence;
    }
    public static void UpdateAllSentence()
    {
        foreach (var abilityPrefab in CardAbility.LoadAllFromResources())
        {
            foreach (var ability in abilityPrefab.GetComponentsInChildren<CardAbility>())
                ability.UpdateDescriptionLanguage();
        }
    }
}
