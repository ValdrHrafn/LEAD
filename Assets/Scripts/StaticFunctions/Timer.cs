using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Timer
{
    public static float Positive(ref float timer) 
    {
        return timer += Time.deltaTime;
    }

    public static float Negative(ref float timer) 
    {
        return timer -= Time.deltaTime;
    }
}