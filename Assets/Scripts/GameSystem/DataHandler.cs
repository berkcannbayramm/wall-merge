using BBGameStudios.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataHandler
{
    private const string MoneyIndexKey = "MoneyIndexKey";
    private const string LevelKey = "LevelKey";


    public static int MoneyIndex
    {
        get => PlayerPrefs.GetInt(MoneyIndexKey, 300);
        set => PlayerPrefs.SetInt(MoneyIndexKey, value);
    }
    public static int Level
    {
        get => PlayerPrefs.GetInt(LevelKey, 0);
        set => PlayerPrefs.SetInt(LevelKey, value);
    }
}
