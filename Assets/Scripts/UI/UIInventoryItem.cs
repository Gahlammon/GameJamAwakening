using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    [ExecuteAlways]
    public class UIInventoryItem : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Image itemImage;
        [SerializeField]
        private TextMeshProUGUI countText;

        private void LateUpdate()
        {
            transform.rotation = Quaternion.identity;
        }

        public void SetSprite(Sprite sprite, int count)
        {
            if (sprite == null)
            {
                Color transparent = new Color(0, 0, 0, 0);
                itemImage.color = transparent;
            }
            else
            {
                itemImage.color = Color.white;
            }
            itemImage.sprite = sprite;
            countText.text = count.ToString();
        }
    }
}