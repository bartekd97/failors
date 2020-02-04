using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSummaryDisplayer : MonoBehaviour
{
    public Text hiscoreText;
    public Text hiscoreTextShadow;
    public Text totalText;
    public Text totalTextShadow;

    private void Start()
    {
        UpdateText();
    }

    public void AnimTo(int index)
    {
        /*
        hiscoreText.GetComponent<AnimateMoveTo>().MoveTo(index);
        hiscoreText.GetComponent<AnimateScaleTo>().ScaleTo(index);
        totalText.GetComponent<AnimateMoveTo>().MoveTo(index);
        totalText.GetComponent<AnimateScaleTo>().ScaleTo(index);
        */
        GetComponent<AnimateScaleTo>().ScaleTo(index);
        GetComponent<AnimateMoveTo>().MoveTo(index);
    }

    public void UpdateText()
    {
        int highScore = 0, totalScore = 0;
        if (PlayerPrefs.HasKey("HighestScore"))
        {
            highScore = PlayerPrefs.GetInt("HighestScore");
        }
        if (PlayerPrefs.HasKey("TotalScore"))
        {
            totalScore = PlayerPrefs.GetInt("TotalScore");
        }

        hiscoreText.text = "High Score: " + highScore.ToString("N0");
        hiscoreTextShadow.text = "High Score: " + highScore.ToString("N0");
        totalText.text = "Total: " + totalScore.ToString("N0");
        totalTextShadow.text = "Total: " + totalScore.ToString("N0");
    }
}
