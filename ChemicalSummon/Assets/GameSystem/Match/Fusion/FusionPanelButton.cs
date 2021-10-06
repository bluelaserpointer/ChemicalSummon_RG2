using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class FusionPanelButton : MonoBehaviour
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
    public void UpdateList()
    {
        //in counterMode, only counter fusions are avaliable
        SubstanceCard currentAttacker = MatchManager.Player.CurrentAttacker;
        bool counterMode = MatchManager.CurrentTurnType.Equals(TurnType.EnemyAttackTurn) && currentAttacker != null;
        List<Reaction.ReactionMethod> reactionMethods = MatchManager.Player.FindAvailiableReactions(currentAttacker);
        List<FusionButton> fusionButtons = new List<FusionButton>();
        foreach (var method in reactionMethods)
        {
            Reaction reaction = method.reaction;
            FusionButton fusionButton = Instantiate(prefabFusionButton);
            fusionButton.SetReaction(reaction, counterMode);
            fusionButton.Button.onClick.AddListener(() => {
                MatchManager.FusionDisplay.StartReactionAnimation(() =>
                {
                    lastReaction = reaction;
                    //must recheck because player cards may union/distribute in handcards/fields
                    List<SubstanceCard> atNewTimeConsumableCards = MatchManager.Player.GetConsumableCards();
                    if (currentAttacker != null)
                        atNewTimeConsumableCards.Insert(0, currentAttacker);
                    Reaction.ReactionMethod method;
                    if(Reaction.GenerateReactionMethod(reaction, MatchManager.Player, atNewTimeConsumableCards, currentAttacker, out method))
                        MatchManager.Player.DoReaction(method);
                    //counter fusion
                    if (counterMode)
                    {
                        MatchManager.Player.EndDefence();
                    }
                });
            });
            fusionButtons.Add(fusionButton);
        }
        fusionButtonListSlider.GetComponent<ReactionListDisplay>().InitList(fusionButtons);
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
        fusionButtonListSlider.Switch();
    }
    public void HideFusionList()
    {
        fusionButtonListSlider.SlideBack();
    }
}
