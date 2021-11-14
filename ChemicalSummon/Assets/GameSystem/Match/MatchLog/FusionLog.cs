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
    Binder_ImageAndText gemPrefab;
    [SerializeField]
    Color heatGemColor, electricGemColor;
    public void Set(Gamer gamer, Fusion fusion)
    {
        fusionFrameImage.color = gamer.IsMySide ? mySideColor : enemySideColor;
        reactionText.text = fusion.Reaction.formula.Replace("==", " == ");
        reactionIconsTf.DestroyAllChildren();
        //requirementsIcon
        if (fusion.HeatRequire > 0)
        {
            Instantiate(gemPrefab, reactionIconsTf).Set(heatGemColor, fusion.HeatRequire.ToString());
        }
        if (fusion.ElectricRequire > 0)
        {
            Instantiate(gemPrefab, reactionIconsTf).Set(electricGemColor, fusion.ElectricRequire.ToString());
        }
        //productsIcon
        if (fusion.Electric > 0)
            Instantiate(General.Instance.FusionElectricIcon, reactionIconsTf).GetComponentInChildren<Text>().text = fusion.Electric.ToString();
        if (fusion.Heat > 0)
            Instantiate(General.Instance.FusionHeatIcon, reactionIconsTf).GetComponentInChildren<Text>().text = fusion.Heat.ToString();
        if (fusion.Vigorousness > 0)
            Instantiate(General.Instance.FusionVigorousnessIcon, reactionIconsTf).GetComponentInChildren<Text>().text = fusion.Vigorousness.ToString();
        if (fusion.ExplosionPower > 0)
            Instantiate(General.Instance.FusionExplosionIcon, reactionIconsTf).GetComponentInChildren<Text>().text = fusion.ExplosionDamage.ToString();
    }
}
