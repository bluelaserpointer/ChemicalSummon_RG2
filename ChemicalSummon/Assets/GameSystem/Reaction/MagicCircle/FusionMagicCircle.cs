using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Events;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class FusionMagicCircle : MonoBehaviour
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
    GameObject fusionMagicCircleCardSlot;
    [SerializeField]
    GameObject explosionMark, heatMark, electricMark;
    [SerializeField]
    int radius;
    [SerializeField]
    float cardScale = 1;
    [SerializeField]
    SBA_FadeIO fader;
    [SerializeField]
    GameObject fusionAnimationPrefab;

    Fusion fusion;
    bool isAnimating;

    public void HidePreview()
    {
        if (!isAnimating)
            fader.FadeOut();
    }
    public void PreviewReaction(Fusion fusion)
    {
        if (isAnimating)
            return;
        fader.FadeIn();
        this.fusion = fusion;
        anchorParent.DestroyAllChildren();
        materialCardParent.DestroyAllChildren();
        productCardParent.DestroyAllChildren();
        //materials
        int cardAmount = fusion.LeftSubstances.TotalCount();
        int iteration = 0;
        foreach (var stackedElement in fusion.LeftSubstances)
        {
            Substance substance = stackedElement.type;
            for (int i = 0; i < stackedElement.count; ++i)
            {
                float angle = ++iteration * Mathf.PI * 2 / cardAmount;
                Transform anchor = Instantiate(fusionMagicCircleCardSlot, anchorParent).transform;
                anchor.localPosition = new Vector3(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));
                SubstanceCard card = substance.GenerateSubstanceCard();
                card.transform.SetParent(materialCardParent);
                card.transform.position = anchor.position;
                card.transform.localScale = new Vector3(cardScale, cardScale, 1);
            }
        }
        //products
        foreach (var stackedElement in fusion.RightSubstances)
        {
            SubstanceCard card = stackedElement.type.GenerateSubstanceCard(stackedElement.count);
            card.transform.SetParent(productCardParent);
        }
        //specialDamageIcon
        if (fusion.ExplosionPower > 0)
        {
            explosionMark.SetActive(true);
            explosionMark.GetComponentInChildren<Text>().text = fusion.ExplosionPower.ToString();
        }
        else
            explosionMark.SetActive(false);
        if (fusion.Heat > 0)
        {
            heatMark.SetActive(true);
            heatMark.GetComponentInChildren<Text>().text = fusion.Heat.ToString();
        }
        else
            heatMark.SetActive(false);
        if (fusion.Electric > 0)
        {
            electricMark.SetActive(true);
            electricMark.GetComponentInChildren<Text>().text = fusion.Electric.ToString();
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
                    Instantiate(fusionAnimationPrefab, MatchManager.Instance.transform);
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
        for(int i = 0; i < fusion.HeatRequire; ++i)
        {
            MatchManager.StartGemMoveAnimation(Color.red, MatchManager.Player.StatusPanels.HeatGemPanel.transform.position, transform.position);
        }
        for (int i = 0; i < fusion.ElectricRequire; ++i)
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
