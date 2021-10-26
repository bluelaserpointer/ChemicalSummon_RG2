using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 监视玩家与该物体互动
/// </summary>
[DisallowMultipleComponent]
public class InteractionListener : MonoBehaviour
{
    [SerializeField]
    UnityEvent onInteraction;
    [SerializeField]
    protected PopUpGenerator popUpGenerator;

    public UnityEvent OnInteraction => onInteraction;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.Equals(WorldManager.Player.InteractionCollider))
        {
            WorldManager.Player.AboutToInteractionObject = this; //TODO: priority
            popUpGenerator.ShowPopUp();
        }
    }
    public void OnTriggerExit(Collider collider)
    {
        if (collider.Equals(WorldManager.Player.InteractionCollider))
        {
            if (Equals(WorldManager.Player.AboutToInteractionObject))
            {
                WorldManager.Player.AboutToInteractionObject = null;
                popUpGenerator.HidePopUp();
            }
        }
    }
    public void Interaction()
    {
        OnInteraction.Invoke();
    }
}
