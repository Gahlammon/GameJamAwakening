using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class UIInventoryWheel : Singleton<UIInventoryWheel>
    {
        [Header("References")]
        [SerializeField]
        private List<UIInventoryItem> items = new List<UIInventoryItem>();

        [Header("Config")]
        [SerializeField]
        private float rotationDuration = 0.1f;

        private Player.PlayerInventory playerInventory;

        public void SetPlayerInventory(Player.PlayerInventory playerInventory)
        {
            this.playerInventory = playerInventory;
            this.playerInventory.AddObjectEvent += (_, _) => UpdateSprites();
            this.playerInventory.RemoveObjectEvent += (_, _) => UpdateSprites();
            this.playerInventory.ChangeIndexEvent += (_, delta) => RotateInventory(delta);
            UpdateSprites();
        }

        public void RotateInventory(int dir)
        {
            StartCoroutine(RotationCoroutine(dir));
        }

        private IEnumerator RotationCoroutine(int dir)
        {
            float timer = 0;
            while((timer += Time.deltaTime) < rotationDuration)
            {
                float zRotation = Mathf.Lerp(0, 15*dir, timer / rotationDuration);
                transform.rotation = Quaternion.Euler(0, 0, zRotation);
                yield return null;
            }
            UpdateSprites();
            transform.rotation = Quaternion.identity;
        }

        private void UpdateSprites()
        {
            List<(Sprite, int)> inventoryItems = playerInventory.GetInventoryItems();
            for (int i = 0; i < items.Count; i++)
            {
                items[i].SetSprite(inventoryItems[i].Item1, inventoryItems[i].Item2);
            }
        }
    }
}