using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Music : MonoBehaviour
{
    private AudioSource audiosource;

    [SerializeField]
    private bool isMusic = false;

    [SerializeField]
    private bool playOnAwake = false;

    private void Awake()
    {
        audiosource = GetComponent<AudioSource>();

        if (playOnAwake)
            Play();
    }

    public void Play()
    {
        if(isMusic)
        {
            if (PlayerPrefs.HasKey("Music"))
            {
                if(PlayerPrefs.GetInt("Music") == 1)
                    audiosource.Play();
            }
            else
            {
                Debug.Log("Play");
                audiosource.Play();
            }
        }
        else
        {
            if (PlayerPrefs.HasKey("Sounds"))
            {
                if (PlayerPrefs.GetInt("Sounds") == 1)
                    audiosource.Play();
            }
            else
                audiosource.Play();
        }
    }

    public void ChangeMusicSettings()
    {
        int startPlayingMusic = 0;

        if (PlayerPrefs.HasKey("Music"))
        {
            if(PlayerPrefs.GetInt("Music") == 0)
                PlayerPrefs.SetInt("Music", 1);
            else
                PlayerPrefs.SetInt("Music", 0);

            startPlayingMusic = PlayerPrefs.GetInt("Music");
        }
        else
            PlayerPrefs.SetInt("Music", 0);

        if(isMusic)
        {
            if (startPlayingMusic == 1)
                audiosource.Play();
            else
                audiosource.Stop();
        }

        PlayerPrefs.Save();
    }

    public void ChangeSoundsSettings()
    {
        if (PlayerPrefs.HasKey("Sounds"))
        {
            if (PlayerPrefs.GetInt("Sounds") == 0)
                PlayerPrefs.SetInt("Sounds", 1);
            else
                PlayerPrefs.SetInt("Sounds", 0);
        }
        else
            PlayerPrefs.SetInt("Sounds", 0);

        PlayerPrefs.Save();
    }
}
