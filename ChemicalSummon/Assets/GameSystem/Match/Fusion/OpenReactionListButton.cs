using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 融合列表开启按钮
/// </summary>
[DisallowMultipleComponent]
public class OpenReactionListButton : MonoBehaviour
{
    [SerializeField]
    SBA_Slide fusionButtonListSlider;
    [SerializeField]
    TranslatableSentenceSO fusionSentence;
    [SerializeField]
    FusionButton prefabFusionButton;
    [SerializeField]
    Text fusionCountText;
    [SerializeField]
    Image fusionCountImage;
    [SerializeField]
    Color noFusionColor, hasFusionColor;
    [SerializeField]
    SBA_FadingExpand newFusionNoticeAnimation;
    [SerializeField]
    AudioClip clickSE;

    //data
    Reaction lastReaction;
    public Reaction LastReaction => lastReaction;
    public ReactionListDisplay ReactionListDisplay => MatchManager.ReactionListDisplay;
    int lastFusionAmount;
    [HideInInspector]
    public readonly List<Reaction.ReactionMethod> lastAvaliableReactionMethods = new List<Reaction.ReactionMethod>();
    [HideInInspector]
    public readonly UnityEvent onFusionListUpdate = new UnityEvent();

    private void Start()
    {
        fusionCountImage.color = noFusionColor;
        fusionCountText.text = fusionSentence + " 0";
    }
    public void UpdateFusionMethod()
    {
        //in counterMode, only counter fusions are avaliable
        SubstanceCard currentAttacker = MatchManager.Player.CurrentAttacker;
        bool counterMode = MatchManager.CurrentTurnType.Equals(TurnType.EnemyAttackTurn) && currentAttacker != null;
        List<Reaction.ReactionMethod> reactionMethods = MatchManager.Player.FindAvailiableReactions(MatchManager.Player.LearnedReactions, currentAttacker);
        List<FusionButton> fusionButtons = new List<FusionButton>();
        foreach (var method in reactionMethods)
        {
            Fusion fusion = method.fusion;
            FusionButton fusionButton = Instantiate(prefabFusionButton);
            fusionButton.SetFusion(fusion, counterMode);
            //activate fusion
            fusionButton.Button.onClick.AddListener(() => {
                MatchManager.FusionDisplay.StartReactionAnimation(() =>
                {
                    lastReaction = fusion.Reaction;
                    //must recheck because player cards may union/distribute in handcards/fields
                    List<Card> atNewTimeConsumableCards = MatchManager.Player.GetConsumableCards();
                    if (currentAttacker != null)
                        atNewTimeConsumableCards.Insert(0, currentAttacker);
                    Reaction.ReactionMethod method;
                    if(Reaction.GenerateFusionMethod(fusionButton.Fusion, MatchManager.Player, atNewTimeConsumableCards, currentAttacker, out method))
                        MatchManager.Player.DoFusion(method);
                    //enhancements after actions
                    MatchManager.EnhancementList.OnEnhanceTargetExecuted();
                    //counter fusion
                    if (counterMode)
                    {
                        MatchManager.Player.EndDefence();
                    }
                });
            });
            //check energy requirements
            fusionButton.Button.interactable = MatchManager.Player.EnoughEnergyToDo(fusion);
            //TODO: describe which energy resources are unsatisfied.
            fusionButtons.Add(fusionButton);
        }
        ReactionListDisplay.InitList(fusionButtons);
        if (lastFusionAmount < reactionMethods.Count)
        {
            newFusionNoticeAnimation.StartAnimation();
        }
        lastFusionAmount = reactionMethods.Count;
        lastAvaliableReactionMethods.Clear();
        lastAvaliableReactionMethods.AddRange(reactionMethods);
        fusionCountImage.color = lastFusionAmount == 0 ? noFusionColor : hasFusionColor;
        fusionCountText.text = fusionSentence + " " + lastFusionAmount;
        onFusionListUpdate.Invoke();
    }
    public void OnFusionPanelButtonPress()
    {
        MatchManager.PlaySE(clickSE);
        if (fusionButtonListSlider.SlidedOut)
            HideFusionList();
        else
            fusionButtonListSlider.SlideOut();
    }
    public void HideFusionList()
    {
        fusionButtonListSlider.SlideBack();
        ReactionListDisplay.ClearSearchInputField();
        MatchManager.EnhancementList.RemoveAllEnhancements();
    }
    public bool TrySetCard(Card card)
    {
        fusionButtonListSlider.SlideOut();
        return ReactionListDisplay.TrySetCard(card);
    }
}
