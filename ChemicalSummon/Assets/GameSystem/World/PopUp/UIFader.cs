using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(CanvasGroup))]
public class UIFader : MonoBehaviour
{
    [SerializeField]
    CanvasGroup canvasGroup;
    [SerializeField]
    float fadeSpeedScale = 16F;
    public bool fadeIn;

    public CanvasGroup CanvasGroup => canvasGroup;

    private void Update()
    {
        canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, fadeIn ? 1 : 0, fadeSpeedScale * Time.deltaTime);
    }
}
