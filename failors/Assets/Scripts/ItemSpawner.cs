using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public float basicSpawnTime = 2.5f;
    public float minX = -2.3f;
    public float maxY = 2.3f;
    public float startY = 5.7f;
    //public float minSpeed = 0.5f;
    //public float maxSpeed = 1.5f;
    public float itemsSpeed = 0.9f;

    public float spawnTimeFactor = 0.99f;


    [SerializeField]
    private float itemsSpeedModifier = 0.035f;

    [SerializeField]
    private GameObject spawnedItemsContainer;

    public List<GameObject> itemPrefabs;

    [SerializeField]
    private List<GameObject> additionalItemPrefabs = new List<GameObject>();

    private int spawnedItems = 0;

    [SerializeField]
    private int newItemCheckpoint = 30;

    float nextSpawnTime;

    public void StartSpawningItems()
    {
        nextSpawnTime = basicSpawnTime;
        spawnedItems = 0;
        Invoke("SpawnNextItem", nextSpawnTime);
    }

    void SpawnNextItem()
    {
        if (!GameManager.instance.IsPaused())
        {
            itemsSpeed += itemsSpeedModifier;

            GameObject prefab = itemPrefabs[Random.Range(0, itemPrefabs.Count)];
            float spawnX = Random.Range(minX, maxY);
            GameObject spawned;

            //10% chance to spawn additional item
            if (spawnedItems > newItemCheckpoint * 2 && Random.Range(0, 10) == 0)
            {
                spawned = Instantiate(additionalItemPrefabs[1].gameObject, new Vector3(spawnX, startY, 0.0f), Quaternion.identity);
            }
            else if (spawnedItems > newItemCheckpoint && Random.Range(0, 10) == 0)
            {
                spawned = Instantiate(additionalItemPrefabs[0].gameObject, new Vector3(spawnX, startY, 0.0f), Quaternion.identity);
            }
            else
            {
                spawned = Instantiate(prefab, new Vector3(spawnX, startY, 0.0f), Quaternion.identity);
            }

            Item item = spawned.GetComponent<Item>();
            item.speed = itemsSpeed;

            spawned.transform.parent = spawnedItemsContainer.transform;

            nextSpawnTime *= spawnTimeFactor;

            spawnedItems++;
        }
        Invoke("SpawnNextItem", nextSpawnTime);
    }

    public void StopSpawningItems()
    {
        CancelInvoke("SpawnNextItem");

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
