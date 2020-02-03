using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    public int score = 0;

    private bool gameplayActive = false;
    private bool gameOverMenuActive = false;

    public bool GameplayActive { get => gameplayActive; set => gameplayActive = value; }
    public bool GameOverMenuActive { get => gameOverMenuActive; set => gameOverMenuActive = value; }

    public Text signerUserText;
    public Text signedUserTextShadow;
    public Text signInOutButtonText;

    public DepartmentsRanking departmentsRanking;
    public Button buttonOpenerDepartmentChooser;

    private void Awake()
    {
        instance = this;

        SetPlayersHealth();
        Vibration.HasVibrator(); // initialize Vibration static class;
    }

    private void Start()
    {
        //  ADD THIS CODE BETWEEN THESE COMMENTS

        // Create client configuration
        PlayGamesClientConfiguration config = new
            PlayGamesClientConfiguration.Builder()
            .Build();

        // Enable debugging output (recommended)
        PlayGamesPlatform.DebugLogEnabled = true;

        // Initialize and activate the platform
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        // END THE CODE TO PASTE INTO START

        if (PlayerPrefs.HasKey("HadFirstGamesSignIn"))
        {
            // Try silent sign-in (second parameter is isSilent)
            PlayGamesPlatform.Instance.Authenticate(SignInCallback, true);
        }
        else
        {
            PlayerPrefs.SetInt("HadFirstGamesSignIn", 1);
            // Try not-silent sign-in (second parameter is isSilent)
            PlayGamesPlatform.Instance.Authenticate(SignInCallback, false);
        }
    }

    public void SignInCallback(bool success)
    {
        if (success)
        {
            Debug.Log("(Play Games) Signed in!");

            signInOutButtonText.text = "Sign Out";

            // Show the user's name
            signerUserText.text = Social.localUser.userName;
            signedUserTextShadow.text = Social.localUser.userName;
        }
        else
        {
            Debug.Log("(Play Games) Sign-in failed...");

            signInOutButtonText.text = "Sign In";

            signerUserText.text = "Not signed in";
            signedUserTextShadow.text = "Not signed in";
        }

        if (!PlayerPrefs.HasKey("HadFirstDepartmenChooseAsk"))
        {
            PlayerPrefs.SetInt("HadFirstDepartmenChooseAsk", 1);
            buttonOpenerDepartmentChooser.onClick.Invoke();
        }
    }
    public void SignInOutButtonClick()
    {
        if (!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            // Sign in with Play Game Services, showing the consent dialog
            // by setting the second parameter to isSilent=false.
            PlayGamesPlatform.Instance.Authenticate(SignInCallback, false);
        }
        else
        {
            // Sign out of play games
            PlayGamesPlatform.Instance.SignOut();

            // Reset UI
            signInOutButtonText.text = "Sign In";
            signerUserText.text = "Not signed in";
            signedUserTextShadow.text = "Not signed in";
        }
    }
    public void LeaderboardsButtonClick()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated)
            PlayGamesPlatform.Instance.ShowLeaderboardUI();
        else
            PlayGamesPlatform.Instance.Authenticate(SignInCallbackLeaderboards, false);
    }
    public void SignInCallbackLeaderboards(bool success)
    {
        SignInCallback(success);
        if (success)
            LeaderboardsButtonClick();
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


    [SerializeField]
    private bool isPaused = false;
    public void SetPaused(bool paused)
    {
        isPaused = paused;
    }
    public bool IsPaused()
    {
        return isPaused;
    }


    private void SetPlayersHealth()
    {
        health = hearts.Count;
    }


    [SerializeField]
    private Sounds failSound;

    [SerializeField]
    private Sounds bombSound;

    public void LoseHp(bool bombExploded)
    {
        health--;

        if (bombExploded)
            bombSound.PlayBombSound();
        else
            failSound.PlayFailPointSound();

        hearts[health].SetActive(false);

        if (PlayerPrefs.HasKey("Vibrations"))
        {
            if(PlayerPrefs.GetInt("Vibrations") == 1)
                Vibration.Vibrate(200);
        }
        else
            Vibration.Vibrate(200);

        CheckDeathCondition();
    }

    private void CheckDeathCondition()
    {
        if(health <= 0)
        {
            //playerController.enabled = false;
            SetPaused(true);

            gameOverMenuAnimator.SetTrigger("Show");
            gameOverMenuActive = true;
            leftSide.SetTrigger("Hide");
            rightSide.SetTrigger("Hide");

            itemSpawner.StopSpawningItems();

            gameplayActive = false;

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
        playerController.SetCurrentGameShipTypes();

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

    [SerializeField]
    private GameObject newRecord;

    private void CheckPlayerScore()
    {
        if (score > PlayerPrefs.GetInt("HighestScore"))
        {
            newRecord.SetActive(true);
        }

        if (PlayerPrefs.HasKey("HighestScore"))
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

        if(PlayerPrefs.HasKey("TotalScore"))
        {
            PlayerPrefs.SetInt("TotalScore", PlayerPrefs.GetInt("TotalScore") + score);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetInt("TotalScore", score);
            PlayerPrefs.Save();
        }

        SkinsManager.instance.UpdateSkinsStatus();

        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {
            // Note: make sure to add 'using GooglePlayGames'
            PlayGamesPlatform.Instance.ReportScore(score,
                GPGSIds.leaderboard_departmentsstorm_leaderboard,
                (bool success) =>
                {
                    Debug.Log("(Play Games) Leaderboard update success: " + success);
                });
        }

        departmentsRanking.AddScoreToMyDepartmentIfCan(score);
    }

    private void ShowPlayerScore()
    {
        scoreText.text = score.ToString();
        highestScoreText.text = PlayerPrefs.GetInt("HighestScore").ToString();
    }

    #endregion

    #region Vibrations

    public void ChangeVibrationSettings()
    {
        if (PlayerPrefs.HasKey("Vibrations"))
        {
            if (PlayerPrefs.GetInt("Vibrations") == 0)
            {
                PlayerPrefs.SetInt("Vibrations", 1);
                Vibration.Vibrate(200);
            }
            else
                PlayerPrefs.SetInt("Vibrations", 0);
        }
        else
            PlayerPrefs.SetInt("Vibrations", 0);

        PlayerPrefs.Save();
    }


    #endregion
}
