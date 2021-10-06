using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class ProgressIcon : MonoBehaviour
{
    [SerializeField]
    Image lit, unlit;
    
    public void Lit(bool cond)
    {
        lit.gameObject.SetActive(cond);
        unlit.gameObject.SetActive(!cond);
    }
}
