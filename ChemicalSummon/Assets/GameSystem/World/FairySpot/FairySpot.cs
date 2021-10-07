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
    new ParticleSystem particleSystem;

    float passedTime;
    public bool IsRootable { get; protected set; }
    protected override void Start()
    {
        base.Start();
        var emmision = particleSystem.emission;
        emmision.enabled = false;
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
        var emmision = particleSystem.emission;
        emmision.enabled = true;
    }
    public void RootFairy()
    {
        PlayerSave.SubstanceStorage.Add(substance);
        IsRootable = false;
        var emmision = particleSystem.emission;
        emmision.enabled = false;
    }

    protected override void DoEvent()
    {
        RootFairy();
    }

    public override void Submit()
    {
    }
}
