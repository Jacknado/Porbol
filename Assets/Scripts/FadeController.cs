    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;

    public class FadeController : MonoBehaviour
    {
        public Image fadeImage;
        public float fadeDuration = 1.0f; // Duration of the fade in seconds

        public void FadeToBlack()
        {
            StartCoroutine(Fade(0, 1, false)); // Fade from transparent to black
        }

        public void FadeFromBlack()
        {
            StartCoroutine(Fade(1, 0, false)); // Fade from black to transparent
        }
        public void StartLevel()
        {
            StartCoroutine(Fade(1, 0, true)); // Fade from black to transparent
        }

        private IEnumerator Fade(float startAlpha, float targetAlpha, bool levelBegin)
        {   
            if (levelBegin)
            {
                yield return new WaitForSeconds(0.5f);
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
        }
    }