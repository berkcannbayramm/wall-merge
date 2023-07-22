using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressButton : MonoBehaviour
{
    [SerializeField] private Transform touchObject;
    [field:SerializeField] public WallType TargetWallType { get; set; }
    public bool IsPressed { get; set; }

    private void OnDestroy()
    {
        DOTween.Kill(this);
    }
    public void RotateObject()
    {
        touchObject.DOLocalRotate(new Vector3(36f, 0, 0), .3f).SetId(this);
    }
}
