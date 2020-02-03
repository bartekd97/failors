using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateScaleTo : MonoBehaviour
{
    public float animTime = 0.2f;

    public Vector3[] possibleScales;

    Coroutine currentCoro = null;

    public void ScaleTo(int index)
    {
        if (currentCoro != null)
            StopCoroutine(currentCoro);

        currentCoro = StartCoroutine(scaleCoro(transform.localScale, possibleScales[index]));
    }

    IEnumerator scaleCoro(Vector3 from, Vector3 to)
    {
        transform.localScale = from;
        float dt = 0.0f;
        while (dt < animTime)
        {
            transform.localScale = new Vector3(
                    Mathf.SmoothStep(from.x, to.x, dt / animTime),
                    Mathf.SmoothStep(from.y, to.y, dt / animTime),
                    Mathf.SmoothStep(from.z, to.z, dt / animTime)
                );
            yield return new WaitForEndOfFrame();
            dt += Time.deltaTime;
        }
        transform.localScale = to;
        currentCoro = null;
    }
}
