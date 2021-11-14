using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public abstract class ResearchStep : MonoBehaviour
{
    [SerializeField]
    TranslatableSentence stepName;
    public abstract bool IsAutomatedStep { get; }
    public Reaction Reaction => researchWindow.Reaction;
    public string StepName => stepName;

    ReactionResearchWindow researchWindow;
    public void Init(ReactionResearchWindow researchWindow)
    {
        this.researchWindow = researchWindow;
        gameObject.SetActive(false);
    }
    public void Reach()
    {
        gameObject.SetActive(true);
        researchWindow.OnStepProceed(transform.GetSiblingIndex());
        OnReach();
    }
    public abstract void OnReach();
    public void NextStep()
    {
        gameObject.SetActive(false);
        transform.parent.GetChild(transform.GetSiblingIndex() + 1).GetComponent<ResearchStep>().Reach();
    }
    public void HideResearchWindow()
    {
        researchWindow.Hide();
    }
}
