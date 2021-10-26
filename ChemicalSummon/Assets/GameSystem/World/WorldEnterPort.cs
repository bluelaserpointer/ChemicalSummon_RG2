using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class WorldEnterPort : MonoBehaviour
{
    [SerializeField]
    List<WorldHeader> cameFromWorlds = new List<WorldHeader>();
    [SerializeField]
    WorldEnterPortIDSO idso; 
    public bool AcceptsCameFromWorld(WorldHeader worldHeader)
    {
        return cameFromWorlds.Contains(worldHeader);
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
            gameObjectName += "<<" + (cameFromWorlds[0]?.World?.name ?? "(?missing)");
            if (cameFromWorlds.Count > 1)
                gameObjectName += "...";
        }
        gameObject.name = gameObjectName;
    }
}
