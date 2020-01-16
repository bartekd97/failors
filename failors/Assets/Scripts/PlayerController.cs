using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    const float SPEED_SMOOTHING = 0.5F;
    
    [Serializable]
    public class ShipType
    {
        public Faculty faculty;
        public Sprite sprite;
    }
    [SerializeField]
    private ChangeBackground backgroundChanger;
    public List<ShipType> availableShipTypes;
    public float moveSpeed = 800.0f;

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

        nextShip.sprite = availableShipTypes[currentShipIndex + 1].sprite;

        if(currentShipFaculty - 1 < 0)
            previousShip.sprite = availableShipTypes[availableShipTypes.Count - 1].sprite;
        else
            previousShip.sprite = availableShipTypes[currentShipIndex - 1].sprite;
    }

    private void Update()
    {
        /*
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
        */
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.IsPaused())
        {
            rb.simulated = true;

            float targetSpeed = Input.acceleration.x * moveSpeed;
            currentSpeed = currentSpeed * SPEED_SMOOTHING + (1f - SPEED_SMOOTHING) * targetSpeed;

            rb.velocity = Vector2.right * currentSpeed * Time.fixedDeltaTime;
        }
        else
        {
            rb.simulated = false;
        }
    }

    [SerializeField]
    private Image previousShip;

    [SerializeField]
    private Image nextShip;


    public void MoveTypeLeft()
    {
        if (GameManager.instance.IsPaused())
            return;

        currentShipIndex--;

        if (currentShipIndex < 0)
            currentShipIndex = availableShipTypes.Count - 1;

        Sprite activeSprite = this.spriteRenderer.sprite;

        this.spriteRenderer.sprite = previousShip.sprite;
        previousShip.sprite = nextShip.sprite;
        nextShip.sprite = activeSprite;
        
        currentShipFaculty = availableShipTypes[currentShipIndex].faculty;
        //spriteRenderer.sprite = availableShipTypes[currentShipIndex].sprite;
    }

    public void MoveTypeRight()
    {
        if (GameManager.instance.IsPaused())
            return;

        currentShipIndex++;

        if (currentShipIndex >= availableShipTypes.Count)
            currentShipIndex = 0;

        Sprite activeSprite = this.spriteRenderer.sprite;

        this.spriteRenderer.sprite = nextShip.sprite;
        nextShip.sprite = previousShip.sprite;
        previousShip.sprite = activeSprite;

        currentShipFaculty = availableShipTypes[currentShipIndex].faculty;
        //spriteRenderer.sprite = availableShipTypes[currentShipIndex].sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.gameObject.GetComponent<Item>();
        
        if (item != null)
        {
            if (item.possibleFaculties.Contains(currentShipFaculty))
            {
                GameManager.instance.score += item.scoreReward;
                if (GameManager.instance.score % 10 == 0)
                    backgroundChanger.ChangeToNext();

            }
            else
                GameManager.instance.LoseHp();

            Destroy(collision.gameObject);
        }
    }
}
