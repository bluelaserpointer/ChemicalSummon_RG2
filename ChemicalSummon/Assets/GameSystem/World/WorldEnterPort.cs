using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class WorldEnterPort : MonoBehaviour
{
    [SerializeField]
    List<World> cameFromWorlds = new List<World>();
    [SerializeField]
    WorldEnterPortIDSO idso; 
    public bool AcceptsCameFromWorld(World world)
    {
        return cameFromWorlds.Contains(world);
    }
    public bool IsIDSO(WorldEnterPortIDSO idso)
    {
        return idso.Equals(this.idso);
    }
    private void OnValidate()
    {
        if (transform.root.Equals(this))
            return;
        string gameObjectName = "";
        if (idso != null)
            gameObjectName = "[" + idso.name + "]";
        if (cameFromWorlds.Count > 0)
        {
            gameObjectName += "<<" + cameFromWorlds[0].gameObject.name;
            if (cameFromWorlds.Count > 1)
                gameObjectName += "...";
        }
        gameObject.name = gameObjectName;
    }
}
