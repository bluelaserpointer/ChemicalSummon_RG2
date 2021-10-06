using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class ReactionScreen : MonoBehaviour
{
    [SerializeField]
    FusionButton fusionButtonPrefab;
    [SerializeField]
    ReactionListDisplay reactionListDisplay;
    [SerializeField]
    Slider reactionUnlockRateSlider;
    [SerializeField]
    Text reactionUnlockProgressText;
    [SerializeField]
    ReactionInfoDisplay reactionInfoDisplay;
    [SerializeField]
    ReactionAnalyzer reactionAnalyzer;

    private void Start()
    {
        Init();
    }
    // Start is called before the first frame update
    public void Init()
    {
        List<FusionButton> fusionButtons = new List<FusionButton>();
        foreach (Reaction reaction in PlayerSave.DiscoveredReactions)
        {
            FusionButton fusionButton = Instantiate(fusionButtonPrefab);
            fusionButton.SetReaction(reaction);
            if (PlayerSave.NewDicoveredReactions.Contains(reaction))
                fusionButton.MarkNew(true);
            fusionButton.Button.onClick.AddListener(() => {
                reactionInfoDisplay.SetReaction(reaction);
                fusionButton.MarkNew(false);
                PlayerSave.CheckedReaction(fusionButton.Reaction);
                if (PlayerSave.NewDicoveredReactions.Count == 0)
                    WorldManager.NewReactionSign.gameObject.SetActive(false);
            });
            fusionButtons.Add(fusionButton);
        }
        reactionListDisplay.InitList(fusionButtons);
        float unlocked = PlayerSave.DiscoveredReactions.Count, total = Reaction.GetAll().Count;
        float unlockRate = unlocked / total;
        reactionUnlockRateSlider.value = unlockRate;
        reactionUnlockProgressText.text = unlocked + "/" + total + "(" + (float)Math.Round(unlockRate * 100, 2) + "%)";
        //new reaction sign on the LeftTab
        WorldManager.NewReactionSign.gameObject.SetActive(PlayerSave.NewDicoveredReactions.Count > 0);
    }
    public void ShowAnalyzer(bool cond)
    {
        reactionAnalyzer.gameObject.SetActive(cond);
    }
}
