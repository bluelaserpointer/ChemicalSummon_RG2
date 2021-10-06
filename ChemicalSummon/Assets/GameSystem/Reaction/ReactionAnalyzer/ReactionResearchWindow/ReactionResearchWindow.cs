using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ReactionResearchWindow : MonoBehaviour
{
    [SerializeField]
    ReactionAnalyzer reactionAnalyzer;
    [SerializeField]
    Transform progressIconListTransform;
    [SerializeField]
    ProgressIcon progressIconPrefab;
    [SerializeField]
    Transform researchMainPanelTf;
    [Header("Prefab")]
    [SerializeField]
    FormulaStep formulaStepPrefab;
    [SerializeField]
    NewSubstanceStep newSubstanceStepPrefab;
    [SerializeField]
    FinishResearchStep finishResearchStepPrefab;

    List<ResearchStep> generatedResearchSteps = new List<ResearchStep>();
    public Reaction Reaction { get; protected set; }
    public void Init(Reaction reaction)
    {
        Reaction = reaction;
        gameObject.SetActive(true);
        generatedResearchSteps.ForEach(step => Destroy(step.gameObject));
        generatedResearchSteps.Clear();
        GenerateStep(formulaStepPrefab);
        GenerateAllStep(reaction.researchSteps);
        List<Substance> unknownSubstances = new List<Substance>();
        foreach (var substanceStack in reaction.rightSubstances)
        {
            Substance substance = substanceStack.type;
            if (!PlayerSave.DiscoveredSubstances.Contains(substance))
            {
                unknownSubstances.Add(substance);
                GenerateStep(newSubstanceStepPrefab).substance = substance;
                GenerateAllStep(substance.researchSteps);
            }
        }
        GenerateStep(finishResearchStepPrefab);
        progressIconListTransform.DestroyAllChildren();
        for (int i = 0; i < generatedResearchSteps.Count; ++i)
        {
            Instantiate(progressIconPrefab, progressIconListTransform).Lit(false);
        }
        generatedResearchSteps[0].Reach();
    }
    public T GenerateStep<T>(T stepPrefab) where T : ResearchStep
    {
        T step = Instantiate(stepPrefab, researchMainPanelTf);
        generatedResearchSteps.Add(step);
        step.Init(this);
        return step;
    }
    public List<ResearchStep> GenerateAllStep(List<ResearchStep> stepPrefabs)
    {
        List<ResearchStep> steps = new List<ResearchStep>();
        stepPrefabs.ForEach(prefab => steps.Add(GenerateStep(prefab))) ;
        return steps;
    }
    public void Hide()
    {
        gameObject.SetActive(false);
        reactionAnalyzer.OnCardsChange();
    }
    public void OnStepProceed(int stepIndex)
    {
        progressIconListTransform.GetChild(stepIndex).GetComponent<ProgressIcon>().Lit(true);
    }
}
