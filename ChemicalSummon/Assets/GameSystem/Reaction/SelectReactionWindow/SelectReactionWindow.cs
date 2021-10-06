using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public class SelectReactionWindow : MonoBehaviour
{
    [SerializeField]
    ReactionListDisplay reactionListDisplay;
    [SerializeField]
    FusionButton fusionButtonPrefab;
    [SerializeField]
    UnityEvent onSelect;

    List<FusionButton> selectedButtons = new List<FusionButton>();
    public List<FusionButton> SelectedButtons => selectedButtons;
    private void Start()
    {
        List<FusionButton> fusionButtons = new List<FusionButton>();
        foreach(Reaction reaction in PlayerSave.DiscoveredReactions)
        {
            FusionButton fusionButton = Instantiate(fusionButtonPrefab);
            fusionButton.SetReaction(reaction);
            fusionButtons.Add(fusionButton);
        }
        reactionListDisplay.InitList(fusionButtons);
        reactionListDisplay.AddButtonAction((button) =>
        {
            if(selectedButtons.Contains(button))
            {
                button.Button.image.color = Color.white;
                selectedButtons.Remove(button);
            }
            else
            {
                button.Button.image.color = Color.yellow;
                selectedButtons.Add(button);
            }
        });
    }
    public void Decide()
    {
        onSelect.Invoke();
        Destroy(gameObject);
    }
}
