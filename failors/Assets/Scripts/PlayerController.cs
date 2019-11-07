using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const float SPEED_SMOOTHING = 0.75F;
    
    [Serializable]
    public class ShipType
    {
        public Faculty faculty;
        public Sprite sprite;
    }
    public List<ShipType> availableShipTypes;
    public float moveSpeed = 0.5f;

    public Faculty currentShipFaculty { get; private set; }

    float currentSpeed = 0;

    int currentShipIndex;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentShipIndex = 0;
        currentShipFaculty = availableShipTypes[currentShipIndex].faculty;
        spriteRenderer.sprite = availableShipTypes[currentShipIndex].sprite;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (touch.position.x / (float)Screen.width < 0.5f)
                    MoveTypeLeft();
                else
                    MoveTypeRight();
            }
        }
    }

    private void FixedUpdate()
    {
        float targetSpeed = Input.acceleration.x * moveSpeed;
        currentSpeed = currentSpeed * SPEED_SMOOTHING + (1f - SPEED_SMOOTHING) * targetSpeed;
        rb.velocity = Vector2.right * currentSpeed * Time.fixedTime;
    }

    void MoveTypeLeft()
    {
        currentShipIndex--;
        if (currentShipIndex < 0)
            currentShipIndex = availableShipTypes.Count - 1;
        currentShipFaculty = availableShipTypes[currentShipIndex].faculty;
        spriteRenderer.sprite = availableShipTypes[currentShipIndex].sprite;
    }
    void MoveTypeRight()
    {
        currentShipIndex++;
        if (currentShipIndex >= availableShipTypes.Count)
            currentShipIndex = 0;
        currentShipFaculty = availableShipTypes[currentShipIndex].faculty;
        spriteRenderer.sprite = availableShipTypes[currentShipIndex].sprite;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.gameObject.GetComponent<Item>();
        if (item != null)
        {
            if (item.possibleFaculties.Contains(currentShipFaculty))
                GameManager.instance.score += item.scoreReward;

            Destroy(collision.gameObject);
        }
    }
}
