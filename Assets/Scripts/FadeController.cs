    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;

    public class FadeController : MonoBehaviour
    {
        public Image fadeImage;
        public float fadeDuration = 1.0f; // Duration of the fade in seconds

        public void FadeToBlack()
        {
            StartCoroutine(Fade(0, 1)); // Fade from transparent to black
        }

        public void FadeFromBlack()
        {
            StartCoroutine(Fade(1, 0)); // Fade from black to transparent
        }

        private IEnumerator Fade(float startAlpha, float targetAlpha)
        {
            float timer = 0;
            Color currentColor = fadeImage.color;

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                float progress = timer / fadeDuration;
                currentColor.a = Mathf.Lerp(startAlpha, targetAlpha, progress);
                fadeImage.color = currentColor;
                yield return null; // Wait for the next frame
            }

            currentColor.a = targetAlpha; // Ensure final alpha is exact
            fadeImage.color = currentColor;
        }
    }