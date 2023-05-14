using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIImageFadeController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private List<Image> images;

        public void Fade(bool fadeIn, float time)
        {
            StartCoroutine(FadeCoroutine(fadeIn, time));
        }

        private IEnumerator FadeCoroutine(bool fadeIn, float time)
        {
            float counter = 0;
            float start = fadeIn ? 0 : 1;
            float end = fadeIn ? 1 : 0;

            while (counter < time)
            {
                counter += Time.unscaledDeltaTime;

                float alpha = Mathf.Lerp(start, end, counter / time);

                foreach (Image image in images)
                {
                    Color color = image.color;
                    color.a = alpha;
                    image.color = color;
                }

                yield return null;
            }
        }
    }
}