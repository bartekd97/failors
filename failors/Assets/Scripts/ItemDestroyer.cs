using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDestroyer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.gameObject.GetComponent<Item>();

        if (item != null)
        {
            GameManager.instance.LoseHp();

            Destroy(collision.gameObject);
        }
    }
}
