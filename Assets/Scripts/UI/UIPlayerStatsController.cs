using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIPlayerStatsController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Image iconImage;
        [SerializeField]
        private Image partsImage;
        [SerializeField]
        private List<Sprite> partsSprites;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Init(Sprite sprite, int partsCount)
        {
            iconImage.sprite = sprite;
            partsImage.sprite = partsSprites[partsCount];
        }

        public void SetImage(int partsCount, bool hasMedalion)
        {
            if (hasMedalion)
            {
                partsImage.sprite = partsSprites[3];
            }
            else
            {
                partsImage.sprite = partsSprites[partsCount];
            }
        }
    }
}