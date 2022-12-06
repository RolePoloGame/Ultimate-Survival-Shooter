using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();
    public float spawnTime = 3f;

    List<Transform> spawnPoints;

    void Start()
    {
        InitiateSpawnPointsList();

        if (spawnPoints.Count > 0)
            InvokeRepeating(nameof(Spawn), spawnTime, spawnTime);
    }

    private void InitiateSpawnPointsList()
    {
        spawnPoints = new List<Transform>();
        spawnPoints.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            spawnPoints.Add(transform.GetChild(i));
        }
    }

    void Spawn()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Count);
        int randomIndex = Random.Range(0, enemies.Count - 1);
        GameObject createdGameObject = Instantiate(enemies[randomIndex], spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }

}
