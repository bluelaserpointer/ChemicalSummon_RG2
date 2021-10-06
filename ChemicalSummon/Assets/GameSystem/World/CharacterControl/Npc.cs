using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Npc : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    [SerializeField]
    SBA_TraceRotation rotater;
    [SerializeField]
    bool lookAtPlayer;

    private void Update()
    {
        if(lookAtPlayer)
        {
            PlayableCharacter player = WorldManager.Player.TargetModel;
            if(player != null)
            {
                Vector3 playerVecXZ = new Vector3(player.transform.position.x - transform.position.x, 0, player.transform.position.z - transform.position.z);
                if(playerVecXZ.sqrMagnitude < 1.5)
                {
                    rotater.SetTarget(Quaternion.LookRotation(playerVecXZ));
                    if (!rotater.IsBeforeReach)
                        rotater.StartAnimation();
                }
            }
        }
    }
}
