using Core.World;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    [Required]
    private Transform MinimapCamera;

    [SerializeField]
    private List<GameObject> enemies = new();
    [SerializeField]
    private float spawnTime = 3f;

    [SerializeField]
    [ReadOnly]
    private List<Transform> spawnPoints;

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
        Transform spawnTransform = GetRandomSpawnTransform();
        GameObject createdGameObject = Instantiate(GetRandomEnemyPrefab(), spawnTransform.position, spawnTransform.rotation);
        createdGameObject.GetComponentInChildren<MinimapIcon>().UpdateMinimapCamera(MinimapCamera);
    }

    private Transform GetRandomSpawnTransform()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Count);
        Transform spawnTransform = spawnPoints[spawnPointIndex];
        return spawnTransform;
    }

    private GameObject GetRandomEnemyPrefab()
    {
        int randomIndex = Random.Range(0, enemies.Count - 1);
        GameObject spawnedEnemy = enemies[randomIndex];
        return spawnedEnemy;
    }
}
