using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class CardInfoDisplay : MonoBehaviour
{
    [SerializeField]
    Image displayBackground;
    [SerializeField]
    SubstanceCard displayCard;
    [SerializeField]
    ReactionListDisplay reactionListDisplay;
    [SerializeField]
    FusionButton FusionButtonPrefab;
    [SerializeField]
    Transform abilityListTransform;
    [SerializeField]
    AbilityLabel abilityLabelPrefab;
    [SerializeField]
    Text cardDescriptionText;

    public bool IsInBattle => ChemicalSummonManager.CurrentSceneIsMatch;
    SubstanceCard referedCard;
    public SubstanceCard ReferedCard => referedCard;
    List<Reaction> relatedReactions = new List<Reaction>();
    private void Awake()
    {
        if (IsInBattle)
        {
            gameObject.SetActive(false);
        }
    }
    public void SetCard(SubstanceCard substanceCard)
    {
        if(!gameObject.activeSelf)
            gameObject.SetActive(true);
        if (!substanceCard.Equals(ReferedCard))
        {
            referedCard = substanceCard;
            displayCard.Substance = substanceCard.Substance;
            displayCard.InitCardAmount(1);
            displayCard.MeltingPoint = substanceCard.MeltingPoint;
            displayCard.BoilingPoint = substanceCard.BoilingPoint;
            cardDescriptionText.text = displayCard.Description;
            bool isMySide = substanceCard.IsMySide;
            displayBackground.color = isMySide ? new Color(1, 1, 1, 0.5F) : new Color(1, 0, 0, 0.5F);
            abilityListTransform.DestroyAllChildren();
            int abilityIndex = 0;
            foreach(var ability in displayCard.Substance.abilities)
            {
                Instantiate(abilityLabelPrefab, abilityListTransform).Set(referedCard, ++abilityIndex, ability);
            }
            UpdateRelatedReactionList();
        }
    }
    public void SetSubstance(Substance substance)
    {
        displayCard.Substance = substance;
        UpdateRelatedReactionList();
        cardDescriptionText.text = displayCard.Description;
        abilityListTransform.DestroyAllChildren();
        int abilityIndex = 0;
        foreach (var ability in displayCard.Substance.abilities)
        {
            Instantiate(abilityLabelPrefab, abilityListTransform).Set(referedCard, ++abilityIndex, ability);
        }
    }
    public void UpdateRelatedReactionList()
    {
        relatedReactions.Clear();
        relatedReactions.AddRange(PlayerSave.FindDiscoveredReactionsByLeftSubstance(displayCard.Substance));
        List<FusionButton> fusionButtons = new List<FusionButton>();
        foreach (Reaction reaction in relatedReactions)
        {
            FusionButton fusionButton = Instantiate(FusionButtonPrefab);
            fusionButton.SetReaction(reaction);
            fusionButtons.Add(fusionButton);
        }
        reactionListDisplay.InitList(fusionButtons);
    }
}
