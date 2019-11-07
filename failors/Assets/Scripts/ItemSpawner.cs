using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public float spawnTime = 2.5f;
    public float minX = -2.3f;
    public float maxY = 2.3f;
    public float startY = 5.7f;
    public float minSpeed = 0.5f;
    public float maxSpeed = 1.5f;
    public List<GameObject> itemPrefabs;

    private void Start()
    {
        InvokeRepeating("SpawnNextItem", spawnTime, spawnTime);
    }

    void SpawnNextItem()
    {
        GameObject prefab = itemPrefabs[Random.Range(0,itemPrefabs.Count)];
        float spawnX = Random.Range(minX, maxY);
        GameObject spawned = Instantiate(prefab, new Vector3(spawnX, startY, 0.0f), Quaternion.identity);
        Item item = spawned.GetComponent<Item>();
        item.speed = Random.Range(minSpeed, maxSpeed);
    }
}
