using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAbility : MonoBehaviour
{
    [SerializeField]
    Sprite icon;
    [SerializeField]
    [Min(0)]
    int cost = 1;
    [SerializeField]
    bool isActiveAbility = true;
    [SerializeField]
    List<MatchAction> actions;

    //data
    public Sprite Icon => icon;
    string description;
    public string Description => description;
    /// <summary>
    /// 是否为主动技能
    /// </summary>
    public bool IsActiveAbility => isActiveAbility;
    public void InitDescription()
    {
        description = "";
        //costs
        string costDescription = ChemicalSummonManager.LoadSentence("AbilityCost").ToString().Replace("$amount", cost.ToString());
        //effects
        string effectDescription = "";
        bool isFirst = true;
        foreach (var action in actions)
        {
            action.InitDescription();
            if (isFirst)
            {
                isFirst = false;
                effectDescription = action.Description;
            }
            else
            {
                effectDescription += ChemicalSummonManager.LoadSentence("AfterThat") + action.Description;
            }
        }
        description = costDescription + effectDescription;
    }
    public bool IsAvaliable(SubstanceCard card)
    {
        return IsActiveAbility || !actions.Exists(action => !action.CanAction(card.Gamer));
    }
    public void DoAbility(SubstanceCard card)
    {
        ActionLoop(card, 0);
    }
    public void ActionLoop(SubstanceCard card, int actionIndex)
    {
        if (actionIndex < actions.Count)
        {
            actions[actionIndex].DoAction(card.Gamer, () => ActionLoop(card, ++actionIndex));
        }
        else
        {
            card.RemoveAmount(cost, SubstanceCard.DecreaseReason.SkillCost);
        }
    }
    public static CardAbility[] GetBySubstanceName(string cardName)
    {
        GameObject abilityObject = Resources.Load<GameObject>("Chemical/CardAbility/" + cardName);
        return abilityObject == null ? new CardAbility[0] : abilityObject.GetComponentsInChildren<CardAbility>();
    }
}
