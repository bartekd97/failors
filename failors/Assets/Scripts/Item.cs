using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public float speed = 1.0f;
    public List<Faculty> possibleFaculties;
    public int scoreReward = 1;

    public void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
}
