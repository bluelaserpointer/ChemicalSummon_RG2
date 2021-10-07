using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class FairySpot : AbstractWorldEventObject
{
    [SerializeField]
    Substance substance;
    [SerializeField]
    float generateSpanSec = 600;
    [SerializeField]
    ParticleSystem avaliableEffect, rootEffect;

    float passedTime;
    public bool IsRootable { get; protected set; }
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
        if(!IsRootable && (passedTime += Time.deltaTime) > generateSpanSec)
        {
            passedTime = 0;
            GenerateFairy();
        }
    }
    public void GenerateFairy()
    {
        IsRootable = true;
        avaliableEffect.Play();
    }
    public void RootFairy()
    {
        PlayerSave.SubstanceStorage.Add(substance);
        IsRootable = false;
        avaliableEffect.Stop();
        rootEffect.Play();
    }

    protected override void DoEvent()
    {
        if(IsRootable)
            RootFairy();
    }

    public override void Submit()
    {
    }
}
