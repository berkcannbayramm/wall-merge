using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private WallMerge wallMerge;
    [SerializeField] private ProjectorMovement projectorMovement;
    [SerializeField] private GameObject mergeButton, exitMergeButton, joystick;
    [SerializeField] private Transform player;
    public bool CanMerge { get; set; }

    private bool _control = true;
    private void Start()
    {
        Application.targetFrameRate = 60;
    }
    public void Merge(Button button)
    {
        wallMerge.MergePlayer();

        _control = false;
        mergeButton.transform.localScale = Vector3.zero;
    }
    public void HideMergeButtons()
    {
        mergeButton.transform.localScale = Vector3.zero;
        exitMergeButton.transform.localScale = Vector3.zero;
    }
    public void ShowMergeButton()
    {
        if (!_control) return;
        mergeButton.transform.localScale = Vector3.one;
        exitMergeButton.transform.localScale = Vector3.zero;
    }

    public void ShowExitMergeBtn()
    {
        exitMergeButton.transform.localScale = Vector3.one;
    }
    public void ExitMerge()
    {
        _control = true;
        player.SetParent(null);
        CanMerge = false;
        projectorMovement.ExitMerge();
        mergeButton.transform.localScale = Vector3.zero;
        exitMergeButton.transform.localScale = Vector3.zero;
    }
    public void HideJoystick()
    {
        joystick.SetActive(false);
    }
}
