using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class PlayerPickupCollider : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private PlayerPickuper playerPickuper;

        private void Start()
        {
            if (playerPickuper == null)
            {
                Debug.LogError("No playerPickuper in PlayerPickupCollider");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (LayerMask.NameToLayer("Pickups") == other.gameObject.layer)
            {
                if (other.GetComponent<PickupController>().TryToPickup())
                {
                    playerPickuper.PickUpObject(other.gameObject);
                }
            }
        }
    }
}