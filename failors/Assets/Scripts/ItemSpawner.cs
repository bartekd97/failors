using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public float spawnTime = 2.5f;
    public float minX = -2.3f;
    public float maxY = 2.3f;
    public float startY = 5.7f;
    //public float minSpeed = 0.5f;
    //public float maxSpeed = 1.5f;
    public float itemsSpeed = 1.0f;

    [SerializeField]
    private float itemsSpeedModifier = 0.1f;

    [SerializeField]
    private GameObject spawnedItemsContainer;

    public List<GameObject> itemPrefabs;

    public void StartSpawningItems()
    {
        InvokeRepeating("SpawnNextItem", spawnTime, spawnTime);
    }

    void SpawnNextItem()
    {
        itemsSpeed += itemsSpeedModifier;

        GameObject prefab = itemPrefabs[Random.Range(0,itemPrefabs.Count)];
        float spawnX = Random.Range(minX, maxY);
        GameObject spawned = Instantiate(prefab, new Vector3(spawnX, startY, 0.0f), Quaternion.identity);
        Item item = spawned.GetComponent<Item>();
        item.speed = itemsSpeed;

        spawned.transform.parent = spawnedItemsContainer.transform;
    }

    public void StopSpawningItems()
    {
        CancelInvoke();

        int spawnedItems = spawnedItemsContainer.transform.childCount;

        for (int i = 0; i < spawnedItems; i++)
            Destroy(spawnedItemsContainer.transform.GetChild(i).gameObject);
    }

    public void Restart()
    {
        itemsSpeed = 1.0f;
        StartSpawningItems();
    }
}
