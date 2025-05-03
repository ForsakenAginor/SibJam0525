using UnityEngine;
using NSpace;
using System;
using FMODUnity;
using System.Collections.Generic;

public interface ISprinter
{
    bool IsSprinting { get; }
}

[RequireComponent(typeof(CharacterController))]
public class PlayerControll : MonoBehaviour, IEntity, ISprinter
{

    [SerializeField] float moveSpeed;
    [SerializeField] float lookSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] Vector2 viewLimits;
    [SerializeField] float grav;
    [SerializeField] float crouchSpeedMult;
    [SerializeField] float sprintSpeedMult;
    [SerializeField] float crouchMult;
    [SerializeField] Transform viewer;
    [SerializeField] float ceilingMinHeight;
    [SerializeField] float staminaMin;
    [SerializeField] PlayerSoundController soundController;
    [SerializeField] UnityEngine.Events.UnityEvent<float> onMove;
    [SerializeField] string surfaceTypeParameterName;
    [SerializeField] EventReference walkSound;
    [SerializeField] EventReference runSound;
    [SerializeField] EventReference crouchSound;
    [SerializeField] EventReference jumpSound;
    [SerializeField] EventReference landSound;
    

    public System.Action onJump;
    public System.Action onLand;
    [SerializeField] UnityEngine.Events.UnityEvent<float> onSprint;
    [SerializeField] UnityEngine.Events.UnityEvent<float> onHeartBeat;


    CharacterController controller;
    ControllsBase input;
    [SerializeField]
    Vector3 moveDemand;
    Vector2 lookDemand;
    Vector3 move;
    float viewAngle;
    bool jump;
    bool inAir;
    bool crouch;
    bool sprint;
    float defaultHeight;
    private SprintController _sprintController;
    float lastStepTime;
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
        input.CharacterControlls.Crouch.started += (context) => { crouch = true; };
        input.CharacterControlls.Crouch.canceled += (context) => { crouch = false; };
        input.CharacterControlls.Sprint.started += (context) => { sprint = true; };
        input.CharacterControlls.Sprint.canceled += (context) => { sprint = false; };
    }

    // Update is called once per frame
    void Update()
    {
        bool ceiling = Physics.Raycast(viewer.transform.position, Vector3.up, ceilingMinHeight);
        if(Physics.Raycast(transform.TransformPoint(controller.center),Vector3.down, out RaycastHit ground, controller.height))
        {
            
            floor = ground.collider;
            if (speedPar > 0 && Time.time - lastStepTime > (1 / (speedPar + 1)) && soundController != null)
            {
                lastStepTime = Time.time;
                List<SoundEventParameter> parameters = new List<SoundEventParameter>();
                if (floor != null)
                {
                    Debug.Log(floor);
                    Surface surf = floor.GetComponentInParent<Surface>();
                    if (surf != null)
                    {
                        SoundEventParameter soundEventParameter = new SoundEventParameter();
                        soundEventParameter.name = surfaceTypeParameterName;
                        soundEventParameter.value = surf.surfaceType;
                        parameters.Add(soundEventParameter);
                    }
                }
                if (isCrouching) { soundController.PlaySound(crouchSound, transform.position, parameters.ToArray());  }
                else if (isRunning) soundController.PlaySound(runSound, transform.position, parameters.ToArray());
                else soundController.PlaySound(walkSound, transform.position, parameters.ToArray());
            }
        }
        if (jump && controller.isGrounded && !ceiling)
        {
            moveDemand.y = jumpHeight;
            onJump?.Invoke();
            if(soundController != null)
            {
                soundController.PlaySound(jumpSound, transform.position, null);
            }
            jump = false;
            


        }
        if (ceiling) crouch = true;
        

        if (controller.isGrounded)
        {
            if (inAir) {
                inAir = false; 
                onLand?.Invoke();
                if (soundController != null)
                {
                    List<SoundEventParameter> parameters = new List<SoundEventParameter>();
                    if (floor != null)
                    {
                        Surface surf = floor.GetComponentInParent<Surface>();
                        
                        if (surf != null)
                        {
                            SoundEventParameter soundEventParameter = new SoundEventParameter();
                            soundEventParameter.name = surfaceTypeParameterName;
                            soundEventParameter.value = surf.surfaceType;
                            parameters.Add(soundEventParameter);
                        }
                    }
                    soundController.PlaySound(landSound, transform.position, parameters.ToArray() );
                }
            }
            
            move = (Vector3.ClampMagnitude(transform.TransformDirection(moveDemand).With(y: 0), 1))
            * moveSpeed
            * (crouch ? crouchSpeedMult : 1)
            * (sprint && _sprintController.CanSprint ? sprintSpeedMult : 1);
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
            if(moveDemand.y>0) inAir = true;
            moveDemand.y += grav * Time.deltaTime;
            
        }
        else moveDemand.y = 0;
       

        if (move.sqrMagnitude > 0)
        {
            speedPar = move.magnitude / moveSpeed;
            onMove?.Invoke(speedPar);

        }
        else speedPar = 0;
    }

    public float speedPar { get; private set; }
    public Collider floor { get; private set; }
    public bool isRunning => sprint;
    public bool isCrouching => crouch;

    void CheckStamina(float stamina)
    {
        onSprint?.Invoke(stamina);
    }

    public void Init(SprintController sprintController)
    {
        _sprintController = sprintController != null ? sprintController : throw new ArgumentNullException(nameof(sprintController));
        _sprintController.onSprintValue = CheckStamina;
        controller = GetComponent<CharacterController>();
        input = GetComponentInParent<ControllInput>().controlls;
        InitControlls();
        //Cursor.lockState = CursorLockMode.Locked;
        viewAngle = 0;
        defaultHeight = controller.height;
    }
}
