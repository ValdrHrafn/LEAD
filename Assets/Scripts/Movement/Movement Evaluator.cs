using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MovementEvaluator
{
    public static bool IsGrounded(GameObject player) {
        Vector3 origin = player.transform.position + new Vector3(0, .1f, 0);
        Debug.DrawRay(origin,Vector3.down * 1.3f, Color.blue);

        if(Physics.Raycast(origin, Vector3.down, out var hit, .3f)) 
        {
            player.transform.position = hit.point;
            return true;
        }

        return false;
    }
}