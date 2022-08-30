using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour, IDamagable
{
    public int Health 
    {
        get;
        set; 
    }

    public int health;

    private void Awake()
    {
        Health = health;
    }

    public void TakeDamage(int damageAmount)
    {
        Health -= damageAmount;

        //Debug.Log(gameObject.name + " said ouchie");

        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
