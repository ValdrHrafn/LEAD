using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFirstPerson : MonoBehaviour
{
    //Vector2 cameraRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
