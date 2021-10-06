using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class FusionLog : MonoBehaviour
{
    [SerializeField]
    Image fusionFrameImage;
    [SerializeField]
    Text reactionText;
    [SerializeField]
    Transform reactionIconsTf;

    [SerializeField]
    Color mySideColor, enemySideColor;
    [SerializeField]
    GameObject explosionIconPrefab, heatIconPrefab, electricIconPrefab;
    [SerializeField]
    Binder_ImageAndText gemPrefab;
    [SerializeField]
    Color heatGemColor, electricGemColor;
    public void Set(Gamer gamer, Reaction reaction)
    {
        fusionFrameImage.color = gamer.IsMySide ? mySideColor : enemySideColor;
        reactionText.text = reaction.description.Replace("==", " == ");
        reactionIconsTf.DestroyAllChildren();
        //requirementsIcon
        if (reaction.heatRequire > 0)
        {
            Instantiate(gemPrefab, reactionIconsTf).Set(heatGemColor, reaction.heatRequire.ToString());
        }
        if (reaction.electricRequire > 0)
        {
            Instantiate(gemPrefab, reactionIconsTf).Set(electricGemColor, reaction.electricRequire.ToString());
        }
        //productsIcon
        if (reaction.explosion > 0)
            Instantiate(explosionIconPrefab, reactionIconsTf).GetComponentInChildren<Text>().text = reaction.explosion.ToString();
        if (reaction.heat > 0)
            Instantiate(heatIconPrefab, reactionIconsTf).GetComponentInChildren<Text>().text = reaction.heat.ToString();
        if (reaction.electric > 0)
            Instantiate(electricIconPrefab, reactionIconsTf).GetComponentInChildren<Text>().text = reaction.electric.ToString();
    }
}
