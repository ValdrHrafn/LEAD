using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MovementEvaluator
{
    public static bool IsGrounded(CharacterController controller)
    {
        LayerMask groundLayer = LayerMask.GetMask("Ground");
        Vector3 origin = controller.transform.position + new Vector3(0, .45f, 0);

        if (Physics.CheckSphere(origin, .49f, groundLayer))
        {
            return true;
        }
        return false;
    }

    public static Vector3 GroundNormal(CharacterController controller)
    {
        Vector3 origin = controller.transform.position + new Vector3(0, .1f, 0);
        Vector3 normal;
        LayerMask groundLayer = LayerMask.GetMask("Ground");

        Physics.Raycast(origin, Vector3.down, out var hit, 1, groundLayer);
        normal = IsGrounded(controller) ? hit.normal : Vector3.up;

        Debug.DrawRay(origin, Vector3.down, Color.red, 1);
        return normal;
    }
}