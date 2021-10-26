using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class FairySpot : StepInListener
{
    [SerializeField]
    Substance substance;
    [SerializeField]
    float generateSpanSec = 600;
    [SerializeField]
    ParticleSystem avaliableEffect, rootEffect;

    SubstanceCard generatedCard;
    float passedTime;
    public bool IsRootable { get; protected set; }
    private void Awake()
    {
        OnStepIn.AddListener(() =>
        {
            if (IsRootable)
                RootFairy();
        });
    }
    protected void Start()
    {
        UITraceWorldObject cardPopUp = new GameObject("CardPopUp", typeof(UITraceWorldObject)).GetComponent<UITraceWorldObject>();
        cardPopUp.transform.SetParent(WorldManager.PopUpTransform);
        cardPopUp.target = transform;
        generatedCard = SubstanceCard.GenerateSubstanceCard(substance);
        generatedCard.transform.SetParent(cardPopUp.transform);
        generatedCard.GetComponent<CanvasGroup>().alpha = 0.2F;
        passedTime = generateSpanSec;
    }
    protected void Update()
    {
        if(!IsRootable && (passedTime += Time.deltaTime) > generateSpanSec)
        {
            passedTime = 0;
            GenerateFairy();
        }
    }
    public void GenerateFairy()
    {
        IsRootable = true;
        generatedCard.GetComponent<CanvasGroup>().alpha = 1F;
        avaliableEffect.Play();
    }
    public void RootFairy()
    {
        PlayerSave.SubstanceStorage.Add(substance);
        IsRootable = false;
        generatedCard.GetComponent<CanvasGroup>().alpha = 0.2F;
        avaliableEffect.Stop();
        rootEffect.Play();
    }
    private void OnValidate()
    {
        if (transform.root.Equals(transform))
            return;
        if(substance == null)
            gameObject.name = "FairySpot";
        else
            gameObject.name = substance.chemicalSymbol + "_FairySpot";
    }
}
