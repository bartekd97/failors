using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public GameObject musicCross;
    public GameObject vibrationCross;
    public GameObject soundCross;

    public Music musicScript;

    private void Start()
    {
        UpdateCrosses();
    }

    public void ChangeMusicSetting()
    {
        musicScript.ChangeMusicSettings();
        UpdateCrosses();
    }
    public void ChangeVibrationsSettings()
    {
        GameManager.instance.ChangeVibrationSettings();
        UpdateCrosses();
    }
    public void ChangeSoundsSettings()
    {
        musicScript.ChangeSoundsSettings();
        UpdateCrosses();
    }

    void UpdateCrosses()
    {
        if (PlayerPrefs.HasKey("Music"))
        {
            if (PlayerPrefs.GetInt("Music") == 1)
                musicCross.SetActive(false);
            else
                musicCross.SetActive(true);
        }
        else
            musicCross.SetActive(false);



        if (PlayerPrefs.HasKey("Vibrations"))
        {
            if (PlayerPrefs.GetInt("Vibrations") == 1)
                vibrationCross.SetActive(false);
            else
                vibrationCross.SetActive(true);
        }
        else
            vibrationCross.SetActive(false);



        if (PlayerPrefs.HasKey("Sounds"))
        {
            if (PlayerPrefs.GetInt("Sounds") == 1)
                soundCross.SetActive(false);
            else
                soundCross.SetActive(true);
        }
        else
            soundCross.SetActive(false);
    }
}
