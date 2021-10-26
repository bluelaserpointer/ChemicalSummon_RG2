using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPlayer : MovementControl3D
{
    //data
    public SBA_TraceRotation Rotater { get; protected set; }
    enum ModelState { Idle, Walk, Run, Jump }
    string[] stateNames = {"idleTrigger", "walkTrigger", "runTrigger", "jumpTrigger" };
    ModelState currentState;
    PlayableCharacterModel playerModel;
    public PlayableCharacterModel Model => playerModel;
    public Collider StepInCollider => Model.StepInCollider;
    public Collider InteractionCollider => Model.InteractionCollider;
    public InteractionListener AboutToInteractionObject { get; set; }
    public bool LockMovement { get; set; }
    public Animator Animator { get; protected set; }

    public PlayableCharacterModel SetModel(PlayableCharacterModel modelPrefab)
    {
        if (modelPrefab == null)
        {
            playerModel = null;
            controller = null;
            Animator = null;
            return null;
        }
        else
        {
            if (playerModel != null)
            {
                Destroy(playerModel);
            }
            playerModel = Instantiate(modelPrefab);
            controller = playerModel.GetComponent<CharacterController>();
            Animator = playerModel.GetComponent<Animator>();
            Animator.SetBool(stateNames[(int)(currentState = ModelState.Idle)], true);
            Rotater = playerModel.GetComponent<SBA_TraceRotation>();
            return playerModel;
        }
    }
    void Update()
    {
        if (playerModel == null)
            return;
        float xInput = Input.GetAxis("Horizontal"), yInput = Input.GetAxis("Vertical");
        if(LockMovement)
        {
            moveDirection = Vector3.zero;
            SetModelState(ModelState.Idle);
        }
        else
        {
            if (controller.isGrounded)
            {
                moveDirection = new Vector3(xInput, 0, yInput);
                if (moveDirection.Equals(Vector3.zero))
                {
                    SetModelState(ModelState.Idle);
                }
                else
                {
                    SetModelState(ModelState.Run);
                    Rotater.SetTarget(Quaternion.Euler(0, Mathf.Rad2Deg * Mathf.Atan2(xInput, yInput), 0));
                    if (!Rotater.IsBeforeReach)
                        Rotater.StartAnimation();
                }
                moveDirection *= speed;
                if (Input.GetButton("Jump"))
                    moveDirection.y = jumpSpeed;
            }
            else if (xInput == 0 && yInput == 0)
            {
                SetModelState(ModelState.Idle);
            }
            if (Input.GetButtonDown("Submit"))
            {
                if (AboutToInteractionObject != null)
                    AboutToInteractionObject.Interaction();
            }
        }
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
    void SetModelState(ModelState modelState)
    {
        if (currentState.Equals(modelState))
            return;
        Animator.SetBool(stateNames[(int)currentState], false);
        Animator.SetBool(stateNames[(int)(currentState = modelState)], true);
    }
    void GetModelState(ModelState modelState)
    {
        Animator.GetBool(stateNames[(int)modelState]);
    }
}
