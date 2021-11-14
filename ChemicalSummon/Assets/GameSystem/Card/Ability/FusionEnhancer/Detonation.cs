using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detonation : FusionEnhancer
{
    [SerializeField]
    int minHeat;
    [SerializeField]
    int minVigorousness;

    public override string GetDescription()
    {
        return General.LoadSentence("Detonation");
    }
    public override bool Appliable(Fusion fusion)
    {
        return fusion.Heat >= minHeat && fusion.Vigorousness >= minVigorousness;
    }

    public override void Apply(Fusion fusion)
    {
        fusion.stats.explosionPower = 2;
    }
    public override void OnFusion(Gamer gamer, Reaction reaction)
    {
        //explosion process is built-in
    }
}
