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
    CardPoolDisplay playerStorage;
    [SerializeField]
    ReactionResearchWindow reactionResearchWindow;

    List<Reaction> allReactions;
    List<Reaction> AllReactions {
        get
        {
            return allReactions ?? (allReactions = Reaction.GetAll());
        }
    }
    Reaction displayingValidReaction;
    private void Start()
    {
        foreach(var eachCard in PlayerSave.SubstanceStorage)
        {
            playerStorage.AddCard(eachCard.type, eachCard.amount);
        }
        OnCardsChange();
    }
    public void OnCardsChange()
    {
        displayingValidReaction = null;
        StackedElementList<Substance> puttedSubstances = new StackedElementList<Substance>();
        string puttedSubstancesStr = "";
        foreach(CardSlot slot in slots)
        {
            SubstanceCard card = slot.Card;
            if (card != null)
            {
                puttedSubstances.Add(card.Substance, card.CardAmount);
                if (puttedSubstancesStr.Length > 0)
                    puttedSubstancesStr += " + ";
                if (card.CardAmount > 1)
                    puttedSubstancesStr += card.CardAmount;
                puttedSubstancesStr += card.Symbol;
            }
        }
        if(puttedSubstancesStr.Length == 0)
            reactionText.text = ChemicalSummonManager.LoadSentence("PleaseSetCards");
        else
            reactionText.text = puttedSubstancesStr + " == ?";
        int nearEqReactionCount = 0;
        int discoveredNearEqReactionCount = 0;
        Reaction nearestEqReaction = null;
        int nearestAmountDist = int.MaxValue;
        foreach(Reaction reaction in AllReactions)
        {
            StackedElementList<Substance> eachLeftSubstances = reaction.LeftSubstances;
            if (eachLeftSubstances.CountType() == puttedSubstances.CountType())
            {
                bool nearCond = true;
                int amountDist = 0;
                foreach(var substanceStack in eachLeftSubstances)
                {
                    int putAmount = puttedSubstances.CountStack(substanceStack.type);
                    if (putAmount == 0)
                    {
                        nearCond = false;
                        break;
                    }
                    amountDist += Mathf.Abs(substanceStack.amount - putAmount);
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
        if (nearestAmountDist == 0)
        {
            if (PlayerSave.DiscoveredReactions.Contains(nearestEqReaction))
            {
                messageText.text = ChemicalSummonManager.LoadSentence("AlreadyHadReaction");
                magicCircleImage.color = magicCircleAlreadyHadColor;
            }
            else
            {
                messageText.text = ChemicalSummonManager.LoadSentence("NewReactionFound");
                magicCircleImage.color = magiCircleDiscoverNewColor;
                displayingValidReaction = nearestEqReaction;
            }
        }
        else if (nearEqReactionCount > 0) {
            if(discoveredNearEqReactionCount < nearEqReactionCount)
            {
                if(discoveredNearEqReactionCount == 0)
                {
                    messageText.text = ChemicalSummonManager.LoadSentence("AlmostDiscoverReaction");
                    magicCircleImage.color = magicCircleAlmostDiscoverColor;
                }
                else
                {
                    messageText.text = ChemicalSummonManager.LoadSentence("AlmostDiscoverAnotherReaction");
                    magicCircleImage.color = magicCircleAlmostDiscoverColor;
                }
            }
            else
            {
                messageText.text = ChemicalSummonManager.LoadSentence("CombinationAlreadyResearched");
                magicCircleImage.color = magicCircleAlreadyHadColor;
            }
        }
        else
        {
            messageText.text = ChemicalSummonManager.LoadSentence("NotValidReaction");
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
            if (obj.GetComponent<CardInfoDisplay>() != null)
                return; //keep info display shown
            //if it is card
            SubstanceCard card = obj.GetComponent<SubstanceCard>();
            if (card != null)
            {
                CardSlot slot = slots.Find(each => card.Equals(each.Card));
                if(slot != null)
                {
                    playerStorage.AddCard(card.Substance, 1);
                    card.RemoveAmount(1);
                    if(card.IsDisposing)
                    {
                        slot.SlotTopClear();
                    }
                    OnCardsChange();
                    return;
                }
                CardPoolDisplay belongCardPool = card.transform.GetComponentInParent<CardPoolDisplay>();
                if (playerStorage.Equals(belongCardPool))
                {
                    foreach (CardSlot eachSlot in slots)
                    {
                        SubstanceCard slotCard = eachSlot.Card;
                        if (slotCard == null)
                        {
                            SubstanceCard newCard = SubstanceCard.GenerateSubstanceCard(card.Substance, 1);
                            newCard.SetDraggable(false);
                            newCard.transform.position = card.transform.position;
                            eachSlot.SlotSet(newCard, OnCardsChange);
                            playerStorage.RemoveOneCard(card);
                            return;
                        }
                        else if (slotCard.IsSameSubstance(card))
                        {
                            SubstanceCard newCard = SubstanceCard.GenerateSubstanceCard(card.Substance, 1);
                            newCard.transform.SetParent(ChemicalSummonManager.MainCanvas.transform);
                            newCard.SetDraggable(false);
                            newCard.transform.position = card.transform.position;
                            newCard.TracePosition(slotCard.transform, () =>
                            {
                                slotCard.UnionSameCard(newCard);
                                OnCardsChange();
                            });
                            playerStorage.RemoveOneCard(card);
                            return;
                        }
                    }
                }
                return;
            }
            //CardInfoDisplay.gameObject.SetActive(false);
        }
    }
    public void ResearchReaction()
    {
        if (displayingValidReaction == null)
            return;
        reactionResearchWindow.Init(displayingValidReaction);
    }
}
