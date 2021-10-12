using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class WorldExitPort : AbstractWorldEventObject
{
    [SerializeField]
    World dst;
    [SerializeField]
    WorldEnterPortIDSO idso;

    public override void Submit()
    {
    }

    protected override void DoEvent()
    {
        WorldManager.EnterWorld(dst, idso);
    }
    private void OnValidate()
    {
        if (transform.root.Equals(this))
            return;
        string gameObjectName = ">>" + (dst == null ? "?" : dst.gameObject.name);
        if (idso != null)
            gameObjectName += " - " + idso.name;
        gameObject.name = gameObjectName;
    }
}
