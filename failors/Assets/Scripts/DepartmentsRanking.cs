using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class DepartmentsRanking : MonoBehaviour
{
    //private static FirebaseApp app = null;
    //private static FirebaseDatabase databaseApp = null;

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

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                Debug.Log("Firebase is ready to use");
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                /*
                app = FirebaseApp.DefaultInstance;
                databaseApp = FirebaseDatabase.DefaultInstance;
                DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
                string json = JsonUtility.ToJson(this);
                reference.Child("users").SetRawJsonValueAsync(json);
                */
            }
            else
            {
                Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }
}
