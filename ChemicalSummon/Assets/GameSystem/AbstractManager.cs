using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 游戏管理模板
/// </summary>
public abstract class AbstractManager : MonoBehaviour
{
    //inspector
    [SerializeField]
    Canvas mainCanvas;

    //data
    protected void ManagerInit(AbstractManager manager)
    {
        CurrentManagerInstance = manager;
        DynamicGI.UpdateEnvironment();
    }
    public static AbstractManager CurrentManagerInstance { get; private set; }
    public static Canvas MainCanvas => CurrentManagerInstance.mainCanvas;
    protected static bool CurrentSceneIsMatch => General.CurrentSceneIsMatch;
    protected static bool CurrentSceneIsWorld => General.CurrentSceneIsWorld;
    protected static bool CurrentSceneIsTitle => General.CurrentSceneIsTitle;
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
}
