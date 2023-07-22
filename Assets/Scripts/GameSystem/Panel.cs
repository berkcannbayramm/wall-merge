using BBGameStudios.Managers;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
    [SerializeField] private List<CanvasGroup> panels;
    [SerializeField] private Transform transitionCircle;
    private byte panelIndex;
    private void OnDestroy()
    {
        DOTween.Kill(this);
    }
    public void AppearPanel(byte index)
    {
        float panelAlphaValue = 0;

        panelIndex = index;

        panels[panelIndex].gameObject.SetActive(true);

        DOTween.To(() => panelAlphaValue, x => panelAlphaValue = x, 1, .3f)
            .OnUpdate(() =>
            {
                panels[panelIndex].alpha = panelAlphaValue;
            })
            .SetId(this)
            .SetDelay(2f);
    }

    public void DisapperPanel()
    {

        transitionCircle.DOScale(Vector3.one * 1.5f, .5f).SetDelay(.2f).SetId(this).OnComplete(() =>
        {
            panels[panelIndex].alpha = 0;

            panels[panelIndex].gameObject.SetActive(false);

            ScaleDownTransitionCircle();
        });
    }

    private void ScaleDownTransitionCircle()
    {
        transitionCircle.DOScale(Vector3.zero, .5f).SetId(this).OnComplete(() => GameManager.instance.CanClick = false);
    }
}
