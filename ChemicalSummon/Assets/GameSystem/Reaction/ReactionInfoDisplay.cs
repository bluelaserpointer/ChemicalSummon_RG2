using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class ReactionInfoDisplay : MonoBehaviour
{
    [SerializeField]
    Transform leftSubstanceListTf, rightSubstanceListTf, specialsListTf;
    [SerializeField]
    SubstanceCard substanceCardPrefab;
    [SerializeField]
    ReactionSpecialDamageLabel reactionSpecialDamageLabelPrefab;
    [SerializeField]
    Text clickAnyReactionText;
    //data
    public Reaction Reaction { get; protected set; }
    public void SetReaction(Reaction reaction)
    {
        Reaction = reaction;
        if (clickAnyReactionText.gameObject.activeSelf)
            clickAnyReactionText.gameObject.SetActive(false);
        //display
        foreach (Transform eachTf in leftSubstanceListTf)
            Destroy(eachTf.gameObject);
        foreach (Transform eachTf in rightSubstanceListTf)
            Destroy(eachTf.gameObject);
        foreach (Transform eachTf in specialsListTf)
            Destroy(eachTf.gameObject);
        foreach (var each in reaction.LeftSubstances)
        {
            SubstanceCard card = Instantiate(substanceCardPrefab, leftSubstanceListTf);
            card.Substance = each.type;
            card.InitCardAmount(each.count);
            card.SetDraggable(false);
        }
        foreach (var each in reaction.RightSubstances)
        {
            SubstanceCard card = Instantiate(substanceCardPrefab, rightSubstanceListTf);
            card.Substance = each.type;
            card.InitCardAmount(each.count);
        }
        if(reaction.Electric > 0)
        {
            Instantiate(reactionSpecialDamageLabelPrefab, specialsListTf).SetReactionDamageType(DamageType.Electronic);
        }
        if (reaction.ExplosionPower > 0)
        {
            Instantiate(reactionSpecialDamageLabelPrefab, specialsListTf).SetReactionDamageType(DamageType.Explosion);
        }
        if (reaction.Heat > 0)
        {
            Instantiate(reactionSpecialDamageLabelPrefab, specialsListTf).SetReactionDamageType(DamageType.Heat);
        }
    }
}
