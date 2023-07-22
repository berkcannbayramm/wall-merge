using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectorMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    public float movSpeed = 3;
    public float rotSpeed = 2;
    public float rotationLerp;
    public float distanceToTurn = 1f;

    [Space]

    [Header("Booleans")]
    public bool isActive;
    public bool movementMode;
    public bool rotationMode;
    public bool isGoingRight;
    public bool isMoving;
    private bool activation;

    private Vector3 originPos;
    private Vector3 targetPos;

    [Space]

    [Header("Public References")]
    public WallMerge player;
    private RaySearch search;
    public Transform pivot;
    public Transform lineRef1, lineRef2;
    [SerializeField] private VariableJoystick variableJoystick;
    [SerializeField] private FlowController flowController;

    private Vector3 savedNormal;


    public void SetPosition(Vector3 orig, Vector3 target, float lerp, RaySearch ray, bool nextCornerIsRight, Vector3 normal)
    {
        transform.forward = normal;
        transform.position = Vector3.Lerp(orig, target, lerp);
        search = ray;
        originPos = nextCornerIsRight ? orig : target;
        targetPos = nextCornerIsRight ? target : orig;
        movementMode = true;
    }

    public void ExitMerge()
    {
        var objectTransform = transform;
        objectTransform.parent = null;
        isActive = false;
        movementMode = false;
        rotationMode = false;
        activation = false;
        flowController.CanMove = false;

        var playerTransform = player.transform;
        var forward = objectTransform.forward;
        
        playerTransform.position = new Vector3(objectTransform.position.x, -0.05f , objectTransform.position.z - 2f) - (forward * .5f);

        Vector3 playerFinalPos = playerTransform.position + (forward);

        player.Transition(false, playerFinalPos, forward);
    }

    private void Update()
    {

        if (!flowController.CanMove) return;

        float axis = variableJoystick.Horizontal;

        if (movementMode)
        {
            transform.position = Vector3.MoveTowards(transform.position, axis > 0 ? targetPos : originPos, Mathf.Abs(axis) * Time.deltaTime * movSpeed);

            if (axis != 0 && !activation)
                activation = true;
            if (!activation)
                return;
        }
    }

    public void SetPlayerPos()
    {
        player.gameObject.SetActive(false);
        Transform playerTransform;
        
        (playerTransform = player.transform).SetParent(transform);
        playerTransform.localPosition = Vector3.zero -Vector3.up * 2;
    }
}
