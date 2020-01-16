using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Vector3 basePosition;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        basePosition = transform.position;
    }

    private void OnEnable()
    {
        transform.position = basePosition;
        spriteRenderer.sortingOrder = -2;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void FadeOut(float animTime)
    {
        StartCoroutine(FadeOutCo(animTime));
    }

    IEnumerator FadeOutCo(float animTime)
    {
        float dt = 0.0f;
        spriteRenderer.sortingOrder = -1;
        while (dt < animTime)
        {
            yield return new WaitForEndOfFrame();
            dt += Time.deltaTime;
            transform.position = basePosition + Vector3.left * -10.0f * (dt / animTime);
        }
        gameObject.SetActive(false);
    }
}
