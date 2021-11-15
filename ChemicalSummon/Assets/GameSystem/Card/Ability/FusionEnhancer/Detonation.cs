using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detonation : FusionEnhancer
{
    [SerializeField]
    int explosionDamageMultiply = 1;
    public override string GetDescription()
    {
        return General.LoadSentence("Detonation");
    }
    public override bool Appliable(Fusion fusion)
    {
        return fusion.Heat > 0 && fusion.Vigorousness > 0;
    }

    public override void Apply(Fusion fusion)
    {
        fusion.stats.explosionDamage += explosionDamageMultiply * fusion.Heat * fusion.Vigorousness;
    }
    public override void OnFusion(Gamer gamer, Reaction reaction)
    {
        //explosion process is built-in
    }
}
