using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerInventory))]
    [RequireComponent(typeof(PlayerAimer))]
    public class PlayerThrower : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Transform throwOrigin;

        [Header("Config")]
        [SerializeField]
        private Vector2 throwStrength;
        [SerializeField]
        private float maxHoldTime;
        [SerializeField]
        private float throwCooldown;

        private bool canThrow = true;

        private PlayerInventory playerInventory;
        private PlayerAimer playerAimer;

        private void Start()
        {
            playerInventory = GetComponent<PlayerInventory>();
            playerAimer = GetComponent<PlayerAimer>();
        }

        public void Throw(float holdTime)
        {
            if (!canThrow)
            {
                return;
            }

            float forceMagnitude = Mathf.Lerp(throwStrength.x, throwStrength.y, holdTime / maxHoldTime);
            Vector3 forceDirection = playerAimer.AimDirection;
            GameObject gameObject = playerInventory.GetInstancedObject();
            if (gameObject == null)
            {
                return;
            }
            PickupController controller = gameObject.GetComponent<PickupController>();
            controller.transform.position = throwOrigin.position;
            controller.Throw(forceDirection * forceMagnitude);
            canThrow = false;
            StartCoroutine(CooldownCoroutine());
        }

        private IEnumerator CooldownCoroutine()
        {
            yield return new WaitForSeconds(throwCooldown);
            canThrow = true;
        }
    }
}