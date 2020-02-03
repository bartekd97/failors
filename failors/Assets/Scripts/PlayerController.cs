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
        public Item item;
    }
    [Serializable]
    public class BlockGroup
    {
        public List<Faculty> group;
    }

    [SerializeField]
    private ChangeBackground backgroundChanger;

    public List<ShipType> availableShipTypes;

    private ShipType[] currentGameShipTypes = new ShipType[3];

    public float moveSpeed = 800.0f;

    public Faculty currentShipFaculty { get; private set; }


    public List<BlockGroup> blockedColorsTogether;

    float currentSpeed = 0;

    int currentShipIndex;
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    public ItemSpawner itemSpawner;

    [SerializeField]
    private GameObject collectSplash;

    [SerializeField]
    private GameObject wrongCollectSplash;

    [SerializeField]
    private GameObject explosion;

    [SerializeField]
    private GameObject collectStar;

    private void Awake()
    {
        //SetCurrentGameShipTypes();

        rb = GetComponent<Rigidbody2D>();
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
            currentShipIndex = currentGameShipTypes.Length - 1;

        Sprite activeSprite = this.spriteRenderer.sprite;

        this.spriteRenderer.sprite = previousShip.sprite;
        previousShip.sprite = nextShip.sprite;
        nextShip.sprite = activeSprite;
        
        currentShipFaculty = currentGameShipTypes[currentShipIndex].faculty;
        //spriteRenderer.sprite = availableShipTypes[currentShipIndex].sprite;
    }

    public void MoveTypeRight()
    {
        if (GameManager.instance.IsPaused())
            return;

        currentShipIndex++;

        if (currentShipIndex >= currentGameShipTypes.Length)
            currentShipIndex = 0;

        Sprite activeSprite = this.spriteRenderer.sprite;

        this.spriteRenderer.sprite = nextShip.sprite;
        nextShip.sprite = previousShip.sprite;
        previousShip.sprite = activeSprite;

        currentShipFaculty = currentGameShipTypes[currentShipIndex].faculty;
        //spriteRenderer.sprite = availableShipTypes[currentShipIndex].sprite;
    }

    [SerializeField]
    private Sounds collectingSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Item item = collision.gameObject.GetComponent<Item>();
        
        if (item != null)
        {
            if (item.possibleFaculties.Contains(currentShipFaculty) || item.possibleFaculties[0] == Faculty.ANY)
            {
                int scoreBefore = GameManager.instance.score;
                GameManager.instance.score += item.scoreReward;
                int scoreAfter = GameManager.instance.score;
                if ((scoreBefore / 10) != (scoreAfter / 10) && scoreBefore != 0)
                    backgroundChanger.ChangeToNext();

                collectingSound.PlayGettingPointSound();

                if(item.possibleFaculties[0] == Faculty.ANY)
                {
                    collectStar.transform.position = collision.transform.position;
                    collectStar.GetComponent<ParticleSystem>()?.Play();
                }
                else
                {
                    collectSplash.transform.position = collision.transform.position;
                    collectSplash.GetComponent<ParticleSystem>()?.Play();
                }
            }
            else
            {
                if(item.possibleFaculties[0] != Faculty.BOMB)
                {
                    GameManager.instance.LoseHp(false);
                    wrongCollectSplash.transform.position = collision.transform.position;
                    wrongCollectSplash.GetComponent<ParticleSystem>()?.Play();
                }
                else
                {
                    GameManager.instance.LoseHp(true); ;
                    explosion.transform.position = collision.transform.position;
                    explosion.GetComponent<ParticleSystem>()?.Play();
                }
            }

            Destroy(collision.gameObject);
        }
    }

    public void SetCurrentGameShipTypes()
    {
        itemSpawner.itemPrefabs.Clear();

        List<ShipType> shipTypes = new List<ShipType>(availableShipTypes);

        for(int i = 0; i < 3; i++)
        {
            currentGameShipTypes[i] = shipTypes[UnityEngine.Random.Range(0, shipTypes.Count)];
            shipTypes.Remove(currentGameShipTypes[i]);
            foreach (var blockGroup in blockedColorsTogether)
            {
                if (blockGroup.group.Contains(currentGameShipTypes[i].faculty))
                {
                    foreach (var blockedFaculty in blockGroup.group)
                    {
                        shipTypes.Remove(shipTypes.Find(st => st.faculty == blockedFaculty));
                    }
                }
            }

            itemSpawner.itemPrefabs.Add(currentGameShipTypes[i].item.gameObject);

            Debug.Log(currentGameShipTypes[i].faculty);
        }

        spriteRenderer = GetComponent<SpriteRenderer>();
        currentShipIndex = 0;
        currentShipFaculty = currentGameShipTypes[currentShipIndex].faculty;
        spriteRenderer.sprite = currentGameShipTypes[currentShipIndex].sprite;

        nextShip.sprite = currentGameShipTypes[currentShipIndex + 1].sprite;

        if (currentShipIndex - 1 < 0)
            previousShip.sprite = currentGameShipTypes[currentGameShipTypes.Length - 1].sprite;
        else
            previousShip.sprite = currentGameShipTypes[currentShipIndex - 1].sprite;
    }
}
