using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 融合按钮
/// </summary>
[DisallowMultipleComponent]
public class FusionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    Button button;
    [SerializeField]
    Text formulaText;
    [SerializeField]
    Transform iconsTf;
    [SerializeField]
    Binder_ImageAndText gemPrefab;
    [SerializeField]
    Color heatGemColor, electricGemColor;
    [SerializeField]
    GameObject newSign;
    [SerializeField]
    AudioClip clickSE;

    //data
    public Button Button => button;
    public Fusion Fusion { get; protected set; }
    public Reaction Reaction => Fusion.Reaction;
    public bool IsCounter { get; protected set; }

    private void Awake()
    {
        Button.onClick.AddListener(() => MatchManager.PlaySE(clickSE));
    }
    /// <summary>
    /// 设置融合，用于战斗内
    /// </summary>
    /// <param name="fusion"></param>
    /// <param name="isCounter"></param>
    public void SetFusion(Fusion fusion, bool isCounter = false)
    {
        Fusion = fusion;
        IsCounter = isCounter;
        UpdateUI();
    }
    /// <summary>
    /// 设置反应式，用于战斗外
    /// </summary>
    /// <param name="reaction"></param>
    public void SetReaction(Reaction reaction)
    {
        Fusion = new Fusion(reaction);
        IsCounter = false;
        UpdateUI();
    }
    public void MarkNew(bool cond)
    {
        newSign.gameObject.SetActive(cond);
    }
    public void ApplyEnhancement(FusionEnhancer enhancer)
    {
        enhancer.Apply(Fusion);
        UpdateUI();
    }
    public void UpdateUI()
    {
        formulaText.text = Reaction.formula;
        iconsTf.DestroyAllChildren();
        if (IsCounter)
            Instantiate(General.Instance.FusionCounterIcon, iconsTf);
        //requirementsIcon
        if (Fusion.HeatRequire > 0)
        {
            Instantiate(gemPrefab, iconsTf).Set(heatGemColor, Fusion.HeatRequire.ToString());
        }
        if (Fusion.ElectricRequire > 0)
        {
            Instantiate(gemPrefab, iconsTf).Set(electricGemColor, Fusion.ElectricRequire.ToString());
        }
        //productsIcon
        if (Fusion.Heat > 0)
            Instantiate(General.Instance.FusionHeatIcon, iconsTf).GetComponentInChildren<Text>().text = Fusion.Heat.ToString();
        if (Fusion.Electric > 0)
            Instantiate(General.Instance.FusionElectricIcon, iconsTf).GetComponentInChildren<Text>().text = Fusion.Electric.ToString();
        if (Fusion.Vigorousness > 0)
            Instantiate(General.Instance.FusionVigorousnessIcon, iconsTf).GetComponentInChildren<Text>().text = Fusion.Heat.ToString();
        if (Fusion.ExplosionPower > 0)
            Instantiate(General.Instance.FusionExplosionIcon, iconsTf).GetComponentInChildren<Text>().text = Fusion.ExplosionDamage.ToString();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(General.CurrentSceneIsMatch)
            MatchManager.FusionDisplay.PreviewReaction(Fusion);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (General.CurrentSceneIsMatch)
            MatchManager.FusionDisplay.HidePreview();
    }
}
