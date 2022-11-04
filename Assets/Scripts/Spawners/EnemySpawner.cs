using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        Instantiate(enemyPrefab, transform.position + Vector3.up, transform.rotation);
    }
}
