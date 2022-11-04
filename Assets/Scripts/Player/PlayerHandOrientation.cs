using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandOrientation : MonoBehaviour
{
    [SerializeField] private Transform camTransform;
    [SerializeField] private Transform handTransform;


    private Vector3 crosshairPosition;
    [SerializeField] private float aimLookStart;
    [SerializeField] private float aimLookDistance;

    public void Update()
    {
        LookAtAim();
    }

    public virtual void LookAtAim() //Look at where you are aiming
    {
        if (Physics.Raycast(camTransform.localPosition + camTransform.forward * aimLookStart, camTransform.forward, out var hit, aimLookDistance))
        {
            handTransform.LookAt(hit.point);
            crosshairPosition = hit.point;
        }
        else
        {
            handTransform.rotation = transform.rotation;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(crosshairPosition, .05f);
    }
}