using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideMessageGenerator : MonoBehaviour
{
    //inspector
    [SerializeField]
    Transform topMessagePositionAnchor, messageAnchorsParent;
    [SerializeField]
    [Min(0)]
    float goNextCooldown = 2;
    [SerializeField]
    float bottomMessageScale;

    //data
    List<SideMessage> generatedMessages = new List<SideMessage>();
    SideMessage topMessage;
    float cooldown;
    public void AddMessage(SideMessage message)
    {
        generatedMessages.Add(message);
        message.transform.SetParent(transform);
        GameObject anchorObject = new GameObject("Anchor", typeof(RectTransform));
        anchorObject.transform.SetParent(messageAnchorsParent);
        anchorObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, message.GetComponent<RectTransform>().sizeDelta.y * bottomMessageScale);
        message.transform.position = anchorObject.transform.position;
        message.transform.localScale = bottomMessageScale * Vector3.one;
        message.Tracer.SetTarget(anchorObject.transform);
        OnAnyAnchorChange();
    }
    private void Update()
    {
        if (cooldown < goNextCooldown)
        {
            cooldown += Time.deltaTime;
            if (cooldown < topMessage.Tracer.timeLength)
            {
                topMessage.transform.localScale = Vector3.Lerp(topMessage.transform.localScale, Vector3.one, cooldown / topMessage.Tracer.timeLength);
            }
        }
        else
        {
            if (topMessage != null)
            {
                Destroy(topMessage.gameObject);
                topMessage = null;
            }
            if (messageAnchorsParent.childCount > 0)
            {
                Destroy(messageAnchorsParent.GetChild(0).gameObject);
                topMessage = generatedMessages.RemoveFirst();
                OnAnyAnchorChange();
                topMessage.Tracer.SetTarget(topMessagePositionAnchor);
                topMessage.Tracer.StartAnimation();
                cooldown = 0;
            }
        }
    }
    public void ShowNextMessage()
    {
        cooldown = goNextCooldown;
    }
    private void OnAnyAnchorChange()
    {
        foreach(SideMessage message in generatedMessages)
        {
            message.Tracer.StartAnimation();
        }
    }
}
