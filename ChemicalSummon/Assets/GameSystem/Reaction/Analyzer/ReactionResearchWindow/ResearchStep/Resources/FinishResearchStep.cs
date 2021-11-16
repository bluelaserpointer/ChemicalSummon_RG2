using UnityEngine;
using UnityEngine.UI;

public class FinishResearchStep : ResearchStep
{
    [SerializeField]
    Text formulaText, discoverNewSubstanceText;
    [SerializeField]
    Transform discoveredSubstanceListTransform;
    [SerializeField]
    DiscoveredSubstancePreview discoveredSubstancePreviewPrefab;
    public override bool IsAutomatedStep => true;

    public override void OnReach()
    {
        formulaText.text = Reaction.formula;
        PlayerSave.AddDiscoveredReaction(Reaction);
        PlayerSave.AddResearchExp(10);
        WorldManager.ReactionScreen.Init();
        bool hasNewSubstance = false;
        foreach (var substanceStack in Reaction.RightSubstances)
        {
            Substance substance = substanceStack.type;
            if (PlayerSave.DiscoveredCards.Contains(substance))
                continue;
            Instantiate(discoveredSubstancePreviewPrefab, discoveredSubstanceListTransform).Init(substance);
            hasNewSubstance = true;
            PlayerSave.DiscoveredCards.Add(substance);
        }
        if(hasNewSubstance)
        {
            discoverNewSubstanceText.gameObject.SetActive(true);
            WorldManager.ReactionScreen.ReactionAnalyzer.UpdateDiscoveredSubstancePool();
        }
        else
        {
            discoverNewSubstanceText.gameObject.SetActive(false);
        }
    }
}
