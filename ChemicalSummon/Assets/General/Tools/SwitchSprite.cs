using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[DisallowMultipleComponent]
public class SwitchSprite : MonoBehaviour
{
    public Sprite unlitSprite;
    public Sprite litSprite;
    [SerializeField]
    bool isLit;

    //data
    Image image;
    public bool IsLit {
        get => isLit;
        protected set
        {
            if(isLit != value)
            {
                isLit = value;
                UpdateSprite();
            }
        }
    }

    private void Awake()
    {
        image = GetComponent<Image>();
        UpdateSprite();
    }

    public void Lit()
    {
        IsLit = true;
    }

    public void UpdateSprite()
    {
        if (IsLit)
        {
            image.sprite = litSprite;
        }
        else
        {
            image.sprite = unlitSprite;
        }
    }
    public void Toggle()
    {
        IsLit = !IsLit;
    }
}
