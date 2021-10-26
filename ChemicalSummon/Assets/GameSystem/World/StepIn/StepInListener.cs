using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ������ҽ��������
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public class StepInListener : MonoBehaviour
{
    [SerializeField]
    UnityEvent onStepIn;

    public UnityEvent OnStepIn => onStepIn;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.Equals(WorldManager.Player.StepInCollider))
        {
            StepIn();
        }
    }
    public void StepIn()
    {
        OnStepIn.Invoke();
    }
}
