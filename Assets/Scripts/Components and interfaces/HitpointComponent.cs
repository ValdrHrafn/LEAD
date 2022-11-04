using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitpointComponent : MonoBehaviour, IDamagable
{
    public int hitpoints 
    {
        get;
        set; 
    }

    public int hitpointMax;

    private void Awake()
    {
        hitpoints = hitpointMax;
    }

    public void TakeDamage(int hitpointDamage)
    {
        hitpoints -= hitpointDamage;

        Debug.Log(gameObject.name + " said ouchie");

        if (hitpoints <= 0)
        {
            Destroy(gameObject);
        }
    }
}
