using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class DeclareAttackLog : MonoBehaviour
{
    [SerializeField]
    ImageWithTeamFrame cardDisplay;

    public void Set(SubstanceCard card)
    {
        cardDisplay.SetCard(card);
    }
}
