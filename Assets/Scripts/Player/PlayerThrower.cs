using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerInventory))]
    [RequireComponent(typeof(PlayerAimer))]
    [RequireComponent(typeof(ServerInventory))]
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

        private ServerInventory serverInventory;
        private PlayerInventory playerInventory;
        private PlayerAimer playerAimer;

        private void Start()
        {
            serverInventory = GetComponent<ServerInventory>();
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
            serverInventory.ThrowPickupServerRpc(playerInventory.GetHeldPickupType(), forceDirection * forceMagnitude);
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