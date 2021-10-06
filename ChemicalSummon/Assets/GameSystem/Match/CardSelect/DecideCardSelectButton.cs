using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class DecideCardSelectButton : MonoBehaviour
{
    [SerializeField]
    CardSelectButton cardSelectButtonPrefab;
    [SerializeField]
    SBA_Slide selectListSlider;
    [SerializeField]
    Transform selectListTransform;
    [SerializeField]
    Text selectGuideText;
    [SerializeField]
    TranslatableSentenceSO selectGuideSentence;
    [SerializeField]
    AudioClip clickSE;

    Action<StackedElementList<SubstanceCard>> resultReceiver;
    Action cancelAction;
    int requiredAmountMin, requiredAmountMax;
    int selectedAmount;

    public readonly Stack<CardSelectButton> lastSelections = new Stack<CardSelectButton>();

    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void InitList(int requiredAmountMin, int requiredAmountMax, Action<StackedElementList<SubstanceCard>> resultReceiver, Action cancelAction)
    {
        gameObject.SetActive(true);
        this.requiredAmountMin = requiredAmountMin;
        this.requiredAmountMax = requiredAmountMax;
        selectedAmount = 0;
        this.resultReceiver = resultReceiver;
        this.cancelAction = cancelAction;
        lastSelections.Clear();
        selectListTransform.DestroyAllChildren();
        selectListSlider.SlideOut();
        string requiredRangeStr;
        if (requiredAmountMin == requiredAmountMax)
            requiredRangeStr = requiredAmountMin.ToString();
        else
            requiredRangeStr = requiredAmountMin + "~" + requiredAmountMax;
        selectGuideText.text = selectGuideSentence.ToString().Replace("$amount", requiredRangeStr);
    }
    public void InitList(int requiredAmount, Action<StackedElementList<SubstanceCard>> resultReceiver, Action cancelAction)
    {
        InitList(requiredAmount, requiredAmount, resultReceiver, cancelAction);
    }
    public void StopSelect()
    {
        selectListSlider.SlideBack();
        if (cancelAction != null)
            cancelAction.Invoke();
        gameObject.SetActive(false);
    }
    public void AddSelection(SubstanceCard card)
    {
        CardSelectButton button = Instantiate(cardSelectButtonPrefab, selectListTransform);
        button.Set(card);
        button.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (selectedAmount < requiredAmountMax && button.OnButtonClick())
            {
                lastSelections.Push(button);
                ++selectedAmount;
            }
        });
    }
    public void UndoSelection()
    {
        if(selectedAmount > 0)
        {
            lastSelections.Pop().Undo();
            --selectedAmount;
        }
        else
        {
            StopSelect();
        }
    }

    public void DecideSelections()
    {
        MatchManager.PlaySE(clickSE);
        if(requiredAmountMin <= selectedAmount && selectedAmount <= requiredAmountMax)
        {
            StackedElementList<SubstanceCard> cardConsumes = new StackedElementList<SubstanceCard>();
            foreach(var button in lastSelections)
            {
                cardConsumes.Add(button.Card);
            }
            resultReceiver.Invoke(cardConsumes);
            selectListSlider.SlideBack();
            gameObject.SetActive(false);
        }
    }
}
