using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerInventory))]
    [RequireComponent(typeof(ServerInventory))]
    public class PlayerPickuper : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Transform pickupDestination;

        [Header("Config")]
        [SerializeField]
        private float pickupDuration;

        private PlayerInventory playerInventory;
        private ServerInventory serverInventory;

        private bool canPickup = true;

        private void Start()
        {
            playerInventory = GetComponent<PlayerInventory>();
            serverInventory = GetComponent<ServerInventory>();
        }

        public void PickUpObject(GameObject gameObject)
        {
            if (!canPickup)
            {
                return;
            }
            StartCoroutine(PickupCoroutine(gameObject));
        }

        private IEnumerator PickupCoroutine(GameObject pickup)
        {
            Rigidbody rigidbody = pickup.GetComponent<Rigidbody>();
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
            serverInventory.PickupPickupServerRpc(pickup.GetComponent<NetworkObject>().NetworkObjectId);
            canPickup = true;
        }
    }
}