using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEvent : InteractionListener
{
    [SerializeField]
    Event invokeEvent;
    [Tooltip("Inputs this value will automaticaly use ButtonPopUp")]
    [SerializeField]
    TranslatableSentenceSO defaultPopUpSentence;

    private void Awake()
    {
        invokeEvent.OnEventFinish.AddListener(() => popUpGenerator.ShowPopUp());
        OnInteraction.AddListener(StartEvent);
        if(popUpGenerator == null && defaultPopUpSentence != null)
        {
            PopUpButton popUpButton = gameObject.AddComponent<PopUpButton>();
            popUpGenerator = popUpButton;
            popUpButton.SetTranslatedText(defaultPopUpSentence);
            popUpButton.OnClick.AddListener(StartEvent);
        }
    }
    public void StartEvent()
    {
        WorldManager.Player.AboutToInteractionObject = null;
        popUpGenerator.HidePopUp();
        invokeEvent.StartEvent();
    }
}
