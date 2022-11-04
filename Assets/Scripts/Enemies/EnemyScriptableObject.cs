using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data", menuName = "ScriptableObject/Enemy")]
public class EnemyScriptableObject : ScriptableObject
{
    public string enemyName;
    public string description;
    
    public GameObject enemyModel;
    public GameObject enemyGun;

    public int health;
    public float visionRange = 10f;
    public float speed = 2f;
}