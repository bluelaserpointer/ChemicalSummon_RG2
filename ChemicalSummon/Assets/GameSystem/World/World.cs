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
    public WorldEnterPort FindEnterPort(World lastWorld)
    {
        WorldEnterPort port = enterPortsParent.FindComponentInChildren<WorldEnterPort>(port => port.AcceptsCameFromWorld(lastWorld));
        if(port == null)
        {
            Debug.LogWarning("\"" + name + "\" does not accepts " + 
                (lastWorld == null ? "initial join" : ("came from another world \"" + lastWorld.name + "\"")));
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
