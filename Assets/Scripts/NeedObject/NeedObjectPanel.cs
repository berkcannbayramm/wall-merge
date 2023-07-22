using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(60)]
public class NeedObjectPanel : MonoBehaviour
{
    [SerializeField] private PortalController portalController;
    [SerializeField] private GameObject needObjItemPrefab;
    [SerializeField] private Transform parent;

    private void Start()
    {
        if (portalController.PortalState == PortalStates.Ready) return;

        for(int i = 0; i < portalController.NeedObjects.Count; i++)
        {
            var item = Instantiate(needObjItemPrefab).gameObject.GetComponent<NeedObjectItem>();

            var needObject = portalController.NeedObjects[i];

            item.Icon.sprite = needObject.ObjectIcon;

            item.Text.text = $"{needObject.NeedValue}x {needObject.ObjectName}";

            Transform itemTransform;
            (itemTransform = item.transform).SetParent(parent);

            itemTransform.localPosition = Vector3.zero;
        }
    }

}
