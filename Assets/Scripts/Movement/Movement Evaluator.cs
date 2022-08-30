using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MovementEvaluator
{
    public static bool IsGrounded(CharacterController controller)
    {
        Vector3 origin = controller.transform.position + new Vector3(0, .1f, 0);
        Vector3 normal;

        if (Physics.Raycast(origin, Vector3.down, out var hit, .3f))
        {
            normal = hit.normal;
            return true;
        }

        return false;
    }
}