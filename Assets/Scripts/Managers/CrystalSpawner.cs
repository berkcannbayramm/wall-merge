using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(50)]
public class CrystalSpawner : MonoBehaviour
{
    [SerializeField] private PortalController portalController;
    [SerializeField] private BoxCollider boxCollider;
    [field:SerializeField] public PortalObjectsSO PortalObjectsSO { get; set; }

    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        var crystal = Instantiate(PortalObjectsSO.ObjectModel, transform);

        crystal.transform.localPosition = Vector3.zero;

        portalController.NeedObjects.Add(PortalObjectsSO);
    }

    public void CollectCrystal()
    {
        GetComponentInChildren<MeshRenderer>().enabled = false;
        GetComponentInChildren<ParticleSystem>().Play();
        boxCollider.enabled = false;
    }
}
