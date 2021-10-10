using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider))]
public class GoToWorld : MonoBehaviour
{
    [SerializeField]
    World dst;

    public void Go()
    {
        //ChemicalSummonManager.CurrentManagerInstance.GotoWorld(dst);
    }
}
