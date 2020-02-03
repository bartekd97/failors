using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimateDarkBackground : MonoBehaviour
{
    public float animTime = 0.2f;
    public CanvasGroup cgroup;


    Coroutine currentCoro = null;

    public void FadeTo(float value)
    {
        if (currentCoro != null)
            StopCoroutine(currentCoro);

        currentCoro = StartCoroutine(fadeCoro(cgroup.alpha, value));
    }

    IEnumerator fadeCoro(float from, float to)
    {
        cgroup.alpha = from;
        float dt = 0.0f;
        while (dt < animTime)
        {
            cgroup.alpha = Mathf.SmoothStep(from, to, dt / animTime);
            yield return new WaitForEndOfFrame();
            dt += Time.deltaTime;
        }
        cgroup.alpha = to;
        currentCoro = null;
    }
}
