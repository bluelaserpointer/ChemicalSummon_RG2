using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class PopupWhenPlayerClose : MonoBehaviour
{
    [SerializeField]
    GameObject popUpPrefab;
    [SerializeField]
    TranslatableSentenceSO sentence;
    [SerializeField]
    Transform popUpPosition;

    CanvasGroup canvasGroup;

    public bool IsDisplaying { get; protected set; }
    private void Start()
    {
        GameObject generatedPopup = Instantiate(popUpPrefab);
        generatedPopup.transform.position = (popUpPosition ?? transform).position;
        canvasGroup = generatedPopup.GetComponentInChildren<CanvasGroup>();
        canvasGroup.GetComponentInChildren<Text>().text = sentence;
    }
    void Update()
    {
        PlayableCharacter player = WorldManager.Player.TargetModel;
        if(Vector3.Distance(transform.position, player.transform.position) < 3)
        {
            IsDisplaying = true;
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 1, 16F * Time.deltaTime);
        }
        else
        {
            IsDisplaying = false;
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0, 16F * Time.deltaTime);
        }
    }
}
