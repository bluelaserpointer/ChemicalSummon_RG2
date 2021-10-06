using UnityEngine;

[DisallowMultipleComponent]
public class EventInvoker : MonoBehaviour
{
    [SerializeField]
    Event eventInvokeOnAwake;
    [SerializeField]
    bool hideEventScreen;
    void Start()
    {
        if (hideEventScreen && eventInvokeOnAwake.gameObject.activeSelf)
            eventInvokeOnAwake.gameObject.SetActive(false);
        eventInvokeOnAwake.StartEvent();
    }
}
