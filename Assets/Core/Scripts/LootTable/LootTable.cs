using UnityEngine;

[CreateAssetMenu(menuName = "Loot/Table")]
public class LootTable : ScriptableObject
{
    [SerializeField]
    private GameObject[] lootItems;

    public GameObject GetLoot()
    {
        if (lootItems == null || lootItems.Length == 0)
            return null;

        return lootItems[Random.Range(0, lootItems.Length - 1)];
    }
}
