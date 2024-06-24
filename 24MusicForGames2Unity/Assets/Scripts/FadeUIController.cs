using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeUIController : MonoBehaviour
{
    public Image fadeImage;

    void Start()
    {
        // Ensure the image starts fully transparent
        SetAlpha(0f);
    }

    public void FadeToBlack(float duration)
    {
        StartCoroutine(Fade(1f, duration));
    }

    public void FadeFromBlack(float duration)
    {
        StartCoroutine(Fade(0f, duration));
    }

    private IEnumerator Fade(float targetAlpha, float duration)
    {
        float startAlpha = fadeImage.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            SetAlpha(newAlpha);
            yield return null;
        }

        SetAlpha(targetAlpha);
    }

    private void SetAlpha(float alpha)
    {
        Color color = fadeImage.color;
        color.a = alpha;
        fadeImage.color = color;
    }
}