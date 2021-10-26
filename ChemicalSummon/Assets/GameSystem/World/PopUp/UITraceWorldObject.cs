using UnityEngine;

[DisallowMultipleComponent]
public class UITraceWorldObject : MonoBehaviour
{
    [SerializeField]
    new Camera camera;
    public Transform target;
    public bool destroyWhenNoTarget = true;
    public bool destroyWhenNoCamera;

    public Camera Camera => camera != null ? camera : Camera.main;
    private void Update()
    {
        if (target == null)
        {
            if (destroyWhenNoTarget)
                Destroy(gameObject);
            return;
        }
        if (Camera == null)
        {
            if (destroyWhenNoCamera)
                Destroy(gameObject);
            return;
        }
        if (gameObject.activeInHierarchy)
        {
            transform.position = Camera.main.WorldToScreenPoint(target.position);
        }
    }
}
