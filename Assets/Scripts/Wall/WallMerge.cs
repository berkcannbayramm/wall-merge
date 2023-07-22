using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;
using Cinemachine;
using UnityEngine.Rendering.Universal;
using System;
using BBGameStudios.Managers;
using System.Collections;

public class WallMerge : MonoBehaviour
{
    [Header("Other Scripts")]
    [SerializeField] private UIController uiController;
    [SerializeField] private CollectManager collectManager;
    [SerializeField] private PortalController portalController;

    [Space,Header("Player")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private MovementInput playerMovement;
    private Vector3 closestCorner;
    private Vector3 nextCorner;
    private Vector3 previousCorner;
    private Vector3 chosenCorner;
    private float playerZScale;

    [Space,Header("Parameters")]
    public float transitionTime = .8f;

    [Space,Header("References")]
    [SerializeField] private ProjectorMovement decalMovement;
    [SerializeField] private Transform frameQuad;
    [SerializeField] private ParticleSystem mergePS, exitPS;
    [SerializeField] private Camera camera;

    [Space,Header("Frame Settings")]
    public Color frameLitColor;

    [Space,Header("Post Processing")]
    public Volume zoomVolume;

    [SerializeField] private FlowController flowController;
    [SerializeField] private ProjectorMovement projectorMovement;
    [SerializeField] private DecalProjector decalProjector;

    public Action<WallType> PressButtonEvent;

    [field:SerializeField]public List<ObjectTypes> ObjectTypes { get; set; }

    private Renderer frameRenderer;
    private CinemachineImpulseSource impulseSource;
    private bool _triggerPortalController;
    private WaitForSeconds _wait = new WaitForSeconds(.2f);


    private void Start()
    {
        playerZScale = transform.GetChild(0).localScale.z;
        frameRenderer = frameQuad.GetComponent<Renderer>();
        impulseSource = camera.GetComponent<CinemachineImpulseSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInChildren<RaySearch>() != null)
        {
            if (!uiController.CanMerge)
            {
                uiController.ShowMergeButton();
                return;
            }
        }
        if (other.TryGetComponent<PressButton>(out PressButton pressButton))
        {
            if (pressButton.IsPressed) return;
            pressButton.IsPressed = true;
            pressButton.RotateObject();
            PressButtonEvent?.Invoke(pressButton.TargetWallType);
        }
        if (other.CompareTag("Fall"))
        {
            GameManager.instance.LevelManager.ReloadScene();
        }
        if (other.CompareTag("Finish"))
        {
            FinishLevel();
        }
        if(other.TryGetComponent<PortalController>(out PortalController portal))
        {
            if(portal.PortalState == PortalStates.NotReady)
            {
                if (_triggerPortalController) return;
                _triggerPortalController = true;
                portal.NeedItem();
                portalController.CheckNeedItem();
            }
        }
        if (other.TryGetComponent<CrystalSpawner>(out CrystalSpawner crystalSpawner) || other.CompareTag("Crystals"))
        {
            PortalObjectsSO portalObject;
            if (crystalSpawner != null)
            {
                portalObject = crystalSpawner.PortalObjectsSO;
            }
            else
            {
                portalObject = other.GetComponent<CrystalSpawner>().PortalObjectsSO;
            }
            ObjectTypes.Add(portalObject.ObjectTypes);

            collectManager.CrystalCounts[portalObject.ObjectID]++;

            if (crystalSpawner != null) crystalSpawner.CollectCrystal();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponentInChildren<RaySearch>() != null)
        {
            if (!uiController.CanMerge)
            {
                uiController.HideMergeButtons();
                return;
            }
        }
        if (other.TryGetComponent<PortalController>(out PortalController portal))
        {
            if (portal.PortalState == PortalStates.NotReady)
            {
                _triggerPortalController = false;
            }
        }
    }

    private void FinishLevel()
    {
        flowController.CanMovePlayer = false;
        uiController.HideJoystick();
        DOVirtual.Float(zoomVolume.weight, 1, .7f, ZoomVolume).OnComplete(() => DOVirtual.Float(zoomVolume.weight, 0, .3f, ZoomVolume).OnComplete(() => GameManager.instance.LevelManager.NextLevel()));
    }

    public void MergePlayer()
    {
        if (Physics.Raycast(transform.position + (Vector3.up * .1f), transform.forward, out RaycastHit hit, 1))
        {
            if (hit.transform.GetComponentInChildren<RaySearch>() != null)
            {
                //store raycasted object's RaySearch component
                RaySearch search = hit.transform.GetComponentInChildren<RaySearch>();

                //create a new list of all the corner positions
                List<Vector3> cornerPoints = new List<Vector3>();

                for (int i = 0; i < search.cornerPoints.Count; i++)
                    cornerPoints.Add(search.cornerPoints[i].position);

                //find the closest corner position and index
                closestCorner = GetClosestPoint(cornerPoints.ToArray(), hit.point);
                int index = search.cornerPoints.FindIndex(x => x.position == closestCorner);

                //determine the adjacent corners
                nextCorner = (index < search.cornerPoints.Count - 1) ? search.cornerPoints[index + 1].position : search.cornerPoints[0].position;
                previousCorner = (index > 0) ? search.cornerPoints[index - 1].position : search.cornerPoints[search.cornerPoints.Count - 1].position;

                //choose a corner to be the target
                chosenCorner = Vector3.Dot((closestCorner - hit.point), (nextCorner - hit.point)) > 0 ? previousCorner : nextCorner;
                bool nextCornerIsRight = isRightSide(-hit.normal, chosenCorner - closestCorner, Vector3.up);

                //find the distance from the origin point
                float distance = Vector3.Distance(closestCorner, chosenCorner);
                float playerDis = Vector3.Distance(chosenCorner, hit.point);

                //quick fix so that we don't allow the player to start in a corner;
                if (playerDis > (distance - decalMovement.distanceToTurn))
                    playerDis = distance - decalMovement.distanceToTurn;
                if (playerDis < decalMovement.distanceToTurn)
                    playerDis = decalMovement.distanceToTurn;

                //find it's normalized position in the distance of the origin and target
                float positionLerp = Mathf.Abs(distance - playerDis) / ((distance + playerDis) / 2);

                //start the MovementScript
                decalMovement.SetPosition(closestCorner, chosenCorner, positionLerp, search, nextCornerIsRight, hit.normal);

                //transition logic
                Transition(true, Vector3.Lerp(closestCorner, chosenCorner, positionLerp), hit.normal);
            }
        }
    }

    public void Transition(bool merge, Vector3 point, Vector3 normal)
    {
        flowController.CanMove = false;
        Vector3 finalNormal = merge ? -normal : normal;
        Vector3 finalPosition = merge ? point - new Vector3(0, .9f, 0) : point;
        //Vector3 finalPosition = point;
        string animatorStatus = merge ? "turn" : "normal";
        float scale = merge ? .01f : playerZScale;
        float finalTransition = merge ? .5f : .3f;

        if (merge == true)
            FrameMovement(normal, finalPosition, finalTransition);

        transform.forward = finalNormal;
        playerAnimator.SetTrigger(animatorStatus);
        PlayerActivation(merge);
        StartCoroutine(WallMergePlayer(merge, finalPosition, scale, finalTransition));

        if (merge)
            DOVirtual.Float(zoomVolume.weight, 1, .7f, ZoomVolume).SetId(this).OnComplete(() => DOVirtual.Float(zoomVolume.weight, 0, .3f, ZoomVolume).SetId(this));
    }

    void PlayerActivation(bool active)
    {
        if (active == true)
        {
            playerMovement.enabled = false;
            //playerController.enabled = false;
        }
        else
        {
            playerMovement.gameObject.SetActive(true);
        }
    }

    void FrameMovement(Vector3 normal, Vector3 finalPosition, float finalTransition)
    {
        var playerTransform = transform;
        frameQuad.position = playerTransform.position + new Vector3(0, .85f, 0) - (playerTransform.forward * .5f);
        frameQuad.forward = -normal;
        frameRenderer.material.color = Color.clear;
        frameRenderer.material.DOColor(frameLitColor, 1f).SetDelay(.3f);
        frameQuad.DOMove(finalPosition + new Vector3(0, .85f, 0) - (transform.forward * .05f), finalTransition).SetEase(Ease.InBack).SetDelay(.2f);
    }

    Vector3 GetClosestPoint(Vector3[] points, Vector3 currentPoint)
    {
        Vector3 pMin = Vector3.zero;
        float minDist = Mathf.Infinity;

        foreach (Vector3 p in points)
        {
            float dist = Vector3.Distance(p, currentPoint);
            if (dist < minDist)
            {
                pMin = p;
                minDist = dist;
            }
        }
        return pMin;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawRay(transform.position + (Vector3.up*.1f), transform.forward);
        Gizmos.DrawSphere(closestCorner, .2f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(previousCorner, .2f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(nextCorner, .2f);

    }

    //https://forum.unity.com/threads/left-right-test-function.31420/
    public bool isRightSide(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 right = Vector3.Cross(up.normalized, fwd.normalized);        // right vector
        float dir = Vector3.Dot(right, targetDir.normalized);
        return dir > 0f;
    }

    public void ZoomVolume(float x)
    {
        zoomVolume.weight = x;
    }

    // TODO: Yok et.
    IEnumerator WallMergePlayer(bool merge, Vector3 finalPosition, float scale, float finalTransition)
    {

        if (merge) yield return _wait;
        else exitPS.Play();

        if (merge) transform.DOMove(finalPosition, finalTransition).SetEase(Ease.InBack).SetId(this);

        transform.GetChild(0).DOScaleZ(scale, finalTransition).SetEase(Ease.InSine).SetId(this);

        yield return new WaitForSeconds(finalTransition);

        if (merge)
        {
            playerMovement.gameObject.SetActive(false);
            projectorMovement.SetPlayerPos();
        }
        else
        {
            playerMovement.enabled = true;
        }

        decalMovement.transform.GetChild(0).gameObject.SetActive(merge);

        mergePS.Play();

        decalProjector.enabled = decalMovement.isActive = merge;

        frameRenderer.material.DOColor(Color.clear, 1).SetId(this);

        flowController.CanMove = true;

        if (merge) uiController.ShowExitMergeBtn();

    }
}
