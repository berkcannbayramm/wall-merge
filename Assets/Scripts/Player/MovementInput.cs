
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementInput : MonoBehaviour {

	[SerializeField] private VariableJoystick variableJoystick;
    [SerializeField] private FlowController flowController;
    public float Velocity;
    [Space]

	[SerializeField] private Camera cam;
	[SerializeField] private CharacterController controller;
	[SerializeField] private Animator anim;

    [Header("Animation Smoothing")]
    [Range(0, 1f)]
    [SerializeField] private float HorizontalAnimSmoothTime = 0.2f;
    [Range(0, 1f)]
    [SerializeField] private float VerticalAnimTime = 0.2f;
    [Range(0,1f)]
    [SerializeField] private float StartAnimTime = 0.3f;
    [Range(0, 1f)]
    [SerializeField] private float StopAnimTime = 0.15f;
    [SerializeField] private float verticalVel;

    private float InputX;
    private float InputZ;
    private Vector3 desiredMoveDirection;
    private bool blockRotationPlayer;
    private float desiredRotationSpeed = 0.1f;
    private float Speed;
    private float allowPlayerRotation = 0.1f;
    private bool isGrounded;

    private Vector3 moveVector;
    private IMovement _movement;
    
    private static readonly int Blend = Animator.StringToHash("Blend");

    private void Awake()
    {
        _movement = GetComponent<IMovement>();

        if (_movement == null)
        {
	        Debug.LogError($"{gameObject.name}s {_movement} is null.");
        }
    }
	
	void Update () {
        
        if (!flowController.CanMovePlayer)
        {
            moveVector = Vector3.zero;
            controller.Move(moveVector);
            
            Speed = 0;
            anim.SetFloat(Blend, Speed, StopAnimTime, Time.deltaTime);
            return;
        }

		InputMagnitude ();

        isGrounded = controller.isGrounded;
        if (isGrounded)
        {
            verticalVel -= 0;
        }
        else
        {
            verticalVel -= 1;
        }
        moveVector = new Vector3(0, verticalVel * .2f * Time.deltaTime, 0);
        controller.Move(moveVector);
        
    }
    void PlayerMoveAndRotation() {

		InputX = variableJoystick.Horizontal;
		InputZ = variableJoystick.Vertical;

		var canTransform = cam.transform;
		var forward = canTransform.forward;
		var right = canTransform.right;

		forward.y = 0f;
		right.y = 0f;

		forward.Normalize ();
		right.Normalize ();

		desiredMoveDirection = forward * InputZ + right * InputX;

		if (blockRotationPlayer != false) return;
		
		_movement.Rotation();
		controller.Move(desiredMoveDirection * (Velocity * Time.deltaTime));
    }

	void InputMagnitude() {
        InputX = variableJoystick.Horizontal;
        InputZ = variableJoystick.Vertical;

        Speed = new Vector2(InputX, InputZ).sqrMagnitude;

		if (Speed > allowPlayerRotation) {
			anim.SetFloat (Blend, Speed, StartAnimTime, Time.deltaTime);
			PlayerMoveAndRotation ();
		} else if (Speed < allowPlayerRotation) {
			anim.SetFloat (Blend, Speed, StopAnimTime, Time.deltaTime);
        }
    }
}
