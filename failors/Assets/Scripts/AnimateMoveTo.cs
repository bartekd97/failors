using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateMoveTo : MonoBehaviour
{
    public float animTime = 0.2f;

    public Vector3[] possiblePositions;

    Coroutine currentCoro = null;

    public void MoveTo(int index)
    {
        if (currentCoro != null)
            StopCoroutine(currentCoro);

        currentCoro = StartCoroutine(moveCoro(transform.localPosition, possiblePositions[index]));
    }

    IEnumerator moveCoro(Vector3 from, Vector3 to)
    {
        transform.localPosition = from;
        float dt = 0.0f;
        while (dt < animTime)
        {
            transform.localPosition = new Vector3(
                    Mathf.SmoothStep(from.x, to.x, dt / animTime),
                    Mathf.SmoothStep(from.y, to.y, dt / animTime),
                    Mathf.SmoothStep(from.z, to.z, dt / animTime)
                );
            yield return new WaitForEndOfFrame();
            dt += Time.deltaTime;
        }
        transform.localPosition = to;
        currentCoro = null;
    }
}
