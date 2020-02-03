using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;

public class FirebaseStartup : MonoBehaviour
{
    //public static FirebaseApp App { get; private set; }
    //public static FirebaseDatabase DatabaseApp { get; private set; }
    public static DatabaseReference DatabaseReference { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Setup()
    {
        var dependencyStatus = FirebaseApp.CheckDependencies();
        if (dependencyStatus == Firebase.DependencyStatus.Available)
        {
            Debug.Log("Firebase is ready to use");
            // Create and hold a reference to your FirebaseApp,
            // where app is a Firebase.FirebaseApp property of your application class.

            //App = FirebaseApp.DefaultInstance;
            //DatabaseApp = FirebaseDatabase.DefaultInstance;
            DatabaseReference = FirebaseDatabase.DefaultInstance.RootReference;

            //Debug.Log("onFirebaseLoaded.Invoke();");
            //Debug.Log(onFirebaseLoaded.GetPersistentMethodName(0));
            //onFirebaseLoaded.Invoke();
            //Invoke("InvokeInMainThread", 0.2f);
        }
        else
        {
            Debug.LogError(System.String.Format(
                "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            // Firebase Unity SDK is not safe to use here.
        }
        /*
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                Debug.Log("Firebase is ready to use");
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.

                //App = FirebaseApp.DefaultInstance;
                //DatabaseApp = FirebaseDatabase.DefaultInstance;
                DatabaseReference = FirebaseDatabase.DefaultInstance.RootReference;

                //Debug.Log("onFirebaseLoaded.Invoke();");
                //Debug.Log(onFirebaseLoaded.GetPersistentMethodName(0));
                //onFirebaseLoaded.Invoke();
                Invoke("InvokeInMainThread", 0.1f);
            }
            else
            {
                Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
        */
    }

    /*
    private void InvokeInMainThread()
    {
        //Debug.Log("onFirebaseLoaded.Invoke();");
        onFirebaseLoaded.Invoke();
    }
    */
}
