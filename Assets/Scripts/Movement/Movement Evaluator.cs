using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MovementEvaluator
{
    public static bool IsGrounded(CharacterController controller) {
        Vector3 origin = controller.transform.position + new Vector3(0, .1f, 0);

        if(Physics.Raycast(origin, Vector3.down, out var hit, .2f)) 
        {
            controller.Move(new Vector3(0, -hit.distance, 0));
            return true;
        }

        return false;
    }

    public static float LookDownDistance(GameObject player, float distance) {
        if (Physics.Raycast(player.transform.position, Vector3.down, out var hit, distance)) 
        {
            return hit.distance;
        }

        return 0;
    }
}