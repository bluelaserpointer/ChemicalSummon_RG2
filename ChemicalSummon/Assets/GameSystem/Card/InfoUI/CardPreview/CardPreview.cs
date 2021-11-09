using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class CardPreview : MonoBehaviour
{
    [SerializeField]
    Image windowBackground;
    [SerializeField]
    SubstanceCard displaySubstanceCard;
    [SerializeField]
    MagicCard displayMagicCard;
    [SerializeField]
    Transform abilityListTransform;
    [SerializeField]
    AbilityLabel abilityLabelPrefab;
    [SerializeField]
    Text cardDescriptionText;

    public bool IsInBattle => General.CurrentSceneIsMatch;
    public Card DisplayCard { get; protected set; }
    public Card ReferedCard { get; protected set; }
    /// <summary>
    /// 以卡牌设置预览，额外显示敌我阵营所属与卡牌堆叠数信息，默认战斗中调用
    /// </summary>
    /// <param name="card"></param>
    public void SetCard(Card card)
    {
        if (!card.Equals(ReferedCard))
        {
            ReferedCard = card;
            SetCardHeader(card.Header);
            foreach (AbilityLabel abilityLabel in abilityListTransform.GetComponentsInChildren<AbilityLabel>())
                abilityLabel.SetCard(card);
            windowBackground.color = card.IsMySide ? new Color(1, 1, 1, 0.5F) : new Color(1, 0, 0, 0.5F);
        }
        else if (!gameObject.activeSelf)
            gameObject.SetActive(true);
    }
    /// <summary>
    /// 以卡牌头标设置预览
    /// </summary>
    /// <param name="cardHeader"></param>
    public void SetCardHeader(CardHeader cardHeader)
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        //select display card
        Substance substance = cardHeader as Substance;
        if (substance != null)
        {
            DisplayCard = displaySubstanceCard;
            displaySubstanceCard.gameObject.SetActive(true);
            displayMagicCard.gameObject.SetActive(false);
            displaySubstanceCard.Substance = substance;
            displaySubstanceCard.InitCardAmount(1); //prevent ATK change animation
        }
        else
        {
            Magic magic = cardHeader as Magic;
            if (magic != null)
            {
                DisplayCard = displayMagicCard;
                displayMagicCard.gameObject.SetActive(true);
                displaySubstanceCard.gameObject.SetActive(false);
                displayMagicCard.Magic = magic;
                displayMagicCard.CardAmount = 1;
            }
        }
        cardDescriptionText.text = cardHeader.description;
        //TODO: prevent display card's passive ability being invoked
        abilityListTransform.DestroyAllChildren();
        int abilityIndex = 0;
        foreach (var ability in displaySubstanceCard.abilities)
        {
            Instantiate(abilityLabelPrefab, abilityListTransform).Init(++abilityIndex, ability);
        }
    }
}
