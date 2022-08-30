using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MovementEvaluator
{
    public static bool IsGrounded(GameObject player) {
        Vector3 origin = player.transform.localPosition + new Vector3(0, -.9f, 0);
        if(Physics.Raycast(origin, Vector3.down, out var hit, .2f)) {
            player.transform.position = hit.point;

            return true;
        }

        return false;
    }
}