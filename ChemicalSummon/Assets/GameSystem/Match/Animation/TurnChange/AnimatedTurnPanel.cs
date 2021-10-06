using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Animator))]
public class AnimatedTurnPanel : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    [SerializeField]
    Text turnTypeText;
    public bool IsPlaying => animator.GetBool("Play");
    public UnityEvent animationStopped;
    public void Stop() //referenced by animation
    {
        animator.SetBool("Play", false);
    }
    public void Play()
    {
        animator.SetBool("Play", true);
        turnTypeText.text = MatchManager.TurnTypeToString(MatchManager.CurrentTurnType);
    }
}
