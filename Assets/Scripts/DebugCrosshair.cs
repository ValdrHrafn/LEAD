using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCrosshair : MonoBehaviour
{
    void Update()
    {
        transform.position = CollStaticVar.camRayEndPos;
    }
}