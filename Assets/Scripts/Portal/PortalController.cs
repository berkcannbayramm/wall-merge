using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public enum PortalStates
{
    Ready,
    NotReady
}
[DefaultExecutionOrder(45)]
public class PortalController : MonoBehaviour
{
    [field:SerializeField] public PortalStates PortalState { get; set; }
    public List<PortalObjectsSO> NeedObjects;
    public List<ParticleSystem> OpenPS;
    public CollectManager CollectManager;
    public GameObject PortalEffects;

    private void Start()
    {
        if (PortalState == PortalStates.NotReady) PortalEffects.SetActive(false);
    }

    public void NeedItem()
    {
        //Debug.LogError(NeedObjects[0].NeedValue.ToString() + " " + NeedObjects[0].ObjectTypes.ToString());
    }

    public void CheckNeedItem()
    {
        var needObjectCount = NeedObjects.Count;
        byte controlCount = 0;

        foreach(var needObject in NeedObjects)
        {
            if(needObject.NeedValue <= CollectManager.CrystalCounts[needObject.ObjectID])
            {
                controlCount++;
            }
        }

        if(controlCount == needObjectCount)
        {
            PortalEffects.SetActive(true);
            foreach(var x in OpenPS)
            {
                x.Play();
            }
        }
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(PortalController))]
public class PortalControllrEditor: Editor
{
    public override void OnInspectorGUI()
    {

        var portalController = (PortalController)target;

        portalController.PortalState = (PortalStates)EditorGUILayout.EnumPopup("Portal Effects" ,portalController.PortalState);

        switch (portalController.PortalState)
        {
            case PortalStates.NotReady:
                portalController.PortalEffects = (GameObject)EditorGUILayout.ObjectField("Portal Effects", portalController.PortalEffects, typeof(GameObject), true);
                
                portalController.CollectManager = (CollectManager)EditorGUILayout.ObjectField("Portal", portalController.CollectManager, typeof(CollectManager), true);

                var needObjectList = serializedObject.FindProperty("NeedObjects");

                var psList= serializedObject.FindProperty("OpenPS");

                EditorGUILayout.PropertyField(needObjectList, new GUIContent("Need Object List"), true);

                EditorGUILayout.PropertyField(psList, new GUIContent("Particles"), true);

                serializedObject.ApplyModifiedProperties();
                break;
        }
    }
}
#endif
