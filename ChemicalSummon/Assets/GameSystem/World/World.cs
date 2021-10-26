using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class World : MonoBehaviour
{
    [SerializeField]
    new TranslatableSentence name;
    [SerializeField]
    AudioClip bgm;
    [SerializeField]
    Transform enterPortsParent;

    public AudioClip BGM => bgm;
    public WorldEnterPort FindEnterPort(WorldHeader lastWorldHeader)
    {
        WorldEnterPort port = enterPortsParent.FindComponentInChildren<WorldEnterPort>(port => port.AcceptsCameFromWorld(lastWorldHeader));
        if(port == null)
        {
            Debug.LogWarning("\"" + name + "\" does not accepts " + 
                (lastWorldHeader == null ? "initial join" : ("came from another world \"" + lastWorldHeader.World.gameObject.name + "\"")));
        }
        return port;
    }
    public WorldEnterPort FindEnterPort(WorldEnterPortIDSO idso)
    {
        WorldEnterPort port = enterPortsParent.FindComponentInChildren<WorldEnterPort>(port => port.IsIDSO(idso));
        if (port == null)
        {
            Debug.LogWarning("\"" + name + "\" does not have enter port id \"" + idso.name + "\"");
        }
        return port;
    }
}
