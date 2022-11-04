using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Place
{
    public static void UnderParent(GameObject what, Transform where)
    {
        what.transform.SetParent(where);
        what.transform.localPosition = Vector3.zero;
        what.transform.localRotation = Quaternion.identity;
        what.transform.localScale = Vector3.one;
    }
}