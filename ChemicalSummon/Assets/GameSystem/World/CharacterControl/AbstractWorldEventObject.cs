using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 世界内的含事件物体
/// </summary>
public abstract class AbstractWorldEventObject : MonoBehaviour
{
    public enum EventType { PressInteract, StepIn }
    [SerializeField]
    EventType eventType = EventType.PressInteract;
    [SerializeField]
    Collider eventCollider;
    [SerializeField]
    UnityEvent OnEvent;

    //data
    GameObject generatedPopUp;
    CanvasGroup popUpCanvasGroup;
    [SerializeField]
    TranslatableSentenceSO popUpSentence;
    [SerializeField]
    Vector3 popUpPosCorrect = new Vector3(0, 50, 0);
    public Collider EventCollider => eventCollider;
    public bool OccupyMovement { get; protected set; }
    private void Start()
    {
        generatedPopUp = Instantiate(Resources.Load<GameObject>("PopUp"), WorldManager.MainCanvas.transform);
        generatedPopUp.GetComponent<Button>().onClick.AddListener(() => { if(popUpCanvasGroup.alpha > 0) InvokeEvent(); });
        popUpCanvasGroup = generatedPopUp.GetComponentInChildren<CanvasGroup>();
        popUpCanvasGroup.GetComponentInChildren<Text>().text = popUpSentence;
        popUpCanvasGroup.alpha = 0;
        generatedPopUp.gameObject.SetActive(false);
    }
    private void Update()
    {
        generatedPopUp.transform.position = Camera.main.WorldToScreenPoint(transform.position) + popUpPosCorrect;
        if (Equals(WorldManager.Player.InInteractionColliderEventObject))
            popUpCanvasGroup.alpha = Mathf.MoveTowards(popUpCanvasGroup.alpha, 1, 16F * Time.deltaTime);
        else if (popUpCanvasGroup.alpha > 0)
            popUpCanvasGroup.alpha = Mathf.MoveTowards(popUpCanvasGroup.alpha, 0, 16F * Time.deltaTime);
        else
            generatedPopUp.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        switch(eventType)
        {
            case EventType.StepIn:
                if (other.Equals(WorldManager.Player.StepInCollider))
                {
                    InvokeEvent();
                }
                break;
            case EventType.PressInteract:
                if (other.Equals(WorldManager.Player.InteractionCollider))
                {
                    WorldManager.Player.InInteractionColliderEventObject = this; //TODO: priority
                    generatedPopUp.gameObject.SetActive(true);
                }
                break;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        switch (eventType)
        {
            case EventType.StepIn:
                break;
            case EventType.PressInteract:
                if (other.Equals(WorldManager.Player.InteractionCollider))
                {
                    if (Equals(WorldManager.Player.InInteractionColliderEventObject))
                    {
                        WorldManager.Player.InInteractionColliderEventObject = null;
                    }
                }
                break;
        }
    }
    public void InvokeEvent()
    {
        OnEvent.Invoke();
        DoEvent();
    }
    protected abstract void DoEvent();
    public abstract void Submit();
}
