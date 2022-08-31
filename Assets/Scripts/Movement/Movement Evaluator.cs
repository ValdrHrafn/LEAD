using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MovementEvaluator
{
    public static bool IsGrounded(CharacterController controller)
    {
        Vector3 origin = controller.transform.position + new Vector3(0, .1f, 0);

        if (Physics.Raycast(origin, Vector3.down, out var hit, .3f))
        {
            return true;
        }
        return false;
    }

    public static Vector3 GroundNormal(CharacterController controller)
    {
        Vector3 origin = controller.transform.position + new Vector3(0, .1f, 0);
        Vector3 normal; 
        int layerMask = LayerMask.GetMask("Ground");

        if (Physics.Raycast(origin, Vector3.down, out var hit, .3f, layerMask))
        {
            normal = hit.normal;
            return normal;
        }
        return Vector3.zero;
    }
}