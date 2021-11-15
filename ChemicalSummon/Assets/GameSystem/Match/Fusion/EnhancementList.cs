using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class EnhancementList : MonoBehaviour
{
    [SerializeField]
    EnhancementLabel enhancementButtonPrefab;
    [SerializeField]
    Transform listParent;
    [SerializeField]
    SBA_Slide slider;

    List<EnhancementLabel> enhancementLabels = new List<EnhancementLabel>();

    public void UpdateUI()
    {
        slider.Slide(enhancementLabels.Count > 0) ;
        foreach (FusionButton fusionButton in MatchManager.ReactionListDisplay.FusionButtons)
        {
            fusionButton.ClearEnhancement();
            foreach (EnhancementLabel label in enhancementLabels)
            {
                fusionButton.ApplyEnhancement(label.EnhancementAbility as FusionEnhancer);
            }
        }
    }
    public bool AddAbility(Card card, FusionEnhancer enhancer)
    {
        foreach (EnhancementLabel label in enhancementLabels)
        {
            if(label.EnhancementAbility.Equals(enhancer)) {
                return false; //duplicated;
            }
        }
        EnhancementLabel newLabel = Instantiate(enhancementButtonPrefab, listParent);
        newLabel.Set(card, enhancer);
        enhancementLabels.Add(newLabel);
        UpdateUI();
        return true;
    }
    public void RemoveEnhancement(EnhancementLabel label)
    {
        Destroy(label.gameObject);
        enhancementLabels.Remove(label);
        UpdateUI();
    }
    public void RemoveAllEnhancements()
    {
        listParent.DestroyAllChildren();
        enhancementLabels.Clear();
        UpdateUI();
    }
    public void OnEnhanceTargetExecuted()
    {
        foreach (EnhancementLabel label in enhancementLabels)
        {
            //label.EnhancementAbility.OnEnhancementActionDone();
            label.Card.Dispose();
            Destroy(label.gameObject);
        }
        enhancementLabels.Clear();
        UpdateUI();
    }
}
