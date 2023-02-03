using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootDrop : MonoBehaviour
{
    [SerializeField]
    private LootTable Loot;

    public void SpawnLoot()
    {
        GameObject original = Loot.GetLoot();
        if (original == null) return;

        _ = Instantiate(original, transform.position, Quaternion.identity);
    }
}
