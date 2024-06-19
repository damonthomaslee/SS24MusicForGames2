using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class CinemachineFadeController : MonoBehaviour
{
    public Volume volume;
    private Vignette vignette;

    void Start()
    {
        // Get the Vignette effect from the Post-Processing Volume
        if (volume.profile.TryGet(out Vignette v))
        {
            vignette = v;
        }
    }

    public void FadeToBlack(float duration)
    {
        StartCoroutine(Fade(1f, duration));
    }

    public void FadeFromBlack(float duration)
    {
        StartCoroutine(Fade(0f, duration));
    }

    private IEnumerator Fade(float targetIntensity, float duration)
    {
        float startIntensity = vignette.intensity.value;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            vignette.intensity.value = Mathf.Lerp(startIntensity, targetIntensity, elapsedTime / duration);
            yield return null;
        }

        vignette.intensity.value = targetIntensity;
    }
}