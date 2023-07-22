using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NeedObjectItem : MonoBehaviour
{
    [field:SerializeField] public Image Icon { get; set; }
    [field:SerializeField] public TextMeshProUGUI Text { get; set; }
}
