using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public int score = 0;

    private void Awake()
    {
        instance = this;

        SetPlayersHealth();
    }

    #region Player's Health

    private int health = 3;

    [Header("Number of hearts = player's health")]
    [SerializeField]
    private List<GameObject> hearts = new List<GameObject>();

    [SerializeField]
    private Animator gameOverMenuAnimator;

    [SerializeField]
    private Animator leftSide, rightSide;

    private void SetPlayersHealth()
    {
        health = hearts.Count;
    }

    public void LoseHp()
    {
        health--;

        hearts[health].SetActive(false);

        CheckDeathCondition();
    }

    private void CheckDeathCondition()
    {
        if(health <= 0)
        {
            playerController.enabled = false;

            gameOverMenuAnimator.SetTrigger("Show");
            leftSide.SetTrigger("Hide");
            rightSide.SetTrigger("Hide");

            itemSpawner.StopSpawningItems();

            CheckPlayerScore();
            ShowPlayerScore();
        }
    }

    #endregion

    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private ItemSpawner itemSpawner;

    #region Play Again

    public void PlayAgain()
    {
        playerController.enabled = true;

        score = 0;

        itemSpawner.Restart();

        foreach (GameObject heart in hearts)
            heart.SetActive(true);

        SetPlayersHealth();
    }

    #endregion

    #region Player's Score

    [SerializeField]
    private Text scoreText, highestScoreText;

    private void CheckPlayerScore()
    {
        if(PlayerPrefs.HasKey("HighestScore"))
        {
            if(PlayerPrefs.GetInt("HighestScore") < score)
            {
                PlayerPrefs.SetInt("HighestScore", score);
                PlayerPrefs.Save();
            }
        }
        else
        {
            PlayerPrefs.SetInt("HighestScore", score);
            PlayerPrefs.Save();
        }
    }

    private void ShowPlayerScore()
    {
        scoreText.text = score.ToString();
        highestScoreText.text = PlayerPrefs.GetInt("HighestScore").ToString();
    }

    #endregion
}
