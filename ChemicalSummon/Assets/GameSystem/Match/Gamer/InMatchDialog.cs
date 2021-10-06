using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class InMatchDialog : MonoBehaviour
{
    [SerializeField]
    CanvasGroup dialogGroup;
    [SerializeField]
    Text text;

    //data
    float lingerTime;
    UnityAction onDisappear;

    public void SetText(string str, UnityAction onDisappear = null)
    {
        text.text = str;
        lingerTime = str.Length * 0.125F + 1;
        this.onDisappear = onDisappear;
        gameObject.SetActive(true);
    }
    void Update()
    {
        if(lingerTime > 0)
        {
            if ((lingerTime -= Time.deltaTime) <= 0)
            {
                gameObject.SetActive(false);
                if(onDisappear != null)
                    onDisappear.Invoke();
            }
            else 
            {
                dialogGroup.alpha = (lingerTime < 1) ? lingerTime / 1 : 1;
                if (!gameObject.activeSelf)
                    gameObject.SetActive(true);
            }
        }
        else if(gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }
}
