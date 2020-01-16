using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeBackground : MonoBehaviour
{
    [SerializeField] List<GameObject> backgroundList = new List<GameObject>();
    private int actualBackground = 0;

    public void ChangeBckg()
    {
        backgroundList[actualBackground].SetActive(false);

        if (actualBackground + 1 < backgroundList.Count)
            actualBackground++;
        else
            actualBackground = 0;

        backgroundList[actualBackground].SetActive(true);

        
    }
}

