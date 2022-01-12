using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DataToolEditor
{
    [MenuItem("WatchYourStep/ResetCoinNum")]
    static void ResetCoinNum()
    {
        PlayerPrefs.SetInt("CoinNum", 0);
        Debug.Log("ResetCoinNum");
    }

    [MenuItem("WatchYourStep/ResetHighScore")]
    static void ResetDistance()
    {
        PlayerPrefs.SetInt("HighScore", 0);
        Debug.Log("ResetHighScore");
    }
}
