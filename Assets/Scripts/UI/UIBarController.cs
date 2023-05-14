using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIBarController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private List<Image> images = new List<Image>();

        [Header("Config")]
        [SerializeField]
        private bool animateColor;
        [SerializeField]
        private Color startColor;
        [SerializeField]
        private Color endColor;

        [Header("Debug")]
        [SerializeField]
        [Range(0, 1)]
        private float value;

        private void OnValidate()
        {
            ChangeValue(value, 1);
        }

        public void ChangeValueInverted(float current, float maxVal)
        {
            ChangeValue((maxVal - current), maxVal);
        }

        public void ChangeValue(float current, float maxVal)
        {
            foreach (Image image in images)
            {
                float t = current / maxVal;
                image.fillAmount = t;

                if (animateColor)
                {
                    Vector3 color1;
                    Color.RGBToHSV(startColor, out color1.x, out color1.y, out color1.z);
                    Vector3 color2;
                    Color.RGBToHSV(endColor, out color2.x, out color2.y, out color2.z);
                    Vector3 colorHSV = Vector3.Lerp(color1, color2, t);
                    image.color = Color.HSVToRGB(colorHSV.x, colorHSV.y, colorHSV.z);
                }
            }
        }

        public void ImageSetEnabled(int index, bool enabled)
        {
            if (index > images.Count || index < 0)
            {
                return;
            }
            images[index].enabled = enabled;
        }
    }
}