using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DepartmentsRanking : MonoBehaviour
{
    [Serializable]
    public class DepartmentRow
    {
        public string name;
        public RectTransform row;
        public Text scoreCounter;
    }

    public float startPosY = -235.0f;
    public float fullRowHeight = 138.0f;
    public float minLength = 240.0f;
    public float maxLength = 1040.0f;

    public DepartmentRow[] departments;
}
