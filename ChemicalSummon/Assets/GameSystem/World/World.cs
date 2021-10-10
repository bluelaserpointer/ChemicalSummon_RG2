using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class World : MonoBehaviour
{
    [SerializeField]
    AudioClip bgm;
    //[SerializeField]

    public AudioClip BGM => bgm;
    public void Init()
    {
        
    }
}
