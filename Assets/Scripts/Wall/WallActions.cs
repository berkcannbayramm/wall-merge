using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallActions : MonoBehaviour, IWallActions
{
    private static readonly int CutoffHeight = Shader.PropertyToID("_Cutoff_Height");

    public void DissolveAction(MeshRenderer meshRenderer, Material finalMat)
    {
        DOTween.To(() => meshRenderer.material.GetFloat(CutoffHeight), x => meshRenderer.material
            .SetFloat(CutoffHeight, x), 10, 1f).SetId(this).OnComplete(()=>meshRenderer.material = finalMat);
    }
}
