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
            itemImage.sprite = sprite;
            countText.text = count.ToString();
        }
    }
}