using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class WorldExitPort : StepInListener
{
    [SerializeField]
    WorldHeader dst;
    [SerializeField]
    WorldEnterPortIDSO idso;

    private void Awake()
    {
        OnStepIn.AddListener(() =>
        {
            WorldManager.EnterWorld(dst, idso);
        });
    }
    private void OnValidate()
    {
        if (transform.root.Equals(this))
            return;
        string gameObjectName = ">>" + (dst == null ? "?" : dst.World.name);
        if (idso != null)
            gameObjectName += " - " + idso.name;
        gameObject.name = gameObjectName;
    }
}
