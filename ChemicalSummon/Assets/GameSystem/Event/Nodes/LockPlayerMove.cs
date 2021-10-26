using UnityEngine;

public class LockPlayerMove : EventNode
{
    [SerializeField]
    bool lockCond = true;
    public override string PreferredGameObjectName => (lockCond ? "Lock" : "UnLock") + " player move";

    public override void Reach()
    {
        WorldManager.Player.LockMovement = lockCond;
        ProgressEvent();
    }
}
