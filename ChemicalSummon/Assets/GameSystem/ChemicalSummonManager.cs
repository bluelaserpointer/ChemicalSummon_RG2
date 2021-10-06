using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class ChemicalSummonManager : MonoBehaviour
{
    public static string Version => "alpha5.1.0";
    //inspector
    [SerializeField]
    Canvas mainCanvas;

    //data
    protected void ManagerInit(ChemicalSummonManager manager)
    {
        CurrentManagerInstance = manager;
        DynamicGI.UpdateEnvironment();
    }
    public static ChemicalSummonManager CurrentManagerInstance { get; private set; }
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
    public void ProgressEvent()
    {
        PlayerSave.ProgressActiveEvent();
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
        foreach (var substance in Substance.GetAll())
        {
            foreach (var ability in substance.abilities)
                ability.InitDescription();
        }
    }
}
