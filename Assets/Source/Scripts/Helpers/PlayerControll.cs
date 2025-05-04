using UnityEngine;
using NSpace;
using System;
using FMODUnity;

public interface ISprinter
{
    bool IsSprinting { get; }
}

[RequireComponent(typeof(CharacterController))]
public class PlayerControll : MonoBehaviour, IEntity, ISprinter
{
    [SerializeField] private EventReference _soundWalk;
    [SerializeField] private EventReference _soundSprint;
    [SerializeField] private EventReference _soundCrouch;
    [SerializeField] float moveSpeed;
    [SerializeField] float lookSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] Vector2 viewLimits;
    [SerializeField] float grav;
    [SerializeField] float crouchSpeedMult;
    [SerializeField] float sprintSpeedMult;
    [SerializeField] float crouchMult;
    [SerializeField] Transform viewer;
    [SerializeField] UnityEngine.Events.UnityEvent<float> onMove;


    CharacterController controller;
    ControllsBase input;
    [SerializeField]
    Vector3 moveDemand;
    Vector2 lookDemand;
    Vector3 move;
    float viewAngle;
    bool jump;
    bool crouch;
    bool sprint;
    float defaultHeight;
    private SprintController _sprintController;
    private bool _isCrouchInput;

    public bool IsSprinting => sprint;

    void InitControlls()
    {
        input.CharacterControlls.Move.started += (context) => { Vector2 inputVector = context.ReadValue<Vector2>(); moveDemand.x = inputVector.x; moveDemand.z = inputVector.y; };
        input.CharacterControlls.Move.performed += (context) => { Vector2 inputVector = context.ReadValue<Vector2>(); moveDemand.x = inputVector.x; moveDemand.z = inputVector.y; };
        input.CharacterControlls.Move.canceled += (context) => { Vector2 inputVector = context.ReadValue<Vector2>(); moveDemand.x = inputVector.x; moveDemand.z = inputVector.y; };
        input.CommonControlls.MouseMove.started += (context) => { lookDemand = context.ReadValue<Vector2>(); };
        input.CommonControlls.MouseMove.performed += (context) => { lookDemand = context.ReadValue<Vector2>(); };
        input.CommonControlls.MouseMove.canceled += (context) => { lookDemand = context.ReadValue<Vector2>(); };
        input.CharacterControlls.Jump.started += (context) => { jump = true; };
        input.CharacterControlls.Jump.canceled += (context) => { jump = false; };
        input.CharacterControlls.Crouch.started += (context) => { crouch = true; _isCrouchInput = true; };
        input.CharacterControlls.Crouch.canceled += (context) => { crouch = false; _isCrouchInput = false; };
        input.CharacterControlls.Sprint.started += (context) => { sprint = true; };
        input.CharacterControlls.Sprint.canceled += (context) => { sprint = false; };
    }

    // Update is called once per frame
    void Update()
    {
        bool ceiling = Physics.Raycast(viewer.transform.position, Vector3.up, 0.5f);
        if (jump && controller.isGrounded && !ceiling)
        {
            moveDemand.y = jumpHeight;
            jump = false;
        }
        if (ceiling) crouch = true;
        if (controller.isGrounded)
        {
            move = Vector3.Lerp(move, (Vector3.ClampMagnitude(transform.TransformDirection(moveDemand).With(y: 0), 1))
            * moveSpeed
            * (crouch ? crouchSpeedMult : 1)
            * (sprint && _sprintController.CanSprint ? sprintSpeedMult : 1), Time.deltaTime * 5);

            bool canStandUp = !Physics.Raycast(viewer.transform.position, Vector3.up, defaultHeight * (1 - crouchMult));

            if (_isCrouchInput == false && canStandUp)
            {
                crouch = false;
            }
        }
        controller.Move((move + Vector3.up * moveDemand.y) * Time.deltaTime);
        controller.transform.rotation *= Quaternion.AngleAxis(lookDemand.x * lookSpeed * Time.deltaTime, Vector3.up);
        viewAngle = Mathf.Clamp(viewAngle - lookDemand.y * lookSpeed * Time.deltaTime, viewLimits.x, viewLimits.y); ;
        viewer.transform.localRotation = Quaternion.AngleAxis(viewAngle, Vector3.right);

        if (crouch)
        {
            controller.height = Mathf.Lerp(controller.height, defaultHeight * crouchMult, Time.deltaTime * 10);
        }
        else controller.height = Mathf.Lerp(controller.height, defaultHeight, Time.deltaTime * 10);
        viewer.transform.localPosition = controller.center + Vector3.up * (controller.height / 2 - 0.1f);
        if (!controller.isGrounded)
        {
            moveDemand.y += grav * Time.deltaTime;
        }
        else moveDemand.y = 0;

        if (move.sqrMagnitude > 0)
        {
            onMove?.Invoke(move.magnitude / moveSpeed);
        }
    }

    public void Init(SprintController sprintController)
    {
        _sprintController = sprintController != null ? sprintController : throw new ArgumentNullException(nameof(sprintController));
        controller = GetComponent<CharacterController>();
        input = GetComponentInParent<ControllInput>().controlls;
        InitControlls();
        //Cursor.lockState = CursorLockMode.Locked;
        viewAngle = 0;
        defaultHeight = controller.height;
    }
}
