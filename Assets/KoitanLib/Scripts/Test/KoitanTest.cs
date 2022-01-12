using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KoitanLib;

public class KoitanTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //KoitanDebug.Display($"SystemInfo.graphicsDeviceType = {SystemInfo.graphicsDeviceType}\n");
        //KoitanDebug.Display($"SystemInfo.graphicsDeviceName = {SystemInfo.graphicsDeviceName}\n");
        KoitanDebug.Display($"Resolution = ({Screen.width}, {Screen.height})\n");
    }
}
