using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerInventory))]
    public class PlayerPickuper : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Transform pickupDestination;

        [Header("Config")]
        [SerializeField]
        private float pickupDuration;

        private PlayerInventory playerInventory;

        private bool canPickup = true;

        private void Start()
        {
            playerInventory = GetComponent<PlayerInventory>();
        }

        public void PickUpObject(GameObject gameObject)
        {
            if (!canPickup)
            {
                return;
            }
            Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
            StartCoroutine(PickupCoroutine(rigidbody));
        }

        private IEnumerator PickupCoroutine(Rigidbody rigidbody)
        {
            rigidbody.isKinematic = true;
            Collider collider = rigidbody.GetComponent<Collider>();
            collider.isTrigger = true;
            Vector3 startingPosition = rigidbody.position;
            float timer = 0;
            canPickup = false;
            while((timer += Time.deltaTime) < pickupDuration)
            {
                rigidbody.position = Vector3.Lerp(startingPosition, pickupDestination.position, timer / pickupDuration);
                yield return null;
            }
            playerInventory.AddObject(rigidbody.gameObject);
            Destroy(rigidbody.gameObject);
            canPickup = true;
        }
    }
}