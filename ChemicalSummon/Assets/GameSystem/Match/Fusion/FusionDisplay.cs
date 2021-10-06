using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class FusionDisplay : MonoBehaviour
{
    [SerializeField]
    Transform anchorParent;
    [SerializeField]
    Transform materialCardParent;
    [SerializeField]
    Transform productCardParent;
    [SerializeField]
    Transform markParent;
    [SerializeField]
    GameObject fusionDisplayCardSlot;
    [SerializeField]
    GameObject explosionMark, heatMark, electricMark;
    [SerializeField]
    int radius;
    [SerializeField]
    float cardScale = 1;
    [SerializeField]
    SBA_FadeIO fader;
    [SerializeField]
    GameObject fusionEffectPrefab;

    Reaction reaction;
    bool isAnimating;

    public void HidePreview()
    {
        if (!isAnimating)
            fader.FadeOut();
    }
    public void PreviewReaction(Reaction reaction)
    {
        if (isAnimating)
            return;
        fader.FadeIn();
        this.reaction = reaction;
        anchorParent.DestroyAllChildren();
        materialCardParent.DestroyAllChildren();
        productCardParent.DestroyAllChildren();
        //material
        int cardAmount = reaction.leftSubstances.CountStack();
        int iteration = 0;
        foreach (var stackedElement in reaction.leftSubstances)
        {
            Substance substance = stackedElement.type;
            for (int i = 0; i < stackedElement.amount; ++i)
            {
                float angle = ++iteration * Mathf.PI * 2 / cardAmount;
                Transform anchor = Instantiate(fusionDisplayCardSlot, anchorParent).transform;
                anchor.localPosition = new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));
                SubstanceCard card = SubstanceCard.GenerateSubstanceCard(substance);
                card.transform.SetParent(materialCardParent);
                card.transform.position = anchor.position;
                card.transform.localScale = new Vector3(cardScale, cardScale, 1);
            }
        }
        //product
        foreach (var stackedElement in reaction.rightSubstances)
        {
            SubstanceCard card = SubstanceCard.GenerateSubstanceCard(stackedElement.type);
            card.InitCardAmount(stackedElement.amount);
            card.transform.SetParent(productCardParent);
        }
        //specialDamageIcon
        if (reaction.explosion > 0)
        {
            explosionMark.SetActive(true);
            explosionMark.GetComponentInChildren<Text>().text = reaction.explosion.ToString();
        }
        else
            explosionMark.SetActive(false);
        if (reaction.heat > 0)
        {
            heatMark.SetActive(true);
            heatMark.GetComponentInChildren<Text>().text = reaction.heat.ToString();
        }
        else
            heatMark.SetActive(false);
        if (reaction.electric > 0)
        {
            electricMark.SetActive(true);
            electricMark.GetComponentInChildren<Text>().text = reaction.electric.ToString();
        }
        else
            electricMark.SetActive(false);
    }
    public void StartReactionAnimation(UnityAction afterAction)
    {
        isAnimating = true;
        bool isFirstOne = true;
        foreach (Transform each in anchorParent)
        {
            SBA_TracePosition tracer = each.GetComponent<SBA_TracePosition>();
            tracer.SetTarget(transform.position);
            if(isFirstOne)
            {
                tracer.AddReachAction(() => {
                    afterAction.Invoke();
                    Instantiate(fusionEffectPrefab, MatchManager.Instance.transform);
                    isAnimating = false;
                    HidePreview();
                });
                isFirstOne = false;
            }
            tracer.StartAnimation();
        }
        foreach (Transform each in markParent)
        {
            if (!each.gameObject.activeSelf)
                continue;
            SBA_FadingExpand expander = each.GetComponent<SBA_FadingExpand>();
            expander.StartAnimation();
        }
        for(int i = 0; i < reaction.heatRequire; ++i)
        {
            MatchManager.StartGemMoveAnimation(Color.red, MatchManager.Player.StatusPanels.HeatGemPanel.transform.position, transform.position);
        }
        for (int i = 0; i < reaction.electricRequire; ++i)
        {
            MatchManager.StartGemMoveAnimation(Color.yellow, MatchManager.Player.StatusPanels.ElectricGemPanel.transform.position, transform.position);
        }
    }
    private void Update()
    {
        int index = -1;
        foreach(Transform eachCardTf in materialCardParent)
        {
            Transform anchor =  anchorParent.GetChild(++index);
            eachCardTf.position = anchor.transform.position;
        }
    }
}
