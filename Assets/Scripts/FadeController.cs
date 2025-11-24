using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeController : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1.0f;
    public float mainMenuDelay = 2.1f;
    public float levelStartDelay = 0.5f;
    public bool mainMenuFirst = true;

    private bool isFading = false;

    void Start()
    {
        if (fadeImage == null)
        {
            Debug.LogError("FadeController: Fade Image reference is missing!");
        }
    }

    public void FadeToBlack()
    {
        if (!isFading)
        {
            StartCoroutine(Fade(0, 1, 0f, false));
        }
    }

    public void FadeFromBlack()
    {
        if (!isFading)
        {
            StartCoroutine(Fade(1, 0, 0f, false));
        }
    }

    public void StartLevel()
    {
        if (!isFading)
        {
            StartCoroutine(Fade(1, 0, levelStartDelay, false));
        }
    }

    public void MainMenuFade()
    {
        if (!isFading)
        {
            StartCoroutine(Fade(1, 0, mainMenuDelay, true));
        }
    }

    private IEnumerator Fade(float startAlpha, float targetAlpha, float initialDelay, bool isMainMenu)
    {
        isFading = true;

        if (fadeImage != null && !fadeImage.gameObject.activeInHierarchy)
        {
            fadeImage.gameObject.SetActive(true);
        }

        if (initialDelay > 0f)
        {
            yield return new WaitForSeconds(initialDelay);
        }

        float timer = 0;
        Color currentColor = fadeImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = timer / fadeDuration;
            currentColor.a = Mathf.Lerp(startAlpha, targetAlpha, progress);
            fadeImage.color = currentColor;
            yield return null;
        }

        currentColor.a = targetAlpha;
        fadeImage.color = currentColor;

        if (isMainMenu && mainMenuFirst)
        {
            fadeImage.gameObject.SetActive(false);
            mainMenuFirst = false;
        }

        isFading = false;
    }
}