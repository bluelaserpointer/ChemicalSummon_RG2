using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class ReactionAnalyzer : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    Image magicCircleImage;
    [SerializeField]
    Color magicCircleNotValidColor, magicCircleAlmostDiscoverColor, magicCircleAlreadyHadColor, magiCircleDiscoverNewColor;
    [SerializeField]
    List<CardSlot> slots;
    [SerializeField]
    Image messagePanel;
    [SerializeField]
    Text reactionText, messageText, nearEqReactionAmountText;
    [SerializeField]
    CardPoolDisplay discoveredSubstancePool;
    [SerializeField]
    Button fusionButton, researchButton;
    [SerializeField]
    ReactionResearchWindow reactionResearchWindow;

    List<Reaction> allReactions;
    List<Reaction> AllReactions {
        get
        {
            return allReactions ?? (allReactions = Reaction.GetAll());
        }
    }
    Reaction displayingReaction;
    public void OnEnable()
    {
        Init();
    }
    private void Init()
    {
        slots.ForEach(slot => slot.DestroyTop());
        UpdateDiscoveredSubstancePool();
        OnCardsChange();
    }
    public void UpdateDiscoveredSubstancePool()
    {
        discoveredSubstancePool.Clear();
        foreach (CardHeader eachCardHeader in PlayerSave.DiscoveredCards)
        {
            discoveredSubstancePool.AddCard(eachCardHeader);
        }
    }
    public void OnCardsChange()
    {
        displayingReaction = null;
        TypeAndCountList<Substance> puttedSubstances = new TypeAndCountList<Substance>();
        string puttedSubstancesStr = "";
        foreach(CardSlot slot in slots)
        {
            SubstanceCard card = slot.TopCard as SubstanceCard;
            if (card != null)
            {
                puttedSubstances.Add(card.Substance, card.CardAmount);
                if (puttedSubstancesStr.Length > 0)
                    puttedSubstancesStr += " + ";
                if (card.CardAmount > 1)
                    puttedSubstancesStr += card.CardAmount;
                puttedSubstancesStr += card.Formula;
            }
        }
        if(puttedSubstancesStr.Length == 0)
            reactionText.text = General.LoadSentence("PleaseSetCards");
        else
            reactionText.text = puttedSubstancesStr + " == ?";
        int nearEqReactionCount = 0;
        int discoveredNearEqReactionCount = 0;
        Reaction nearestEqReaction = null;
        int nearestAmountDist = int.MaxValue;
        foreach(Reaction reaction in AllReactions)
        {
            TypeAndCountList<Substance> eachLeftSubstances = reaction.LeftSubstances;
            if (eachLeftSubstances.TypeCount() == puttedSubstances.TypeCount())
            {
                bool nearCond = true;
                int amountDist = 0;
                foreach(var substanceStack in eachLeftSubstances)
                {
                    int putAmount = puttedSubstances.StackCount(substanceStack.type);
                    if (putAmount == 0)
                    {
                        nearCond = false;
                        break;
                    }
                    amountDist += Mathf.Abs(substanceStack.count - putAmount);
                }
                if (!nearCond)
                    continue;
                ++nearEqReactionCount;
                if (PlayerSave.DiscoveredReactions.Contains(reaction))
                    ++discoveredNearEqReactionCount;
                if (amountDist < nearestAmountDist)
                {
                    nearestAmountDist = amountDist;
                    nearestEqReaction = reaction;
                }
            }
        }
        //print("putted: " + puttedSubstancesStr + ", nearestAmountDist: " + nearestAmountDist);
        nearEqReactionAmountText.text = discoveredNearEqReactionCount + "/" + nearEqReactionCount;
        fusionButton.interactable = researchButton.interactable = false;
        if (nearestAmountDist == 0)
        {
            displayingReaction = nearestEqReaction;
            if (PlayerSave.DiscoveredReactions.Contains(nearestEqReaction))
            {
                messageText.text = General.LoadSentence("AlreadyHadReaction");
                magicCircleImage.color = magicCircleAlreadyHadColor;
                fusionButton.interactable = true;
                reactionText.text = displayingReaction.name;
            }
            else
            {
                messageText.text = General.LoadSentence("NewReactionFound");
                magicCircleImage.color = magiCircleDiscoverNewColor;
                researchButton.interactable = true;
            }
        }
        else if (nearEqReactionCount > 0) {
            if(discoveredNearEqReactionCount < nearEqReactionCount)
            {
                if(discoveredNearEqReactionCount == 0)
                {
                    messageText.text = General.LoadSentence("AlmostDiscoverReaction");
                    magicCircleImage.color = magicCircleAlmostDiscoverColor;
                }
                else
                {
                    messageText.text = General.LoadSentence("AlmostDiscoverAnotherReaction");
                    magicCircleImage.color = magicCircleAlmostDiscoverColor;
                }
            }
            else
            {
                messageText.text = General.LoadSentence("CombinationAlreadyResearched");
                magicCircleImage.color = magicCircleAlreadyHadColor;
            }
        }
        else
        {
            messageText.text = General.LoadSentence("NotValidReaction");
            magicCircleImage.color = magicCircleNotValidColor;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        foreach (RaycastResult rayResult in results)
        {
            GameObject obj = rayResult.gameObject;
            //if it is CardInfoDisplay
            if (obj.GetComponent<CardPreview>() != null)
                return; //keep info display shown
            //if it is card
            SubstanceCard card = obj.GetComponent<SubstanceCard>();
            if (card != null)
            {
                CardSlot slot = slots.Find(each => card.Equals(each.TopCard));
                if(slot != null)
                {
                    card.RemoveAmount(1);
                    if(card.IsDisposing)
                        slot.DisbandTop();
                    SubstanceCard animationCard = card.Substance.GenerateSubstanceCard();
                    animationCard.SetDraggable(false);
                    animationCard.transform.position = card.transform.position;
                    animationCard.transform.SetParent(transform);
                    animationCard.TracePosition(discoveredSubstancePool.transform.position, () =>
                    {
                        Destroy(animationCard.gameObject);
                    });
                    OnCardsChange();
                    return;
                }
                CardPoolDisplay belongCardPool = card.transform.GetComponentInParent<CardPoolDisplay>();
                if (discoveredSubstancePool.Equals(belongCardPool))
                {
                    foreach (CardSlot eachSlot in slots)
                    {
                        SubstanceCard slotCard = eachSlot.TopCard as SubstanceCard;
                        if (slotCard == null)
                        {
                            SubstanceCard newCard = card.Substance.GenerateSubstanceCard();
                            newCard.SetDraggable(false);
                            newCard.transform.position = card.transform.position;
                            eachSlot.SlotSet(newCard, OnCardsChange);
                            return;
                        }
                        else if (slotCard.IsSameSubstance(card))
                        {
                            SubstanceCard newCard = card.Substance.GenerateSubstanceCard();
                            newCard.transform.SetParent(AbstractManager.MainCanvas.transform);
                            newCard.SetDraggable(false);
                            newCard.transform.position = card.transform.position;
                            newCard.TracePosition(slotCard.transform, () =>
                            {
                                slotCard.TryUnion(newCard);
                                OnCardsChange();
                            });
                            return;
                        }
                    }
                }
                return;
            }
        }
    }
    public void ResearchReaction()
    {
        reactionResearchWindow.Init(displayingReaction);
    }
    public void FusionReaction()
    {
        PlayerSave.CardStorage.RemoveAll(displayingReaction.LeftSubstances);
        PlayerSave.CardStorage.AddAll(displayingReaction.RightSubstances);
        Init();
    }
}