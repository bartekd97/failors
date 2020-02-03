using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Sounds : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip tap;
    public AudioClip whoosh;
    public AudioClip fail;

    public AudioClip[] coins;
    public AudioClip bomb;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayTapSound()
    {
        if(tap)
        {
            audioSource.clip = tap;
            PlaySound();
        }
    }

    public void PlayWhooshSound()
    {
        if(whoosh)
        {
            audioSource.clip = whoosh;
            PlaySound();
        }
    }

    public void PlayGettingPointSound()
    {
        if (coins.Length != 0)
        {
            audioSource.clip = coins[Random.Range(0, coins.Length)];
            PlaySound();
        }
    }

    public void PlayBombSound()
    {
        audioSource.clip = bomb;
        PlaySound();
    }

    public void PlayFailPointSound()
    {
        if (fail)
        {
            audioSource.clip = fail;
            PlaySound();
        }
    }

    private void PlaySound()
    {
        if (PlayerPrefs.HasKey("Sounds"))
        {
            if (PlayerPrefs.GetInt("Sounds") == 1)
                audioSource.Play();
        }
        else
            audioSource.Play();
    }
}
