using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeBackground : MonoBehaviour
{
    public float animTime = 2.0f;
    [SerializeField] List<Background> backgroundList = new List<Background>();

    private int currentBackground = 0;

    public void ChangeToNext()
    {
        //backgroundList[actualBackground].SetActive(false);
        backgroundList[currentBackground].FadeOut(animTime);

        if (currentBackground + 1 < backgroundList.Count)
            currentBackground++;
        else
            currentBackground = 0;

        //backgroundList[currentBackground].SetActive(true);
        backgroundList[currentBackground].Show();
    }

    public void ResetBackground()
    {
        if (currentBackground != 0)
        {
            backgroundList[currentBackground].Hide();
            currentBackground = 0;
            backgroundList[currentBackground].Show();
        }
    }
}

