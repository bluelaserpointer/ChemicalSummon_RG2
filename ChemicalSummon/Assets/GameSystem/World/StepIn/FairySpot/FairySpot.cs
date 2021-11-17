using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class FairySpot : StepInListener
{
    [SerializeField]
    CardHeader cardHeader;
    [SerializeField]
    float generateSpanSec = 600;
    [SerializeField]
    ParticleSystem avaliableEffect, rootEffect;

    Card generatedCard;
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
        generatedCard = cardHeader.GenerateCard();
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
        PlayerSave.AddCard(cardHeader);
        IsRootable = false;
        generatedCard.GetComponent<CanvasGroup>().alpha = 0.2F;
        avaliableEffect.Stop();
        rootEffect.Play();
    }
    private void OnValidate()
    {
        if (transform.root.Equals(transform))
            return;
        if(cardHeader == null)
            gameObject.name = "FairySpot";
        else
            gameObject.name = cardHeader.name + "_FairySpot";
    }
}
