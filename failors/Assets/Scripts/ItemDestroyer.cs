using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDestroyer : MonoBehaviour
{
    [SerializeField]
    private GameObject itemDestroyParticleEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.gameObject.GetComponent<Item>();

        if (item != null)
        {
            GameManager.instance.LoseHp();

            itemDestroyParticleEffect.transform.position = collision.transform.position;
            itemDestroyParticleEffect.GetComponent<ParticleSystem>()?.Play();

            Destroy(collision.gameObject);
        }
    }
}
