using UnityEngine;
using UnityEditor;
using System;
using Unity.VisualScripting;

public enum WallType
{
    Normal,
    Dissolve,
    Movement
}
public class WallBase : MonoBehaviour
{
    [SerializeField] protected WallMerge wallMerge;
    [field:SerializeField] public WallType WallType { get; set; }
    public DissolveWallProperties dissolveWallProperties;
    public MovementWallProperties movementWallProperties;

    private IWallActions _wallActions;

    private void Awake()
    {
        _wallActions = GetComponent<IWallActions>();
    }
    private void OnEnable()
    {
        wallMerge.PressButtonEvent += CheckButton;
    }

    private void OnDisable()
    {
        wallMerge.PressButtonEvent -= CheckButton;
    }

    private void CheckButton(WallType wallType)
    {
        switch (wallType)
        {
            case WallType.Dissolve:
            DissolveWall();
            break;
            
        }
    }

    public virtual void DissolveWall()
    {
        if (WallType != WallType.Dissolve) return;
        _wallActions.DissolveAction(dissolveWallProperties.MeshRenderer, dissolveWallProperties.FinalMaterial);
    }
}

[Serializable]
public class DissolveWallProperties
{
    public MeshRenderer MeshRenderer;
    public Material FinalMaterial;
}

[Serializable]
public class MovementWallProperties
{
    public Transform TargetPosition;
}
#if UNITY_EDITOR

[CustomEditor(typeof(WallBase), true)]
public class WallBaseEditor: Editor
{
    public override void OnInspectorGUI()
    {
        var wallBase = (WallBase)target;

        wallBase.WallType = (WallType)EditorGUILayout.EnumPopup("Wall Type", wallBase.WallType);
      
        switch (wallBase.WallType)
        {
            case WallType.Dissolve:
                wallBase.dissolveWallProperties.MeshRenderer = (MeshRenderer)EditorGUILayout.ObjectField("Mesh Renderer ", wallBase.dissolveWallProperties.MeshRenderer, typeof(MeshRenderer), true);
                wallBase.dissolveWallProperties.FinalMaterial = (Material)EditorGUILayout.ObjectField("Final Material", wallBase.dissolveWallProperties.FinalMaterial, typeof(Material), true);
                break;
            case WallType.Movement:
                wallBase.movementWallProperties.TargetPosition = (Transform)EditorGUILayout.ObjectField("Target Position", wallBase.movementWallProperties.TargetPosition, typeof(Transform), true);
                break;
        }
    }
}
#endif