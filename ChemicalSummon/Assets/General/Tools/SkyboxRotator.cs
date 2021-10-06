using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Skybox))]
public class SkyboxRotator : MonoBehaviour
{
    public float rotateSpeed = 0.05f;

    //data
    float num;
    // Update is called once per frame
    void Update()
    {
        num = RenderSettings.skybox.GetFloat("_Rotation");
        RenderSettings.skybox.SetFloat("_Rotation", num + rotateSpeed);
    }
}
