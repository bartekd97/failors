﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplayer : MonoBehaviour
{
    public Text text;
    public Text textshadow;
    public string prefix = "Score: ";

    private void Update()
    {
        if (GameManager.instance != null)
        {
            text.text = prefix + GameManager.instance.score;
            textshadow.text = prefix + GameManager.instance.score;
        }
    }
}
