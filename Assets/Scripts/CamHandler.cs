using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamHandler : MonoBehaviour
{
    #region Variables
    [Header("Key Bindings")]
    [SerializeField] private float cameraSensitivity;
    [SerializeField] private float cameraHeight;
    [SerializeField] private int cameraMaxAngleVer;
    [SerializeField] private float cameraLookStart;
    [SerializeField] private float cameraLookDistance;
    [SerializeField] private Vector3 cameraLookOffset;

    [Header("\nOutput")]
    [SerializeField] private Vector2 cameraRot;
    [SerializeField] private Transform playerPos;
    #endregion

    private void Update()
    {
        //Make it sticky
        transform.position = playerPos.position + Vector3.up * cameraHeight;

        //Raycast and return an end position
        if (Physics.Raycast(transform.localPosition + transform.forward * cameraLookStart, transform.forward, out var hit, cameraLookDistance) && hit.collider != null)
        {
            CollStaticVar.camRayEndPos = hit.point;
            CollStaticVar.camRayHit = true;

            //Debug.Log("You're looking at " + hit.collider.gameObject.name);
        }
        else
        {
            CollStaticVar.camRayHit = false;
        }
        Debug.DrawRay(transform.localPosition + cameraLookOffset, transform.forward, Color.cyan, 1f);
    }

    void LateUpdate()
    {
        cameraRot.y += Input.GetAxis("Mouse X") * cameraSensitivity;
        cameraRot.x += -Input.GetAxis("Mouse Y") * cameraSensitivity;

        cameraRot.x = Mathf.Clamp(cameraRot.x, -cameraMaxAngleVer, cameraMaxAngleVer);

        var angle = playerPos.eulerAngles;
        angle.y = cameraRot.y;
        playerPos.eulerAngles = angle;

        transform.localRotation = Quaternion.Euler(cameraRot.x, cameraRot.y, 0);
    }
}