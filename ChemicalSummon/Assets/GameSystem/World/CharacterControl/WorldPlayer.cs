using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPlayer : MovementControl3D
{
    [SerializeField]
    PlayableCharacter playerModel;
    [SerializeField]
    CanvasGroup popUpCanvas;

    //data
    SBA_TraceRotation rotater;
    enum ModelState { Idle, Walk, Run, Jump }
    string[] stateNames = {"idleTrigger", "walkTrigger", "runTrigger", "jumpTrigger" };
    ModelState currentState;
    public PlayableCharacter TargetModel
    {
        get => playerModel;
        set
        {
            if (value == null)
            {
                playerModel = null;
                controller = null;
                Animator = null;
            }
            else
            {
                playerModel = value;
                controller = playerModel.GetComponent<CharacterController>();
                Animator = playerModel.GetComponent<Animator>();
                rotater = playerModel.GetComponent<SBA_TraceRotation>();
            }
        }
    }
    public Collider StepInCollider => TargetModel.StepInCollider;
    public Collider InteractionCollider => TargetModel.InteractionCollider;
    public AbstractWorldEventObject InInteractionColliderEventObject { get; set; }
    public AbstractWorldEventObject OccupyingMovementEventObject { get; set; }
    public bool IsDoingInput { get; set; }
    public Animator Animator { get; protected set; }
    // Start is called before the first frame update
    void Start()
    {
        TargetModel = playerModel; //force update
        Animator.SetBool(stateNames[(int)(currentState = ModelState.Idle)], true);
    }
    // Update is called once per frame
    void Update()
    {
        if (playerModel == null)
            return;
        transform.position = playerModel.transform.position + Vector3.up * controller.height;
        float xInput = Input.GetAxis("Horizontal"), yInput = Input.GetAxis("Vertical");
        if(OccupyingMovementEventObject != null)
        {
            moveDirection = Vector3.zero;
            SetModelState(ModelState.Idle);
            if (Input.GetButtonDown("Submit"))
            {
                OccupyingMovementEventObject.Submit();
            }
        }
        else if (!IsDoingInput)
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
                    rotater.SetTarget(Quaternion.Euler(0, Mathf.Rad2Deg * Mathf.Atan2(xInput, yInput), 0));
                    if (!rotater.IsBeforeReach)
                        rotater.StartAnimation();
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
                if (InInteractionColliderEventObject != null)
                    InInteractionColliderEventObject.InvokeEvent();
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
