using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public int hitpoints 
    { 
        get; 
        set; 
    }

    public void TakeDamage(int hitpointDamage);
}