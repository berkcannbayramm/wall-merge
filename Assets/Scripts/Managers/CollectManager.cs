using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CollectManager : MonoBehaviour
{
    [field:SerializeField] public List<int> CrystalCounts { get; set; }

    private void Start()
    {
#if UNITY_EDITOR
        var soCount = Directory.GetFiles("Assets/SO", "*.asset").Length;

        CrystalCounts = new List<int>(new int[soCount]);
#elif PLATFORM_ANDROID
        CrystalCounts = new List<int>(new int[2]);
#endif
    }
}
