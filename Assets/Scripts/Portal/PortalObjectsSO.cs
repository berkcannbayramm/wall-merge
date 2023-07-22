using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ObjectTypes
{
    BlueDiamond,
    RedDiamond
}
[CreateAssetMenu(fileName ="NewPortalObject", menuName = "PortalObject")]
public class PortalObjectsSO : ScriptableObject
{
    public ObjectTypes ObjectTypes;
    public string ObjectName;
    public Sprite ObjectIcon;
    public GameObject ObjectModel;
    public int ObjectID;
    public byte NeedValue;
}
