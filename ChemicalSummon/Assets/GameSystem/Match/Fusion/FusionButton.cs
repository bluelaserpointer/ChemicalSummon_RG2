using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    GameObject counterIconPrefab, explosionIconPrefab, heatIconPrefab, electricIconPrefab;
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
    public Reaction Reaction { get; protected set; }

    private void Awake()
    {
        Button.onClick.AddListener(() => MatchManager.PlaySE(clickSE));
    }
    public void SetReaction(Reaction reaction, bool isCounter = false)
    {
        Reaction = reaction;
        formulaText.text = Reaction.description;
        iconsTf.DestroyAllChildren();
        if (isCounter)
            Instantiate(counterIconPrefab, iconsTf);
        //requirementsIcon
        if (Reaction.heatRequire > 0)
        {
            Instantiate(gemPrefab, iconsTf).Set(heatGemColor, Reaction.heatRequire.ToString());
        }
        if (Reaction.electricRequire > 0)
        {
            Instantiate(gemPrefab, iconsTf).Set(electricGemColor, Reaction.electricRequire.ToString());
        }
        //productsIcon
        if (Reaction.explosion > 0)
            Instantiate(explosionIconPrefab, iconsTf).GetComponentInChildren<Text>().text = Reaction.explosion.ToString();
        if (Reaction.heat > 0)
            Instantiate(heatIconPrefab, iconsTf).GetComponentInChildren<Text>().text = Reaction.heat.ToString();
        if (Reaction.electric > 0)
            Instantiate(electricIconPrefab, iconsTf).GetComponentInChildren<Text>().text = Reaction.electric.ToString();
    }
    public void MarkNew(bool cond)
    {
        newSign.gameObject.SetActive(cond);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(MatchManager.CurrentSceneIsMatch)
            MatchManager.FusionDisplay.PreviewReaction(Reaction);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (MatchManager.CurrentSceneIsMatch)
            MatchManager.FusionDisplay.HidePreview();
    }
}
