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
                ClampIndex();
            }
        }

        public PickupController.PickupType GetHeldPickupType()
        {
            return inventory[index].Prefab.GetComponent<PickupController>().Type;
        }

        public GameObject GetInstancedObject()
        {
            if (inventory.Count <= 0)
            {
                return null;
            }
            GameObject gameObject = Instantiate(inventory[index].Prefab);
            if (--inventory[index].Amount <= 0)
            {
                inventory.RemoveAt(index);
                ClampIndex();
            }
            return gameObject;
        }

        private void ClampIndex()
        {
            while(index < 0)
            {
                //Fuck you C#
                index += inventory.Count;
            }

            if (inventory.Count > 0)
            {
                index = index % inventory.Count;
            }
        }
    }
}