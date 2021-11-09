using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detonation : FusionEnhancer
{
    public override Reaction.ReactionMethod Apply(Reaction.ReactionMethod method)
    {
        throw new System.NotImplementedException();
    }

    public override string GetDescription()
    {
        return General.LoadSentence("Detonation");
    }
}
