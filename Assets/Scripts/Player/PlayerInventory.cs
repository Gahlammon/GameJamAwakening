using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerInventory : MonoBehaviour
    {
        [System.Serializable]
        private class Item
        {
            public GameObject Prefab;
            public int Amount;

            public Item(GameObject prefab, int amount)
            {
                Prefab = prefab;
                Amount = amount;
            }
        }

        private int index = 0;
        [SerializeField]
        private List<Item> inventory = new List<Item>();

        public event System.EventHandler AddObjectEvent;
        public event System.EventHandler RemoveObjectEvent;
        public event System.EventHandler<int> ChangeIndexEvent;

        public void ChangeIndex(int delta)
        {
            index = ClampIndex(index + delta);
            ChangeIndexEvent?.Invoke(this, delta);
        }

        public void AddObject(GameObject gameObject)
        {
            PrefabTracker prefabTracker = gameObject.GetComponent<PrefabTracker>();
            if (prefabTracker == null)
            {
                Debug.LogError("No prefabTracker found in added inventory item. This shouldn't happen!");
                return;
            }
            gameObject = prefabTracker.Prefab;

            int i = inventory.FindIndex(item => item.Prefab == gameObject);
            if (i >= 0)
            {
                inventory[i].Amount++;
            }
            else
            {
                inventory.Add(new Item(gameObject, 1));
                index = ClampIndex(index);
            }

            AddObjectEvent?.Invoke(this, null);
        }

        public PickupController.PickupType GetHeldPickupType()
        {
            if (inventory.Count <= 0)
            {
                return PickupController.PickupType.Knife;
            }
            return inventory[index].Prefab.GetComponent<PickupController>().Type;
        }

        public bool RemoveSelectedObject()
        {
            if (inventory.Count <= 0)
            {
                return false;
            }
            if (--inventory[index].Amount <= 0)
            {
                inventory.RemoveAt(index);
                index = ClampIndex(index);
            }
            RemoveObjectEvent?.Invoke(this, null);
            return true;
        }

        private int ClampIndex(int index)
        {
            if (inventory.Count <= 0)
            {
                return 0;
            }

            while(index < 0)
            {
                //Fuck you C#
                index += inventory.Count;
            }

            if (inventory.Count > 0)
            {
                return index % inventory.Count;
            }
            return 0;
        }

        public List<(Sprite, int)> GetInventoryItems()
        {
            List<(Sprite, int)> inventoryItems = new List<(Sprite, int)>();
            for (int i = -3; i <= 3; i++)
            {
                if (inventory.Count <= 0)
                {
                    inventoryItems.Add((null, 0));
                }
                else
                {
                    Item item = inventory[ClampIndex(index + i)];
                    Sprite sprite = item.Prefab.GetComponent<PickupController>().UISprite;
                    inventoryItems.Add((sprite, item.Amount));
                }
            }
            return inventoryItems;
        }
    }
}